import { Form, Input, Button, Card, Typography, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useMutation } from '@tanstack/react-query';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../auth/AuthContext';
import type { LoginRequest } from '../../api/auth';
import { useTranslation } from 'react-i18next';

const { Title } = Typography;

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const { t } = useTranslation();
  const from = (location.state as { from?: { pathname: string } })?.from?.pathname ?? '/dashboard';

  const mutation = useMutation({
    mutationFn: (credentials: LoginRequest) => login(credentials),
    onSuccess: () => {
      navigate(from, { replace: true });
    },
  });

  return (
    <div
      style={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: '#f0f2f5',
      }}
    >
      <Card style={{ width: 380, boxShadow: '0 4px 20px rgba(0,0,0,0.1)' }}>
        <div style={{ textAlign: 'center', marginBottom: 32 }}>
          <Title level={3} style={{ marginBottom: 4 }}>KasabaKIAT</Title>
          <Typography.Text type="secondary">{t('auth.login')}</Typography.Text>
        </div>

        {mutation.isError && (
          <Alert
            message={t('auth.invalidCredentials')}
            type="error"
            showIcon
            style={{ marginBottom: 16 }}
          />
        )}

        <Form
          layout="vertical"
          onFinish={(values) => mutation.mutate(values)}
          autoComplete="off"
        >
          <Form.Item
            name="login"
            label={t('auth.loginLabel')}
            rules={[{ required: true, message: t('auth.loginRequired') }]}
          >
            <Input
              prefix={<UserOutlined />}
              placeholder={t('auth.loginPlaceholder')}
              size="large"
            />
          </Form.Item>
          <Form.Item
            name="password"
            label={t('auth.passwordLabel')}
            rules={[{ required: true, message: t('auth.passwordRequired') }]}
          >
            <Input.Password
              prefix={<LockOutlined />}
              placeholder={t('auth.passwordPlaceholder')}
              size="large"
            />
          </Form.Item>
          <Form.Item style={{ marginBottom: 0 }}>
            <Button
              type="primary"
              htmlType="submit"
              size="large"
              block
              loading={mutation.isPending}
            >
              {t('auth.loginButton')}
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}
