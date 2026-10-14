import axios from 'axios';
import type { InternalAxiosRequestConfig } from 'axios';

if (import.meta.env.PROD && !import.meta.env.VITE_API_URL) {
  console.error(
    '[KasabaKIAT] VITE_API_URL is not set. ' +
    'All API requests will be sent to a relative /api path. ' +
    'Set VITE_API_URL in your .env file before building for production.'
  );
}

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

api.interceptors.request.use((cfg) => {
  const token = sessionStorage.getItem('access_token');
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});

// Track whether a refresh is already in flight so concurrent 401s don't
// each spawn their own refresh request.
let refreshPromise: Promise<string> | null = null;

function clearSession() {
  sessionStorage.removeItem('access_token');
  localStorage.removeItem('refresh_token');
  localStorage.removeItem('current_user');
  window.location.href = '/login';
}

api.interceptors.response.use(
  (res) => res,
  async (err) => {
    const originalRequest = err.config as InternalAxiosRequestConfig & { _retried?: boolean };

    if (err.response?.status !== 401 || originalRequest._retried) {
      return Promise.reject(err);
    }

    const storedRefreshToken = localStorage.getItem('refresh_token');
    if (!storedRefreshToken) {
      clearSession();
      return Promise.reject(err);
    }

    originalRequest._retried = true;

    try {
      // Coalesce concurrent refreshes into one request.
      if (!refreshPromise) {
        refreshPromise = axios
          .post<{ accessToken: string; refreshToken: string }>(
            `${import.meta.env.VITE_API_URL ?? ''}/api/auth/refresh`,
            { refreshToken: storedRefreshToken }
          )
          .then((r) => {
            sessionStorage.setItem('access_token', r.data.accessToken);
            localStorage.setItem('refresh_token', r.data.refreshToken);
            return r.data.accessToken;
          })
          .finally(() => { refreshPromise = null; });
      }

      const newAccessToken = await refreshPromise;
      originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
      return api(originalRequest);
    } catch {
      clearSession();
      return Promise.reject(err);
    }
  }
);

export default api;
