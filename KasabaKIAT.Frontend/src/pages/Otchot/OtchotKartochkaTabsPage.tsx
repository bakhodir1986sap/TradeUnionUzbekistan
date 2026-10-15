import { useState } from 'react';
import { Tabs, Table, Button, Row, Col, Input, Select, DatePicker, Typography, Space } from 'antd';
import { SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import dayjs from 'dayjs';
import { otchotApi } from '../../api/otchot';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Title } = Typography;
const { RangePicker } = DatePicker;

function OrgDateRangeTab({ queryKey, fetcher, paramLabel = 'ID организации', paramName = 'idOrg' }: {
  queryKey: string;
  fetcher: (p: Record<string, unknown>) => Promise<unknown[]>;
  paramLabel?: string;
  paramName?: string;
}) {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [param, setParam] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () =>
    setApplied({ ...(param && { [paramName]: param }), ...(beginDate && { beginDate }), ...(endDate && { endDate }) });

  const handleClear = () => { setParam(''); setBeginDate(''); setEndDate(''); setApplied({}); };

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col><Input placeholder={paramLabel} value={param} onChange={e => setParam(e.target.value)} style={{ width: 180 }} /></Col>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={handleClear}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function DateRangeSimpleTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
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

function PoiskPutyovkiTab() {
  const [numPut, setNumPut] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-poisk-putyovki', applied],
    queryFn: () => otchotApi.getPoiskPutyovki(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col><Input placeholder="№ путевки" value={numPut} onChange={e => setNumPut(e.target.value)} style={{ width: 180 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={() => setApplied({ ...(numPut && { numPut }) })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setNumPut(''); setApplied({}); }}>Сброс</Button></Col>
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

function TalabnomalarViloyatTab() {
  const currentYear = new Date().getFullYear();
  const [yil, setYil] = useState(currentYear);
  const [soato, setSoato] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-talabnomalar-viloyat', applied],
    queryFn: () => otchotApi.getTalabnomalarViloyat(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <Select value={yil} onChange={setYil} style={{ width: 100 }}>
            {Array.from({ length: 6 }, (_, i) => currentYear - i).map(y => <Select.Option key={y} value={y}>{y}</Select.Option>)}
          </Select>
        </Col>
        <Col><Input placeholder="SOATO" value={soato} onChange={e => setSoato(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={() => setApplied({ yil, ...(soato && { soato }) })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYil(currentYear); setSoato(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

export function OtchotKartochkaTabsPage() {
  const items = [
    { key: '1', label: 'Карточка организации', children: <OrgDateRangeTab queryKey="report-kartochka-org" fetcher={otchotApi.getKartochkaOrg} paramLabel="ИНН организации" paramName="orgInn" /> },
    { key: '2', label: 'История (карточка)', children: <OrgDateRangeTab queryKey="report-kartochka-history" fetcher={otchotApi.getKartochkaHistory} paramLabel="ID организации" paramName="idOrg" /> },
    { key: '3', label: 'Оплата (карточка)', children: <OrgDateRangeTab queryKey="report-kartochka-oplata" fetcher={otchotApi.getKartochkaOplata} paramLabel="ID организации" paramName="idOrg" /> },
    { key: '4', label: 'Мед. регистрация', children: <OrgDateRangeTab queryKey="report-med-registraciya" fetcher={otchotApi.getMedRegistraciya} paramLabel="ID организации" paramName="idOrg" /> },
    { key: '5', label: 'Учет стоимости', children: <DateRangeSimpleTab queryKey="report-uchet-stoimosti" fetcher={otchotApi.getUchetStoimosti} /> },
    { key: '6', label: 'Поиск путевки', children: <PoiskPutyovkiTab /> },
    { key: '7', label: 'Остаток (нижестоящих)', children: <NoParamsTab queryKey="report-ostatok-nizhestvuyushix" fetcher={otchotApi.getOstatokNizhestvuyushix} /> },
    { key: '8', label: 'Талабномалар (вилоят)', children: <TalabnomalarViloyatTab /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Карточки / Прочее</Title>
      <Tabs items={items} />
    </div>
  );
}
