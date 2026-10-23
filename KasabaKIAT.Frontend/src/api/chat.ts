import api from './axios';

export type ChatMessage = { role: 'user' | 'assistant'; content: string };

async function getToken(): Promise<string | null> {
  let token = sessionStorage.getItem('access_token');
  if (token) return token;

  // access_token missing (e.g. hard reload) — use refresh_token to get a new one.
  const refreshToken = localStorage.getItem('refresh_token');
  if (!refreshToken) return null;

  try {
    const res = await api.post<{ accessToken: string; refreshToken: string }>(
      '/api/auth/refresh',
      { refreshToken }
    );
    sessionStorage.setItem('access_token', res.data.accessToken);
    localStorage.setItem('refresh_token', res.data.refreshToken);
    token = res.data.accessToken;
  } catch {
    // refresh failed — session truly expired
  }
  return token;
}

export const chatApi = {
  streamMessage: async (
    message: string,
    history: ChatMessage[],
    onDelta: (delta: string) => void,
    onDone: () => void,
    onError: (err: Error) => void,
    signal: AbortSignal
  ): Promise<void> => {
    const token = await getToken();
    if (!token) {
      onError(new Error('Сессия истекла — войдите снова'));
      return;
    }

    let response: Response;
    try {
      response = await fetch(
        `${import.meta.env.VITE_API_URL ?? ''}/api/chat/message`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({ message, history }),
          signal,
        }
      );
    } catch (err) {
      if ((err as Error).name === 'AbortError') return;
      onError(err instanceof Error ? err : new Error(String(err)));
      return;
    }

    if (!response.ok) {
      onError(new Error(`HTTP ${response.status}`));
      return;
    }
    if (!response.body) {
      onError(new Error('No response body'));
      return;
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    let buffer = '';

    try {
      while (true) {
        const { done, value } = await reader.read();
        if (done) break;
        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split('\n');
        buffer = lines.pop() ?? '';
        for (const line of lines) {
          if (!line.startsWith('data: ')) continue;
          const data = line.slice(6).trim();
          if (data === '[DONE]') {
            onDone();
            return;
          }
          try {
            const parsed = JSON.parse(data) as { delta?: string; error?: string };
            if (parsed.error) {
              onError(new Error(parsed.error));
              return;
            }
            if (parsed.delta) onDelta(parsed.delta);
          } catch { /* skip malformed SSE lines */ }
        }
      }
      onDone();
    } catch (err) {
      if ((err as Error).name === 'AbortError') return;
      onError(err instanceof Error ? err : new Error(String(err)));
    }
  },

  ingestDocument: (file: File) => {
    const form = new FormData();
    form.append('file', file);
    return api.post('/api/chat/ingest', form, {
      headers: { 'Content-Type': 'multipart/form-data' },
    }).then(r => r.data);
  },
};
