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

// ─── Svodniy Otchot ────────────────────────────────────────────────────────────
function SvodniyOtchotTab() {
  const [beginDt, setBeginDt] = useState('');
  const [endDt, setEndDt] = useState('');
  const [soato, setSoato] = useState('');
  const [branch, setBranch] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-svodniy-otchot', applied],
    queryFn: () => otchotApi.getSvodniyOtchot(applied),
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () =>
    setApplied({
      ...(beginDt && { beginDt }),
      ...(endDt && { endDt }),
      ...(soato && { soato }),
      ...(branch && { branch }),
    });

  const handleClear = () => {
    setBeginDt(''); setEndDt(''); setSoato(''); setBranch('');
    setApplied({});
  };

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker
            format="YYYY-MM-DD"
            value={[beginDt ? dayjs(beginDt) : null, endDt ? dayjs(endDt) : null]}
            onChange={v => { setBeginDt(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDt(v?.[1]?.format('YYYY-MM-DD') ?? ''); }}
          />
        </Col>
        <Col><Input placeholder="SOATO" value={soato} onChange={e => setSoato(e.target.value)} style={{ width: 120 }} /></Col>
        <Col><Input placeholder="Отрасль" value={branch} onChange={e => setBranch(e.target.value)} style={{ width: 120 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={handleClear}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

// ─── Postupleniya ───────────────────────────────────────────────────────────────
function PostupleniyaTab() {
  const [inn, setInn] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-postupleniya', applied],
    queryFn: () => otchotApi.getPostupleniya(applied),
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col><Input placeholder="ИНН" value={inn} onChange={e => setInn(e.target.value)} style={{ width: 180 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={() => setApplied({ ...(inn && { inn }) })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setInn(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

// ─── Postupleniya N ─────────────────────────────────────────────────────────────
function PostupleniyaNTab() {
  const [inn, setInn] = useState('');
  const [zb, setZb] = useState('');
  const [ze, setZe] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-postupleniya-n', applied],
    queryFn: () => otchotApi.getPostupleniyaN(applied),
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ ...(inn && { inn }), ...(zb && { zb }), ...(ze && { ze }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col><Input placeholder="ИНН" value={inn} onChange={e => setInn(e.target.value)} style={{ width: 180 }} /></Col>
        <Col>
          <RangePicker format="YYYY-MM-DD" value={[zb ? dayjs(zb) : null, ze ? dayjs(ze) : null]}
            onChange={v => { setZb(v?.[0]?.format('YYYY-MM-DD') ?? ''); setZe(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setInn(''); setZb(''); setZe(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

// ─── Generic date-range report tab ─────────────────────────────────────────────
function DateRangeTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
  const [bg, setBg] = useState('');
  const [end, setEnd] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ ...(bg && { bg }), ...(end && { end }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD" value={[bg ? dayjs(bg) : null, end ? dayjs(end) : null]}
            onChange={v => { setBg(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEnd(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setBg(''); setEnd(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

// ─── Year+Inn report tab ────────────────────────────────────────────────────────
function YearInnTab({ queryKey, fetcher, paramName = 'inn' }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]>; paramName?: string }) {
  const currentYear = new Date().getFullYear();
  const [yil, setYil] = useState(currentYear);
  const [param, setParam] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ yil, ...(param && { [paramName]: param }) });

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <Select value={yil} onChange={setYil} style={{ width: 100 }}>
            {Array.from({ length: 6 }, (_, i) => currentYear - i).map(y => (
              <Select.Option key={y} value={y}>{y}</Select.Option>
            ))}
          </Select>
        </Col>
        <Col><Input placeholder={paramName.toUpperCase()} value={param} onChange={e => setParam(e.target.value)} style={{ width: 180 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYil(currentYear); setParam(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

// ─── Page ───────────────────────────────────────────────────────────────────────
export function OtchotCoreTabsPage() {
  const items = [
    { key: '1', label: 'Сводный отчет', children: <SvodniyOtchotTab /> },
    { key: '2', label: 'Поступления', children: <PostupleniyaTab /> },
    { key: '3', label: 'Поступления N', children: <PostupleniyaNTab /> },
    { key: '4', label: 'Проводки (детально)', children: <DateRangeTab queryKey="report-provodki-details" fetcher={otchotApi.getProvodkiDetails} /> },
    { key: '5', label: 'Проводки (сводно)', children: <DateRangeTab queryKey="report-provodki-summary" fetcher={otchotApi.getProvodkiSummary} /> },
    { key: '6', label: 'Талабномалар (корхона)', children: <YearInnTab queryKey="report-talabnomalar-korxona" fetcher={otchotApi.getTalabnomalarKorxona} paramName="inn" /> },
    {
      key: '7', label: 'Талабномалар (тармоқ)', children: (
        <YearInnTab queryKey="report-talabnomalar-tarmoq" fetcher={(p) => otchotApi.getTalabnomalarTarmoq(p) as Promise<unknown[]>} paramName="soato" />
      ),
    },
    { key: '8', label: 'Форма 13', children: <YearInnTab queryKey="report-forma13" fetcher={otchotApi.getForma13} paramName="inn" /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Основные отчеты</Title>
      <Tabs items={items} />
    </div>
  );
}
