import { useState, useRef, useEffect, useCallback } from 'react';
import { Drawer, Input, Button, Typography, App, Spin } from 'antd';
import { SendOutlined, StopOutlined, RobotOutlined, UserOutlined } from '@ant-design/icons';
import { chatApi, type ChatMessage } from '../../api/chat';

const { Text } = Typography;

interface Props {
  open: boolean;
  onClose: () => void;
}

export function AiChatPanel({ open, onClose }: Props) {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [input, setInput] = useState('');
  const [isStreaming, setIsStreaming] = useState(false);
  const abortRef = useRef<AbortController | null>(null);
  const bottomRef = useRef<HTMLDivElement>(null);
  const { message: antMessage } = App.useApp();

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const handleStop = useCallback(() => {
    abortRef.current?.abort();
    setIsStreaming(false);
  }, []);

  const handleSend = useCallback(async () => {
    const text = input.trim();
    if (!text || isStreaming) return;

    const userMsg: ChatMessage = { role: 'user', content: text };
    const assistantMsg: ChatMessage = { role: 'assistant', content: '' };

    setMessages(prev => [...prev, userMsg, assistantMsg]);
    setInput('');
    setIsStreaming(true);

    const history = messages.map(m => ({ role: m.role, content: m.content }));
    const abort = new AbortController();
    abortRef.current = abort;

    await chatApi.streamMessage(
      text,
      history,
      (delta) => {
        setMessages(prev => {
          const updated = [...prev];
          updated[updated.length - 1] = {
            ...updated[updated.length - 1],
            content: updated[updated.length - 1].content + delta,
          };
          return updated;
        });
      },
      () => setIsStreaming(false),
      (err) => {
        setIsStreaming(false);
        if (err.name !== 'AbortError') {
          antMessage.error('Ошибка при получении ответа от AI ассистента');
        }
      },
      abort.signal
    );
  }, [input, isStreaming, messages, antMessage]);

  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  return (
    <Drawer
      title={
        <span style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <RobotOutlined />
          AI Ассистент
        </span>
      }
      placement="right"
      width={420}
      open={open}
      onClose={onClose}
      styles={{
        body: {
          display: 'flex',
          flexDirection: 'column',
          padding: 0,
          height: '100%',
        },
      }}
    >
      {/* Message scroll area */}
      <div
        style={{
          flex: 1,
          overflowY: 'auto',
          padding: 16,
          display: 'flex',
          flexDirection: 'column',
          gap: 12,
        }}
      >
        {messages.length === 0 && (
          <div style={{ textAlign: 'center', color: '#aaa', marginTop: 40 }}>
            <RobotOutlined style={{ fontSize: 40, color: '#d9d9d9' }} />
            <div style={{ marginTop: 12 }}>
              <Text type="secondary">Задайте вопрос об организации, путевках или санаториях</Text>
            </div>
          </div>
        )}

        {messages.map((msg, i) => (
          <div
            key={i}
            style={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: msg.role === 'user' ? 'flex-end' : 'flex-start',
            }}
          >
            <div
              style={{
                display: 'flex',
                alignItems: 'flex-start',
                gap: 8,
                flexDirection: msg.role === 'user' ? 'row-reverse' : 'row',
                maxWidth: '90%',
              }}
            >
              <div
                style={{
                  width: 28,
                  height: 28,
                  borderRadius: '50%',
                  background: msg.role === 'user' ? '#1677ff' : '#f0f0f0',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  flexShrink: 0,
                  marginTop: 2,
                }}
              >
                {msg.role === 'user'
                  ? <UserOutlined style={{ fontSize: 14, color: '#fff' }} />
                  : <RobotOutlined style={{ fontSize: 14, color: '#555' }} />
                }
              </div>
              <div
                style={{
                  backgroundColor: msg.role === 'user' ? '#1677ff' : '#f5f5f5',
                  color: msg.role === 'user' ? '#fff' : '#000',
                  borderRadius: msg.role === 'user' ? '12px 12px 2px 12px' : '12px 12px 12px 2px',
                  padding: '8px 12px',
                  fontSize: 14,
                  lineHeight: 1.6,
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word',
                }}
              >
                {msg.content}
                {isStreaming && i === messages.length - 1 && msg.role === 'assistant' && msg.content === '' && (
                  <Spin size="small" style={{ marginLeft: 4 }} />
                )}
                {isStreaming && i === messages.length - 1 && msg.role === 'assistant' && msg.content !== '' && (
                  <span style={{ display: 'inline-block', marginLeft: 2, animation: 'blink 1s infinite' }}>▋</span>
                )}
              </div>
            </div>
          </div>
        ))}
        <div ref={bottomRef} />
      </div>

      {/* Input area */}
      <div
        style={{
          borderTop: '1px solid #f0f0f0',
          padding: '12px 16px',
          background: '#fafafa',
          display: 'flex',
          gap: 8,
          alignItems: 'flex-end',
        }}
      >
        <Input.TextArea
          value={input}
          onChange={e => setInput(e.target.value)}
          onKeyDown={handleKeyDown}
          placeholder="Задайте вопрос... (Enter — отправить, Shift+Enter — новая строка)"
          autoSize={{ minRows: 1, maxRows: 4 }}
          disabled={isStreaming}
          style={{ flex: 1, resize: 'none' }}
        />
        {isStreaming ? (
          <Button
            danger
            icon={<StopOutlined />}
            onClick={handleStop}
            style={{ flexShrink: 0 }}
          />
        ) : (
          <Button
            type="primary"
            icon={<SendOutlined />}
            onClick={handleSend}
            disabled={!input.trim()}
            style={{ flexShrink: 0 }}
          />
        )}
      </div>
    </Drawer>
  );
}
