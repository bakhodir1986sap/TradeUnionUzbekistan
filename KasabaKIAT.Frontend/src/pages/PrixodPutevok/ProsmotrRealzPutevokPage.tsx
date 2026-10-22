import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Tabs, Typography, App, Upload, Descriptions,
  DatePicker, Select, Row, Col,
} from 'antd';
import type { UploadFile } from 'antd';
import { UploadOutlined, RollbackOutlined, SearchOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { TextArea } = Input;
const { Title } = Typography;
const { RangePicker } = DatePicker;

// ─── Types ────────────────────────────────────────────────────────────────────

interface RealzPutRaw {
  iD: number;
  num_Put?: string;
  idSan?: number;
  name_San?: string;
  idOrg?: number;
  name_Org?: string;
  dataRealizacii?: string;
  symma?: number;
  talon?: string;
  statusVozvrat?: number;
}

interface RealzPutevokRow {
  id: number;
  numPut: string;
  sanat: string;
  dataNach: string;
  dataKon: string;
  price: number;
  kolvo: number;
  obshayaStoimost: number;
}

interface RealzPutevokDetail {
  id: number;
  numPut: string;
  sanat: string;
  dataNach: string;
  dataKon: string;
  dataRealizacii: string;
  poluchenoOt: string;
  price: number;
  oplachenoProcent: number;
  nakladnaya: string;
  summaOplaty: number;
  kodOrg: string;
  innOrg: string;
  otrasl: string;
  nazvOrg: string;
  fio: string;
  dolzhnost: string;
  innSotr: string;
  primechanie: string;
  talon: string;
  poluchatel: string;
  nomDov: string;
  dateDov: string;
  srokIst: string;
}

interface Sanatorium {
  id: number;
  name: string;
}

interface Filters {
  sanId: number | null;
  numPut: string;
  dateFrom: string;
  dateTo: string;
}

const defaultFilters: Filters = { sanId: null, numPut: '', dateFrom: '', dateTo: '' };

// ─── Component ────────────────────────────────────────────────────────────────

export function ProsmotrRealzPutevokPage() {
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const [filters, setFilters] = useState<Filters>(defaultFilters);
  const [applied, setApplied] = useState<Filters>(defaultFilters);

  const [detailOpen, setDetailOpen] = useState(false);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [vozvratOpen, setVozvratOpen] = useState(false);
  const [vozvratFileList, setVozvratFileList] = useState<UploadFile[]>([]);
  const [prichina, setPrichina] = useState('');
  const [dovFileList, setDovFileList] = useState<UploadFile[]>([]);
  const [talonForm] = Form.useForm();
  const [dovForm] = Form.useForm();

  // ─── Queries ──────────────────────────────────────────────────────────────

  const { data: sanatoriums = [] } = useQuery<Sanatorium[]>({
    queryKey: ['sanatoriums'],
    queryFn: () => api.get('/api/sanatoriums').then(r => r.data),
  });

  const { data: rows = [], isLoading, isError, refetch } = useQuery<RealzPutevokRow[]>({
    queryKey: ['prosmotr-realz-putevok', applied],
    queryFn: async () => {
      const params: Record<string, string> = {};
      if (applied.sanId) params.sanId = String(applied.sanId);
      if (applied.numPut) params.numPut = applied.numPut;
      if (applied.dateFrom) params.dateFrom = applied.dateFrom;
      if (applied.dateTo) params.dateTo = applied.dateTo;
      const raw: RealzPutRaw[] = await api.get('/api/prosmotr-realz-putevok', { params }).then(r => r.data);
      return raw.map(r => ({
        id: r.iD,
        numPut: r.num_Put ?? '',
        sanat: r.name_San ?? '',
        dataNach: r.dataRealizacii ?? '',
        dataKon: '',
        price: r.symma ?? 0,
        kolvo: 1,
        obshayaStoimost: r.symma ?? 0,
      }));
    },
  });

  const { data: detail, isLoading: detailLoading } = useQuery<RealzPutevokDetail>({
    queryKey: ['prosmotr-realz-putevok-detail', selectedId],
    queryFn: () => api.get(`/api/prosmotr-realz-putevok/${selectedId}`).then(r => r.data),
    enabled: selectedId !== null && detailOpen,
  });

  // ─── Mutations ────────────────────────────────────────────────────────────

  const talonMutation = useMutation({
    mutationFn: (values: { talon: string }) =>
      api.put(`/api/prosmotr-realz-putevok/${selectedId}/talon`, values).then(r => r.data),
    onSuccess: () => {
      message.success('Талон сохранён');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok-detail', selectedId] });
    },
    onError: () => message.error('Ошибка при сохранении талона'),
  });

  const dovMutation = useMutation({
    mutationFn: (values: { poluchatel: string; nomDov: string; dateDov: string; srokIst: string }) =>
      api.put(`/api/prosmotr-realz-putevok/${selectedId}/doverenost`, values).then(r => r.data),
    onSuccess: () => {
      message.success('Доверенность сохранена');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok-detail', selectedId] });
    },
    onError: () => message.error('Ошибка при сохранении доверенности'),
  });

  const vozvratMutation = useMutation({
    mutationFn: ({ file, prichina }: { file: File; prichina: string }) => {
      const fd = new FormData();
      fd.append('file', file);
      fd.append('prichina', prichina);
      return api.post(`/api/prosmotr-realz-putevok/${selectedId}/vozvrat`, fd, {
        headers: { 'Content-Type': 'multipart/form-data' },
      }).then(r => r.data);
    },
    onSuccess: () => {
      message.success('Путевка возвращена');
      setVozvratOpen(false);
      setVozvratFileList([]);
      setPrichina('');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok'] });
      setDetailOpen(false);
    },
    onError: () => message.error('Ошибка при возврате путевки'),
  });

  // ─── Handlers ─────────────────────────────────────────────────────────────

  const hasActiveFilters =
    applied.sanId !== null || applied.numPut !== '' || applied.dateFrom !== '' || applied.dateTo !== '';

  const handleApply = () => setApplied({ ...filters });

  const handleClear = () => {
    setFilters(defaultFilters);
    setApplied(defaultFilters);
  };

  const openDetail = (rec: RealzPutevokRow) => {
    setSelectedId(rec.id);
    talonForm.resetFields();
    dovForm.resetFields();
    setDovFileList([]);
    setDetailOpen(true);
  };

  const closeDetail = () => {
    setDetailOpen(false);
    setSelectedId(null);
  };

  const handleDetailAfterOpen = () => {
    if (detail) {
      talonForm.setFieldsValue({ talon: detail.talon });
      dovForm.setFieldsValue({
        poluchatel: detail.poluchatel,
        nomDov: detail.nomDov,
        dateDov: detail.dateDov ? dayjs(detail.dateDov) : undefined,
        srokIst: detail.srokIst ? dayjs(detail.srokIst) : undefined,
      });
    }
  };

  const handleVozvratSubmit = () => {
    if (vozvratFileList.length === 0) { message.warning('Прикрепите файл'); return; }
    if (!prichina.trim()) { message.warning('Укажите причину возврата'); return; }
    vozvratMutation.mutate({ file: vozvratFileList[0].originFileObj as File, prichina });
  };

  // ─── Columns ──────────────────────────────────────────────────────────────

  const columns = [
    { title: '№', key: 'idx', width: 55, render: (_: unknown, __: RealzPutevokRow, i: number) => i + 1 },
    { title: 'Номер путевок', dataIndex: 'numPut', key: 'numPut' },
    { title: 'Санаторий', dataIndex: 'sanat', key: 'sanat' },
    {
      title: 'Дата реализации',
      dataIndex: 'dataNach',
      key: 'dataNach',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Цена',
      dataIndex: 'price',
      key: 'price',
      align: 'right' as const,
      render: (v: number) => v != null ? v.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
    {
      title: 'Общая стоимость',
      dataIndex: 'obshayaStoimost',
      key: 'obshayaStoimost',
      align: 'right' as const,
      render: (v: number) => v != null ? v.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
  ];

  // ─── Detail tabs ──────────────────────────────────────────────────────────

  const tab1 = (
    <Descriptions bordered column={1} size="small">
      <Descriptions.Item label="Номер путевки">{detail?.numPut}</Descriptions.Item>
      <Descriptions.Item label="Санаторий">{detail?.sanat}</Descriptions.Item>
      <Descriptions.Item label="Дата реализации">
        {detail?.dataRealizacii ? dayjs(detail.dataRealizacii).format('DD.MM.YYYY') : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Получено от">{detail?.poluchenoOt}</Descriptions.Item>
      <Descriptions.Item label="Цена">
        {detail?.price != null ? detail.price.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Оплачено, %">{detail?.oplachenoProcent}</Descriptions.Item>
      <Descriptions.Item label="Накладная">{detail?.nakladnaya}</Descriptions.Item>
      <Descriptions.Item label="Сумма оплаты">
        {detail?.summaOplaty != null ? detail.summaOplaty.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : ''}
      </Descriptions.Item>
    </Descriptions>
  );

  const tab2 = (
    <div>
      <Descriptions bordered column={1} size="small" style={{ marginBottom: 16 }}>
        <Descriptions.Item label="Код организации">{detail?.kodOrg}</Descriptions.Item>
        <Descriptions.Item label="ИНН организации">{detail?.innOrg}</Descriptions.Item>
        <Descriptions.Item label="Отрасль">{detail?.otrasl}</Descriptions.Item>
        <Descriptions.Item label="Название организации">{detail?.nazvOrg}</Descriptions.Item>
        <Descriptions.Item label="ФИО">{detail?.fio}</Descriptions.Item>
        <Descriptions.Item label="Должность">{detail?.dolzhnost}</Descriptions.Item>
        <Descriptions.Item label="ИНН сотрудника">{detail?.innSotr}</Descriptions.Item>
        <Descriptions.Item label="Примечание">{detail?.primechanie}</Descriptions.Item>
      </Descriptions>
      <Form form={talonForm} layout="inline" onFinish={(v: { talon: string }) => talonMutation.mutate(v)}>
        <Form.Item name="talon" label="Талон" style={{ flex: 1 }}>
          <Input placeholder="Талон" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={talonMutation.isPending}>Сохранить</Button>
        </Form.Item>
      </Form>
    </div>
  );

  const tab3 = (
    <Form
      form={dovForm}
      layout="vertical"
      onFinish={(v: { poluchatel: string; nomDov: string; dateDov: dayjs.Dayjs; srokIst: dayjs.Dayjs }) =>
        dovMutation.mutate({
          poluchatel: v.poluchatel,
          nomDov: v.nomDov,
          dateDov: v.dateDov?.toISOString() ?? '',
          srokIst: v.srokIst?.toISOString() ?? '',
        })
      }
    >
      <Form.Item name="poluchatel" label="Получатель"><Input /></Form.Item>
      <Form.Item name="nomDov" label="Номер доверенности"><Input /></Form.Item>
      <Form.Item name="dateDov" label="Дата"><DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" /></Form.Item>
      <Form.Item name="srokIst" label="Срок истечения"><DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" /></Form.Item>
      <Form.Item label="Файл (PDF)">
        <Upload fileList={dovFileList} accept=".pdf" beforeUpload={() => false}
          onChange={({ fileList }) => setDovFileList(fileList)} maxCount={1}>
          <Button icon={<UploadOutlined />}>Загрузить PDF</Button>
        </Upload>
      </Form.Item>
      <Form.Item>
        <Button type="primary" htmlType="submit" loading={dovMutation.isPending}>Сохранить</Button>
      </Form.Item>
    </Form>
  );

  // ─── Render ───────────────────────────────────────────────────────────────

  return (
    <div style={{ padding: 24 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Просмотр реализованных путевок</Title>
      </div>

      {/* Filters */}
      <Row gutter={12} style={{ marginBottom: 12 }} align="middle">
        <Col xs={24} sm={7}>
          <Select
            style={{ width: '100%' }}
            placeholder="Санаторий"
            allowClear
            value={filters.sanId}
            onChange={v => setFilters(f => ({ ...f, sanId: v ?? null }))}
            options={sanatoriums.map(s => ({ label: s.name, value: s.id }))}
          />
        </Col>
        <Col xs={24} sm={6}>
          <Input
            placeholder="Номер путевки"
            value={filters.numPut}
            onChange={e => setFilters(f => ({ ...f, numPut: e.target.value }))}
            allowClear
          />
        </Col>
        <Col xs={24} sm={7}>
          <RangePicker
            style={{ width: '100%' }}
            format="DD.MM.YYYY"
            value={filters.dateFrom && filters.dateTo ? [dayjs(filters.dateFrom), dayjs(filters.dateTo)] : null}
            onChange={dates => {
              if (dates?.[0] && dates?.[1]) {
                setFilters(f => ({ ...f, dateFrom: dates[0]!.format('YYYY-MM-DD'), dateTo: dates[1]!.format('YYYY-MM-DD') }));
              } else {
                setFilters(f => ({ ...f, dateFrom: '', dateTo: '' }));
              }
            }}
          />
        </Col>
        <Col>
          <Space>
            <Button type="primary" icon={<SearchOutlined />} onClick={handleApply}>Найти</Button>
            {hasActiveFilters && (
              <Button icon={<ClearOutlined />} onClick={handleClear}>Сбросить</Button>
            )}
          </Space>
        </Col>
      </Row>

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table<RealzPutevokRow>
        rowKey="id"
        columns={columns}
        dataSource={rows}
        loading={isLoading}
        size="small"
        bordered
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: total => `Всего: ${total}` }}
        onRow={rec => ({ onClick: () => openDetail(rec), style: { cursor: 'pointer' } })}
        scroll={{ x: 'max-content' }}
      />

      {/* Detail modal */}
      <Modal
        title={`Путевка: ${detail?.numPut ?? '...'}`}
        open={detailOpen}
        onCancel={closeDetail}
        footer={
          <Space>
            <Button danger icon={<RollbackOutlined />} onClick={() => setVozvratOpen(true)}>
              Возврат путевки
            </Button>
            <Button onClick={closeDetail}>Закрыть</Button>
          </Space>
        }
        width={720}
        afterOpenChange={open => { if (open) handleDetailAfterOpen(); }}
        destroyOnClose
      >
        {detailLoading ? (
          <div style={{ textAlign: 'center', padding: 32 }}>Загрузка...</div>
        ) : (
          <Tabs defaultActiveKey="1" items={[
            { key: '1', label: 'Информация о путевке', children: tab1 },
            { key: '2', label: 'Организация и сотрудник', children: tab2 },
            { key: '3', label: 'Доверенность', children: tab3 },
          ]} />
        )}
      </Modal>

      {/* Vozvrat modal */}
      <Modal
        title="Возврат путевки"
        open={vozvratOpen}
        onCancel={() => { setVozvratOpen(false); setVozvratFileList([]); setPrichina(''); }}
        footer={
          <Space>
            <Button onClick={() => { setVozvratOpen(false); setVozvratFileList([]); setPrichina(''); }}>
              Отмена
            </Button>
            <Button type="primary" danger loading={vozvratMutation.isPending} onClick={handleVozvratSubmit}>
              Подтвердить возврат
            </Button>
          </Space>
        }
        width={480}
        destroyOnClose
      >
        <div style={{ marginBottom: 16 }}>
          <div style={{ marginBottom: 8, fontWeight: 500 }}>Причина возврата</div>
          <TextArea rows={4} value={prichina} onChange={e => setPrichina(e.target.value)}
            placeholder="Укажите причину возврата..." />
        </div>
        <div>
          <div style={{ marginBottom: 8, fontWeight: 500 }}>Прикрепить документ</div>
          <Upload fileList={vozvratFileList} beforeUpload={() => false}
            onChange={({ fileList }) => setVozvratFileList(fileList)} maxCount={1}>
            <Button icon={<UploadOutlined />}>Загрузить файл</Button>
          </Upload>
        </div>
      </Modal>
    </div>
  );
}
