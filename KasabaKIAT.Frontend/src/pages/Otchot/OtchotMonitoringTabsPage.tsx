import { useState } from 'react';
import { Tabs, Table, Button, Row, Col, Select, Typography, Space } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import { otchotApi } from '../../api/otchot';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Title } = Typography;

function YearKvartalTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
  const currentYear = new Date().getFullYear();
  const [year, setYear] = useState(currentYear);
  const [kvartal, setKvartal] = useState(1);
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <Select value={year} onChange={setYear} style={{ width: 100 }}>
            {Array.from({ length: 6 }, (_, i) => currentYear - i).map(y => <Select.Option key={y} value={y}>{y}</Select.Option>)}
          </Select>
        </Col>
        <Col>
          <Select value={kvartal} onChange={setKvartal} style={{ width: 120 }}>
            {[1, 2, 3, 4].map(q => <Select.Option key={q} value={q}>Квартал {q}</Select.Option>)}
          </Select>
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={() => setApplied({ year, kvartal })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYear(currentYear); setKvartal(1); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

export function OtchotMonitoringTabsPage() {
  const items = [
    { key: '1', label: 'Охват', children: <YearKvartalTab queryKey="report-monitoring-oxvat" fetcher={otchotApi.getMonitoringOxvat} /> },
    { key: '2', label: 'Нарушения', children: <YearKvartalTab queryKey="report-monitoring-narusheniya" fetcher={otchotApi.getMonitoringNarusheniya} /> },
    { key: '3', label: 'Перерасход (план)', children: <YearKvartalTab queryKey="report-monitoring-pererasxod-plan" fetcher={otchotApi.getMonitoringPererasxodPlan} /> },
    { key: '4', label: 'Перерасход (путевки)', children: <YearKvartalTab queryKey="report-monitoring-pererasxod-putyovki" fetcher={otchotApi.getMonitoringPererasxodPutyovki} /> },
    { key: '5', label: 'Казначейство', children: <YearKvartalTab queryKey="report-monitoring-kaznacheystvo" fetcher={otchotApi.getMonitoringKaznacheystvo} /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Мониторинг</Title>
      <Tabs items={items} />
    </div>
  );
}
