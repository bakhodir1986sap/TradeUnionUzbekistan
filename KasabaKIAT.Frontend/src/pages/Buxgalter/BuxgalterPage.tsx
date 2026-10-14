import { Tabs, Typography } from 'antd';
import { useTranslation } from 'react-i18next';

function PlaceholderTab({ title }: { title: string }) {
  return (
    <div style={{ padding: 24, textAlign: 'center', color: '#aaa' }}>
      <Typography.Text type="secondary">{title} — в разработке</Typography.Text>
    </div>
  );
}

export function BuxgalterPage() {
  const { t } = useTranslation();

  const tabItems = [
    { key: 'input', label: t('nav.vvodDannix'), children: <PlaceholderTab title="Ввод данных" /> },
    { key: 'view', label: t('nav.prosmotrDannix'), children: <PlaceholderTab title="Просмотр данных" /> },
    { key: 'provodki', label: t('nav.provodki'), children: <PlaceholderTab title="Проводки" /> },
  ];

  return (
    <div>
      <Typography.Title level={4} style={{ marginBottom: 16 }}>{t('nav.buxgalteriya')}</Typography.Title>
      <Tabs items={tabItems} />
    </div>
  );
}
