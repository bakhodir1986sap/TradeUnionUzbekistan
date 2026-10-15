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

function GenericDateSanatTab({ queryKey, fetcher }: { queryKey: string; fetcher: (p: Record<string, unknown>) => Promise<unknown[]> }) {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [sanatId, setSanatId] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: [queryKey, applied],
    queryFn: () => fetcher(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () =>
    setApplied({ ...(beginDate && { beginDate }), ...(endDate && { endDate }), ...(sanatId && { sanatId }) });

  const handleClear = () => { setBeginDate(''); setEndDate(''); setSanatId(''); setApplied({}); };

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Input placeholder="ID санатория" value={sanatId} onChange={e => setSanatId(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={handleClear}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function SvodPoPutyovkamTab() {
  const currentYear = new Date().getFullYear();
  const [year, setYear] = useState(currentYear);
  const [kvartal, setKvartal] = useState(1);
  const [otras, setOtras] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-svod-po-putyovkam', applied],
    queryFn: () => otchotApi.getSvodPoPutyovkam(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({ year, kvartal, ...(otras && { otras }) });

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
        <Col><Input placeholder="Отрасль" value={otras} onChange={e => setOtras(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={handleSearch}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setYear(currentYear); setKvartal(1); setOtras(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function SvodniyOtchetPutyovkiTab() {
  const [kvartal, setKvartal] = useState(1);
  const [smfil, setSmfil] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-svodny-otchet-putyovki', applied],
    queryFn: () => otchotApi.getSvodniyOtchetPutyovki(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={8} align="middle" wrap>
        <Col>
          <Select value={kvartal} onChange={setKvartal} style={{ width: 120 }}>
            {[1, 2, 3, 4].map(q => <Select.Option key={q} value={q}>Квартал {q}</Select.Option>)}
          </Select>
        </Col>
        <Col><Input placeholder="Смфил" value={smfil} onChange={e => setSmfil(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Button type="primary" icon={<SearchOutlined />} onClick={() => setApplied({ kvartal, ...(smfil && { smfil }) })}>Поиск</Button></Col>
        <Col><Button icon={<ClearOutlined />} onClick={() => { setKvartal(1); setSmfil(''); setApplied({}); }}>Сброс</Button></Col>
      </Row>
      {isError && <QueryErrorAlert onRetry={refetch} />}
      <Table rowKey={(_, i) => String(i)} columns={cols} dataSource={data} loading={isFetching} size="small" scroll={{ x: 'max-content' }} pagination={{ pageSize: 20, showSizeChanger: true, showTotal: t => `Всего: ${t}` }} />
    </Space>
  );
}

function ZaprosPutyovkiTab() {
  const [beginDate, setBeginDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [idOrg, setIdOrg] = useState('');
  const [idSanat, setIdSanat] = useState('');
  const [lastName, setLastName] = useState('');
  const [numPut, setNumPut] = useState('');
  const [pol, setPol] = useState('');
  const [applied, setApplied] = useState<Record<string, unknown>>({});

  const { data = [], isFetching, isError, refetch } = useQuery<Record<string, unknown>[]>({
    queryKey: ['report-zapros-putyovki', applied],
    queryFn: () => otchotApi.getZaprosPutyovki(applied) as Promise<Record<string, unknown>[]>,
  });

  const cols: ColumnsType<Record<string, unknown>> = data.length
    ? Object.keys(data[0]).map(k => ({ title: k, dataIndex: k, key: k, ellipsis: true }))
    : [];

  const handleSearch = () => setApplied({
    ...(beginDate && { beginDate }), ...(endDate && { endDate }),
    ...(idOrg && { idOrg }), ...(idSanat && { idSanat }),
    ...(lastName && { lastName }), ...(numPut && { numPut }), ...(pol && { pol }),
  });

  const handleClear = () => {
    setBeginDate(''); setEndDate(''); setIdOrg(''); setIdSanat('');
    setLastName(''); setNumPut(''); setPol(''); setApplied({});
  };

  return (
    <Space direction="vertical" style={{ width: '100%' }} size="middle">
      <Row gutter={[8, 8]} align="middle" wrap>
        <Col>
          <RangePicker format="YYYY-MM-DD"
            value={[beginDate ? dayjs(beginDate) : null, endDate ? dayjs(endDate) : null]}
            onChange={v => { setBeginDate(v?.[0]?.format('YYYY-MM-DD') ?? ''); setEndDate(v?.[1]?.format('YYYY-MM-DD') ?? ''); }} />
        </Col>
        <Col><Input placeholder="ID организации" value={idOrg} onChange={e => setIdOrg(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Input placeholder="ID санатория" value={idSanat} onChange={e => setIdSanat(e.target.value)} style={{ width: 130 }} /></Col>
        <Col><Input placeholder="Фамилия" value={lastName} onChange={e => setLastName(e.target.value)} style={{ width: 140 }} /></Col>
        <Col><Input placeholder="№ путевки" value={numPut} onChange={e => setNumPut(e.target.value)} style={{ width: 130 }} /></Col>
        <Col>
          <Select placeholder="Пол" value={pol || undefined} onChange={setPol} allowClear style={{ width: 100 }}>
            <Select.Option value="M">М</Select.Option>
            <Select.Option value="F">Ж</Select.Option>
          </Select>
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

export function OtchotPutyovkiTabsPage() {
  const items = [
    { key: '1', label: 'Оперативка (путевки)', children: <GenericDateSanatTab queryKey="report-op-putyovki" fetcher={otchotApi.getOperativkaPutyovki} /> },
    { key: '2', label: 'Оперативка (остаток)', children: <GenericDateSanatTab queryKey="report-op-ostatok" fetcher={otchotApi.getOperativkaOstatok} /> },
    { key: '3', label: 'Оперативка (приход)', children: <GenericDateSanatTab queryKey="report-op-prixod" fetcher={otchotApi.getOperativkaPrixod} /> },
    { key: '4', label: 'Оперативка (расход)', children: <GenericDateSanatTab queryKey="report-op-rasxod" fetcher={otchotApi.getOperativkaRasxod} /> },
    { key: '5', label: 'Оперативка (возврат)', children: <GenericDateSanatTab queryKey="report-op-vozvrat" fetcher={otchotApi.getOperativkaVozvrat} /> },
    { key: '6', label: 'Свод по путевкам', children: <SvodPoPutyovkamTab /> },
    { key: '7', label: 'Сводный отчет (путевки)', children: <SvodniyOtchetPutyovkiTab /> },
    { key: '8', label: 'Запрос путевок', children: <ZaprosPutyovkiTab /> },
    { key: '9', label: 'Книга владельцев', children: <DateRangeSimpleTab queryKey="report-kniga-vladelsev" fetcher={otchotApi.getKnigaVladelsev} /> },
    { key: '10', label: 'Форма 14', children: <DateRangeSimpleTab queryKey="report-forma14" fetcher={otchotApi.getForma14} /> },
    { key: '11', label: 'Текущий остаток', children: <NoParamsTab queryKey="report-tekushiye-ostatok" fetcher={otchotApi.getTekushiyeOstatok} /> },
  ];

  return (
    <div>
      <Title level={4} style={{ marginBottom: 16 }}>Отчеты по путевкам</Title>
      <Tabs items={items} />
    </div>
  );
}
