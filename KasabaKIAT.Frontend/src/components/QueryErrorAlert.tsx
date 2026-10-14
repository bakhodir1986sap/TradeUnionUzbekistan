import { Alert, Button, Space } from 'antd';
import { useTranslation } from 'react-i18next';

interface QueryErrorAlertProps {
  isError: boolean;
  onRetry?: () => void;
}

export function QueryErrorAlert({ isError, onRetry }: QueryErrorAlertProps) {
  const { t } = useTranslation();

  if (!isError) return null;

  return (
    <Alert
      type="error"
      message={t('common.loadError')}
      style={{ marginBottom: 16 }}
      action={
        onRetry && (
          <Space>
            <Button size="small" onClick={onRetry}>
              {t('common.retry')}
            </Button>
          </Space>
        )
      }
    />
  );
}
