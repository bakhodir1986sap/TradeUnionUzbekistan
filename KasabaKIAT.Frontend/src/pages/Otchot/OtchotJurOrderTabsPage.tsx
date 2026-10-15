import { useState } from 'react';
import { Tabs, Table, Button, Row, Col, Select, DatePicker, Typography, Space } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { otchotApi } from '../../api/otchot';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Title } = Typography;
const { RangePicker } = DatePicker;

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

function JurOrderByTypeTab() {
  const [jorType, setJorType] = useState(4);
  const [bg, setBg] = useState('');
  const [end, setEnd] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-jurorder', applied],
    queryFn: () => otchotApi.getJurOrder(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ jorType, ...(bg && { bg }), ...(end && { end }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <Select value={jorType} onChange={setJorType} style={{ width: 160 }}>
            {[4, 5, 6].map(t => <Select.Option key={t} value={t}>Тип {t}</Select.Option>)}
          </Select>
        </Col>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[bg ? dayjs(bg) : null, end ? dayjs(end) : null]}
            onChange={v => { setBg(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEnd(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setJorType(4); setBg(''); setEnd(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

export function OtchotJurOrderTabsPage() {
  const items = [
    { key: '1', label: 'Журнал-ордер №4', children: <YearKvartalTab queryKey="report-jurorder-4" fetcher={otchotApi.getJurOrder4} /> },
    { key: '2', label: 'Журнал-ордер №5', children: <YearKvartalTab queryKey="report-jurorder-5" fetcher={otchotApi.getJurOrder5} /> },
    { key: '3', label: 'Журнал-ордер №6', children: <YearKvartalTab queryKey="report-jurorder-6" fetcher={otchotApi.getJurOrder6} /> },
    { key: '4', label: 'Журнал-ордер (по типу)', children: <JurOrderByTypeTab /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Журнал-ордера</Title>
      <Tabs items={items} />
    </div>
  );
}
