import { Row, Col, Card, Typography } from 'antd';
import { useTranslation } from 'react-i18next';
import { PrinterOutlined } from '@ant-design/icons';

const reports = [
  { key: 'otchet-1', label: 'Отчет о путевках' },
  { key: 'otchet-2', label: 'Отчет по санаториям' },
  { key: 'otchet-3', label: 'Отчет по регионам' },
  { key: 'otchet-4', label: 'Финансовый отчет' },
  { key: 'otchet-5', label: 'Годовой план (отчет)' },
  { key: 'otchet-6', label: 'Квартальный отчет' },
  { key: 'otchet-7', label: 'Отчет по заявкам' },
  { key: 'otchet-8', label: 'Отчет по пользователям' },
];

export function OtchotPage() {
  const { t } = useTranslation();

  return (
    <div>
      <Typography.Title level={4} style={{ marginBottom: 24 }}>{t('nav.otchety')}</Typography.Title>
      <Row gutter={[16, 16]}>
        {reports.map(r => (
          <Col key={r.key} xs={12} sm={8} md={6}>
            <Card
              hoverable
              style={{ textAlign: 'center', cursor: 'pointer' }}
              styles={{ body: { padding: '20px 12px' } }}
            >
              <PrinterOutlined style={{ fontSize: 32, color: '#fa8c16', marginBottom: 8 }} />
              <Typography.Text style={{ display: 'block', fontSize: 13 }}>{r.label}</Typography.Text>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
}
