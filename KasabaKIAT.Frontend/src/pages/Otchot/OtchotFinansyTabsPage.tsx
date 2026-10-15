import { useState } from 'react';
import { Tabs, Table, Button, Row, Col, Input, DatePicker, Typography, Space } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { otchotApi } from '../../api/otchot';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Title } = Typography;
const { RangePicker } = DatePicker;

function DateRangeTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ ...(beginDate && { beginDate }), ...(endDate && { endDate }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setBeginDate(''); setEndDate(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function DateRangeSoatoTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [soato, setSoato] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () =>
    setApplied({ ...(beginDate && { beginDate }), ...(endDate && { endDate }), ...(soato && { soato }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Input placeholder="SOATO" value={soato} onChange={e => setSoato(e.target.value)} style={{ width: 120 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setBeginDate(''); setEndDate(''); setSoato(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function DateRangeDeptTab() {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [department, setDepartment] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-postupleniya-svodnoye', applied],
    queryFn: () => otchotApi.getPostupleniyaSvodnoye(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () =>
    setApplied({ ...(beginDate && { beginDate }), ...(endDate && { endDate }), ...(department && { department }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Input placeholder="Подразделение" value={department} onChange={e => setDepartment(e.target.value)} style={{ width: 160 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setBeginDate(''); setEndDate(''); setDepartment(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function NoParamsTab({ queryKey, fetcher }: { queryKey: string; fetcher: () => Promise<unknown[]> }) {
  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey],
    queryFn: () => fetcher() as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

export function OtchotFinansyTabsPage() {
  const items = [
    { key: '1', label: 'Поступления ЭСП (отрасли)', children: <DateRangeTab queryKey="report-postupleniya-esp-otrasli" fetcher={otchotApi.getPostupleniyaEspOtrasli} /> },
    { key: '2', label: 'Поступления (сводное)', children: <DateRangeDeptTab /> },
    { key: '3', label: 'Обороты по регионам', children: <DateRangeSoatoTab queryKey="report-oborotka-regionam" fetcher={otchotApi.getOborotkaRegionam} /> },
    { key: '4', label: 'Баланс предприятий', children: <DateRangeTab queryKey="report-balans-predpriyatiy" fetcher={otchotApi.getBalansPredpriyatiy} /> },
    { key: '5', label: 'Дебиторская', children: <NoParamsTab queryKey="report-debitorskaya" fetcher={otchotApi.getDebitorskaya} /> },
    { key: '6', label: 'Организации (отрасли)', children: <NoParamsTab queryKey="report-organizasii-otrasli" fetcher={otchotApi.getOrganizasiiOtrasli} /> },
    { key: '7', label: 'Организации (районы)', children: <NoParamsTab queryKey="report-organizasii-rayony" fetcher={otchotApi.getOrganizasiiRayony} /> },
    { key: '8', label: 'Список организаций', children: <NoParamsTab queryKey="report-spisok-org" fetcher={otchotApi.getSpisokOrg} /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Финансовые отчеты</Title>
      <Tabs items={items} />
    </div>
  );
}
