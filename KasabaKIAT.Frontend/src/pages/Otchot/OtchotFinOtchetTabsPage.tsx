import { useState } from 'react';
import { Tabs, Table, Button, Row, Col, Select, Switch, Typography, Space, Descriptions, Card } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import { otchotApi } from '../../api/otchot';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Title, Text } = Typography;

// ─── FinOtchet ─────────────────────────────────────────────────────────────────

interface TotalOutput {
  finOtchet?: Record<string, number | null>;
  balance?: Record<string, number | null>;
  other?: Record<string, number | null>;
}

function FinOtchetTab() {
  const currentYear = new Date().getFullYear();
  const [year, setYear] = useState(currentYear);
  const [kvartal, setKvartal] = useState(1);
  const [isSvodniy, setIsSvodniy] = useState(false);
  const [applied, setApplied] = useState<Record<string, unknown> | null>(null);

  const { data, isFetching, isError, refetch } = useQuery<TotalOutput>({
    queryKey: ['report-fin-otchet', applied],
    queryFn: () => otchotApi.getFinOtchet(applied as Record<string, unknown>),
    enabled: applied !== null,
  });

  const descItemsFor = (obj: Record<string, number | null> | undefined, title: string) => {
    if (!obj) return null;
    return (
      <Card title={title} size="small" style={{ marginBottom: 12 }}>
        <Descriptions bordered size="small" column={4}>
          {Object.entries(obj)
            .filter(([, v]) => v !== null)
            .map(([k, v]) => (
              <Descriptions.Item key={k} label={k}>{v}</Descriptions.Item>
            ))}
        </Descriptions>
      </Card>
    );
  };

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
        <Col>
          <Space>
            <Text>Сводный:</Text>
            <Switch checked={isSvodniy} onChange={setIsSvodniy} />
          </Space>
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} loading={isFetching} onClick={() => setApplied({ year, kvartal, isSvodniy })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYear(currentYear); setKvartal(1); setIsSvodniy(false); setApplied(null); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      {data && (
        <>
          {descItemsFor(data.finOtchet, 'Финансовый отчет')}
          {descItemsFor(data.balance, 'Баланс')}
          {descItemsFor(data.other, 'Прочее')}
        </>
      )}
    </Space>
  );
}

// ─── GlavnayaKniga ─────────────────────────────────────────────────────────────

interface GlavnayaKnigaResult {
  rows: string[];
  cols: string[];
  data: Record<string, unknown>[];
}

function GlavnayaKnigaTab() {
  const currentYear = new Date().getFullYear();
  const [year, setYear] = useState(currentYear);
  const [kvartal, setKvartal] = useState(1);
  const [isSvodniy, setIsSvodniy] = useState(false);
  const [applied, setApplied] = useState<Record<string, unknown> | null>(null);

  const { data, isFetching, isError, refetch } = useQuery<GlavnayaKnigaResult>({
    queryKey: ['report-glavnaya-kniga', applied],
    queryFn: () => otchotApi.getGlavnayaKniga(applied as Record<string, unknown>),
    enabled: applied !== null,
  });

  const cols: ColumnsType<Record<string, unknown>> = data?.data?.length
    ? Object.keys(data.data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
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
        <Col>
          <Space>
            <Text>Сводный:</Text>
            <Switch checked={isSvodniy} onChange={setIsSvodniy} />
          </Space>
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} loading={isFetching} onClick={() => setApplied({ year, kvartal, isSvodniy })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYear(currentYear); setKvartal(1); setIsSvodniy(false); setApplied(null); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table
        rowKey={(_, i) => String(i)}
        columns={cols}
        dataSource={data?.data ?? []}
        loading={isFetching}
        size="small"
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }}
      />
    </Space>
  );
}

// ─── Page ───────────────────────────────────────────────────────────────────────

export function OtchotFinOtchetTabsPage() {
  const items = [
    { key: '1', label: 'Финансовый отчет', children: <FinOtchetTab /> },
    { key: '2', label: 'Главная книга', children: <GlavnayaKnigaTab /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Фин. отчет / Главная книга</Title>
      <Tabs items={items} />
    </div>
  );
}
