import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, InputNumber, Select, DatePicker,
  Row, Col, Typography, App, Statistic, Card, Checkbox,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined, PrinterOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Option } = Select;

// ─── Types ──────────────────────────────────────────────────────────────────

interface FizLico {
  id: number;
  fio?: string;
  inn?: string;
  address?: string;
  phone?: string;
  passportSeria?: string;
  passportNomer?: string;
  passportKemVydan?: string;
  passportDataVydachi?: string;
}

interface Nakladnoy {
  id: number;
  nomer?: string;
  data?: string;
  fizLicoId?: number;
  fizLicoFio?: string;
  fizLicoInn?: string;
  summa?: number;
  kolvo?: number;
  status?: string;
  provodka?: boolean;
}

interface NakladnoyDetail {
  id: number;
  idVipl?: number;
  recId?: number;
  serialBegin?: string;
  sanatoriumId?: number;
  sanatoriumName?: string;
  restBeginDate?: string;
  price?: number;
  statusPutyovki?: string;
  // convenience aliases kept for columns
  numPut?: string;
  dataNach?: string;
  status?: string;
}

interface Stats {
  totalNakladnoy?: number;
  totalSumma?: number;
  totalVouchers?: number;
  provedeno?: number;
}

interface AvailableVoucher {
  id: number;
  sanatoriumId?: number;
  sanatoriumName?: string;
  numPut?: string;
  dataSan?: string;
  dataSanEnd?: string;
  symmaPut?: number;
}

// ─── FizLicoPickerModal ──────────────────────────────────────────────────────

interface FizLicoPickerModalProps {
  open: boolean;
  onSelect: (rec: FizLico) => void;
  onCancel: () => void;
}

function FizLicoPickerModal({ open, onSelect, onCancel }: FizLicoPickerModalProps) {
  const [search, setSearch] = useState('');

  interface FizLisoRow {
    id: number;
    name_Org?: string;
    inn?: string;
    adress?: string;
    telefon?: string;
    pas_Ser?: string;
    pas_Num?: string;
    pas_KD?: string;
    pas_Data?: string;
  }

  const { data: rawResults = [], isFetching, isError, refetch } = useQuery<FizLisoRow[]>({
    queryKey: ['fizliso-search', search],
    queryFn: () =>
      api.get('/api/fizliso', { params: search ? { search } : undefined }).then(r => r.data),
    enabled: open,
  });

  const results: FizLico[] = rawResults.map(r => ({
    id: r.id,
    fio: r.name_Org,
    inn: r.inn,
    address: r.adress,
    phone: r.telefon,
    passportSeria: r.pas_Ser,
    passportNomer: r.pas_Num,
    passportKemVydan: r.pas_KD,
    passportDataVydachi: r.pas_Data,
  }));

  const pickerColumns = [
    { title: 'ФИО', dataIndex: 'fio' },
    { title: 'ИНН', dataIndex: 'inn', width: 120 },
    { title: 'Адрес', dataIndex: 'address' },
    { title: 'Телефон', dataIndex: 'phone', width: 130 },
    {
      title: 'Паспорт (Серия+Номер)',
      width: 160,
      render: (_: unknown, rec: FizLico) =>
        [rec.passportSeria, rec.passportNomer].filter(Boolean).join(' ') || '—',
    },
    {
      title: '',
      width: 80,
      render: (_: unknown, rec: FizLico) => (
        <Button size="small" type="primary" onClick={() => onSelect(rec)}>
          Выбрать
        </Button>
      ),
    },
  ];

  return (
    <Modal
      title="Выбор физического лица"
      open={open}
      onCancel={onCancel}
      footer={null}
      width={820}
    >
      <Input.Search
        placeholder="Поиск по ФИО, ИНН или серии паспорта"
        value={search}
        onChange={e => setSearch(e.target.value)}
        style={{ marginBottom: 12 }}
        prefix={<SearchOutlined />}
        allowClear
      />
      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table
        columns={pickerColumns}
        dataSource={results}
        rowKey="id"
        loading={isFetching}
        size="small"
        pagination={{ pageSize: 8 }}
        locale={{ emptyText: 'Нет данных. Добавьте физ. лиц через справочник.' }}
      />
    </Modal>
  );
}

// ─── VoucherPickerModal ──────────────────────────────────────────────────────

interface VoucherPickerModalProps {
  open: boolean;
  nakladnoyId: number | null;
  onClose: () => void;
}

function VoucherPickerModal({ open, nakladnoyId, onClose }: VoucherPickerModalProps) {
  const { message } = App.useApp();
  const queryClient = useQueryClient();
  const [search, setSearch] = useState('');
  const [selected, setSelected] = useState<number[]>([]);

  const { data: vouchers = [], isFetching } = useQuery<AvailableVoucher[]>({
    queryKey: ['available-vouchers-fiz', search],
    queryFn: () =>
      api.get(`/api/rasxod-putevok/available-vouchers?search=${encodeURIComponent(search)}`).then(r => r.data),
    enabled: open,
  });

  const addMutation = useMutation({
    mutationFn: (ids: number[]) =>
      api.post(`/api/rasxod-putevok-fiz/${nakladnoyId}/add-vouchers`, { voucherIds: ids }).then(r => r.data),
    onSuccess: () => {
      message.success('Путевки добавлены');
      queryClient.invalidateQueries({ queryKey: ['rasxod-fiz-details'] });
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-fiz'] });
      setSelected([]);
      onClose();
    },
    onError: () => message.error('Ошибка при добавлении путевок'),
  });

  const voucherColumns = [
    {
      title: '',
      width: 40,
      render: (_: unknown, rec: AvailableVoucher) => (
        <Checkbox
          checked={selected.includes(rec.id)}
          onChange={e => {
            setSelected(prev =>
              e.target.checked ? [...prev, rec.id] : prev.filter(id => id !== rec.id)
            );
          }}
        />
      ),
    },
    { title: '№ путевки', dataIndex: 'numPut' },
    { title: 'Санаторий', dataIndex: 'sanatoriumName' },
    {
      title: 'Дата начала',
      dataIndex: 'dataSan',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
    },
    {
      title: 'Дата конца',
      dataIndex: 'dataSanEnd',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
    },
    {
      title: 'Цена',
      dataIndex: 'symmaPut',
      width: 100,
      render: (v: number) => v?.toLocaleString('ru-RU'),
    },
  ];

  return (
    <Modal
      title="Добавить путевки"
      open={open}
      onCancel={() => { setSelected([]); onClose(); }}
      onOk={() => selected.length > 0 && addMutation.mutate(selected)}
      confirmLoading={addMutation.isPending}
      okText={`Добавить (${selected.length})`}
      okButtonProps={{ disabled: selected.length === 0 }}
      width={720}
    >
      <Input.Search
        placeholder="Поиск путевок"
        value={search}
        onChange={e => setSearch(e.target.value)}
        style={{ marginBottom: 12 }}
        allowClear
      />
      <Table
        columns={voucherColumns}
        dataSource={vouchers}
        rowKey="id"
        loading={isFetching}
        size="small"
        pagination={{ pageSize: 8 }}
      />
    </Modal>
  );
}

// ─── Main Page ───────────────────────────────────────────────────────────────

export function RasxodPutyovokFizLisoPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();

  // Filters
  const [filterYear, setFilterYear] = useState<number>(new Date().getFullYear());
  const [filterQuarter, setFilterQuarter] = useState<number>(
    Math.ceil((new Date().getMonth() + 1) / 3)
  );

  // Master selection
  const [selectedNakladnoy, setSelectedNakladnoy] = useState<Nakladnoy | null>(null);
  const [selectedDetailRow, setSelectedDetailRow] = useState<NakladnoyDetail | null>(null);

  // Modals
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [editing, setEditing] = useState<Nakladnoy | null>(null);
  const [fizPickerOpen, setFizPickerOpen] = useState(false);
  const [voucherPickerOpen, setVoucherPickerOpen] = useState(false);

  // FizLico picker result stored in form
  const [selectedFizLico, setSelectedFizLico] = useState<FizLico | null>(null);

  // ── Queries ──────────────────────────────────────────────────────────────

  const { data: masterData, isLoading: masterLoading } = useQuery<{
    nakladnoys: Nakladnoy[];
    stats: Stats;
  }>({
    queryKey: ['rasxod-putevok-fiz', filterYear, filterQuarter],
    queryFn: () =>
      api.get(`/api/rasxod-putevok-fiz?year=${filterYear}&quarter=${filterQuarter}`).then(r => r.data),
  });

  const nakladnoys: Nakladnoy[] = masterData?.nakladnoys ?? [];
  const stats: Stats = masterData?.stats ?? {};

  const { data: details = [], isLoading: detailsLoading } = useQuery<NakladnoyDetail[]>({
    queryKey: ['rasxod-fiz-details', selectedNakladnoy?.id],
    queryFn: () =>
      api.get(`/api/rasxod-putevok-fiz/${selectedNakladnoy!.id}/details`).then(r => r.data),
    enabled: !!selectedNakladnoy,
    staleTime: 0,
  });

  // ── Mutations ─────────────────────────────────────────────────────────────

  const saveMutation = useMutation({
    mutationFn: (values: { year?: number; quarter?: number; numDoc?: unknown; dataPost?: string; fizLico?: number; nameDov?: unknown }) =>
      editing
        ? api.put(`/api/rasxod-putevok-fiz/${editing.id}`, values).then(r => r.data)
        : api.post('/api/rasxod-putevok-fiz', values).then(r => r.data),
    onSuccess: () => {
      message.success('Сохранено');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-fiz'] });
      setEditModalOpen(false);
      form.resetFields();
      setSelectedFizLico(null);
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/rasxod-putevok-fiz/${id}`),
    onSuccess: () => {
      message.success('Удалено');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-fiz'] });
      if (selectedNakladnoy && selectedNakladnoy.id === deleteMutation.variables) {
        setSelectedNakladnoy(null);
      }
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const provodkaMutation = useMutation({
    mutationFn: (id: number) =>
      api.post(`/api/rasxod-putevok-fiz/${id}/provodka`).then(r => r.data),
    onSuccess: () => {
      message.success('Проводка выполнена');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-fiz'] });
    },
    onError: () => message.error('Ошибка при проводке'),
  });

  const removeVoucherMutation = useMutation({
    mutationFn: (voucherId: number) =>
      api.delete(`/api/rasxod-putevok-fiz/${selectedNakladnoy!.id}/vouchers/${voucherId}`),
    onSuccess: () => {
      message.success('Путевка удалена');
      setSelectedDetailRow(null);
      queryClient.invalidateQueries({ queryKey: ['rasxod-fiz-details'] });
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-fiz'] });
    },
    onError: () => message.error('Ошибка при удалении путевки'),
  });

  // ── Handlers ─────────────────────────────────────────────────────────────

  const openAdd = () => {
    setEditing(null);
    setSelectedFizLico(null);
    form.resetFields();
    setEditModalOpen(true);
  };

  const openEdit = (rec: Nakladnoy) => {
    setEditing(rec);
    setSelectedFizLico({
      id: rec.fizLicoId ?? 0,
      fio: rec.fizLicoFio,
      inn: rec.fizLicoInn,
    });
    form.setFieldsValue({
      ...rec,
      fio: rec.fizLicoFio,
      inn: rec.fizLicoInn,
      data: rec.data ? dayjs(rec.data) : undefined,
    });
    setEditModalOpen(true);
  };

  const confirmDelete = (id: number) =>
    modal.confirm({
      title: 'Удалить накладную?',
      content: 'Это действие нельзя отменить.',
      onOk: () => deleteMutation.mutate(id),
    });

  const handleFizLicoSelect = (fiz: FizLico) => {
    setSelectedFizLico(fiz);
    form.setFieldsValue({
      fio: fiz.fio,
      inn: fiz.inn,
      address: fiz.address,
      phone: fiz.phone,
      passportSeria: fiz.passportSeria,
      passportNomer: fiz.passportNomer,
      passportKemVydan: fiz.passportKemVydan,
      passportDataVydachi: fiz.passportDataVydachi
        ? dayjs(fiz.passportDataVydachi)
        : undefined,
    });
    setFizPickerOpen(false);
  };

  const handleFormFinish = (values: Record<string, unknown>) => {
    saveMutation.mutate({
      year: filterYear,
      quarter: filterQuarter,
      numDoc: values.nomer,
      dataPost: values.data ? (values.data as dayjs.Dayjs).toISOString() : undefined,
      fizLico: selectedFizLico?.id,
      nameDov: values.status,
    });
  };

  const handlePrintSchetFaktura = () => {
    if (!selectedNakladnoy) return;
    window.open(`/api/rasxod-putevok-fiz/${selectedNakladnoy.id}/schet-faktura`, '_blank');
  };

  // ── Year options ──────────────────────────────────────────────────────────
  const currentYear = new Date().getFullYear();
  const years = Array.from({ length: 6 }, (_, i) => currentYear - 2 + i);

  // ── Master columns ────────────────────────────────────────────────────────

  const masterColumns = [
    { title: '№', dataIndex: 'nomer', width: 90 },
    { title: 'Дата', dataIndex: 'data', width: 100 },
    { title: 'ФИО', dataIndex: 'fizLicoFio' },
    { title: 'ИНН', dataIndex: 'fizLicoInn', width: 120 },
    { title: 'Кол-во', dataIndex: 'kolvo', width: 70 },
    { title: 'Сумма', dataIndex: 'summa', width: 110, render: (v: number) => v?.toLocaleString('ru-RU') },
    {
      title: 'Проведено',
      dataIndex: 'provodka',
      width: 90,
      render: (v: boolean) => (v ? 'Да' : 'Нет'),
    },
    { title: 'Статус', dataIndex: 'status', width: 90 },
    {
      title: 'Действия',
      width: 140,
      render: (_: unknown, rec: Nakladnoy) => (
        <Space size={4} onClick={e => e.stopPropagation()}>
          <Button
            size="small"
            icon={<EditOutlined />}
            onClick={() => openEdit(rec)}
            title="Редактировать"
          />
          <Button
            size="small"
            danger
            icon={<DeleteOutlined />}
            onClick={() => confirmDelete(rec.id)}
            title="Удалить"
          />
          {!rec.provodka && (
            <Button
              size="small"
              type="link"
              onClick={() => provodkaMutation.mutate(rec.id)}
              loading={provodkaMutation.isPending && provodkaMutation.variables === rec.id}
              title="Провести"
            >
              Пров.
            </Button>
          )}
        </Space>
      ),
    },
  ];

  // ── Detail columns ────────────────────────────────────────────────────────

  const detailColumns = [
    { title: '№ путевки', dataIndex: 'serialBegin' },
    { title: 'Санаторий', dataIndex: 'sanatoriumName' },
    {
      title: 'Дата',
      dataIndex: 'restBeginDate',
      width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '—',
    },
    {
      title: 'Цена',
      dataIndex: 'price',
      width: 110,
      render: (v: number) => v?.toLocaleString('ru-RU'),
    },
    { title: 'Статус', dataIndex: 'statusPutyovki', width: 90 },
  ];

  // ── Render ────────────────────────────────────────────────────────────────

  return (
    <div>
      {/* Header */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>
          Расход путевок — Физ. лицо
        </Typography.Title>
        <Space>
          <Select
            value={filterYear}
            onChange={setFilterYear}
            style={{ width: 90 }}
            placeholder="Год"
          >
            {years.map(y => <Option key={y} value={y}>{y}</Option>)}
          </Select>
          <Select
            value={filterQuarter}
            onChange={setFilterQuarter}
            style={{ width: 120 }}
            placeholder="Квартал"
          >
            {[1, 2, 3, 4].map(q => (
              <Option key={q} value={q}>{q} квартал</Option>
            ))}
          </Select>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
            Добавить
          </Button>
        </Space>
      </div>

      {/* Stats */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col span={6}>
          <Card size="small">
            <Statistic title="Накладных" value={stats.totalNakladnoy ?? 0} />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic
              title="Общая сумма"
              value={stats.totalSumma ?? 0}
              suffix="сум"
              formatter={v => Number(v).toLocaleString('ru-RU')}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic title="Путевок выдано" value={stats.totalVouchers ?? 0} />
          </Card>
        </Col>
        <Col span={6}>
          <Card size="small">
            <Statistic title="Проведено" value={stats.provedeno ?? 0} />
          </Card>
        </Col>
      </Row>

      {/* Master / Detail layout */}
      <Row gutter={12}>
        {/* Master table */}
        <Col xs={24} lg={14}>
          <Table
            columns={masterColumns}
            dataSource={nakladnoys}
            rowKey="id"
            loading={masterLoading}
            size="small"
            pagination={{ pageSize: 15 }}
            scroll={{ x: 620 }}
            rowClassName={rec =>
              selectedNakladnoy?.id === rec.id ? 'ant-table-row-selected' : ''
            }
            onRow={rec => ({
              onClick: () => {
                setSelectedNakladnoy(rec);
                setSelectedDetailRow(null);
              },
              style: { cursor: 'pointer' },
            })}
          />
        </Col>

        {/* Detail table */}
        <Col xs={24} lg={10}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 }}>
            <Typography.Text strong>
              {selectedNakladnoy
                ? `Путевки накладной № ${selectedNakladnoy.nomer ?? selectedNakladnoy.id}`
                : 'Выберите накладную'}
            </Typography.Text>
            <Space>
              {selectedNakladnoy && (
                <>
                  <Button
                    size="small"
                    icon={<PlusOutlined />}
                    onClick={() => setVoucherPickerOpen(true)}
                  >
                    Добавить
                  </Button>
                  <Button
                    size="small"
                    danger
                    icon={<DeleteOutlined />}
                    disabled={!selectedDetailRow}
                    loading={removeVoucherMutation.isPending}
                    onClick={() => {
                      if (!selectedDetailRow) return;
                      modal.confirm({
                        title: 'Удалить путевку?',
                        content: `Путевка № ${selectedDetailRow.numPut} будет удалена из накладной.`,
                        onOk: () => removeVoucherMutation.mutate(selectedDetailRow.id),
                      });
                    }}
                  >
                    Удалить
                  </Button>
                  <Button
                    size="small"
                    icon={<PrinterOutlined />}
                    onClick={handlePrintSchetFaktura}
                  >
                    Печать
                  </Button>
                </>
              )}
            </Space>
          </div>
          <Table
            columns={detailColumns}
            dataSource={selectedNakladnoy ? details : []}
            rowKey="id"
            loading={detailsLoading}
            size="small"
            pagination={{ pageSize: 15 }}
            scroll={{ x: 400 }}
            locale={{ emptyText: selectedNakladnoy ? 'Нет путевок' : 'Выберите накладную слева' }}
            rowClassName={rec =>
              selectedDetailRow?.id === rec.id ? 'ant-table-row-selected' : ''
            }
            onRow={rec => ({
              onClick: () => setSelectedDetailRow(prev => prev?.id === rec.id ? null : rec),
              style: { cursor: 'pointer' },
            })}
          />
        </Col>
      </Row>

      {/* Add / Edit Modal */}
      <Modal
        title={editing ? 'Редактировать накладную' : 'Добавить накладную'}
        open={editModalOpen}
        onOk={() => form.submit()}
        onCancel={() => { setEditModalOpen(false); setSelectedFizLico(null); form.resetFields(); }}
        confirmLoading={saveMutation.isPending}
        width={680}
      >
        <Form form={form} layout="vertical" onFinish={handleFormFinish}>
          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="nomer" label="Номер накладной">
                <Input placeholder="Авто, если не указан" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="data" label="Дата">
                <DatePicker style={{ width: '100%' }} />
              </Form.Item>
            </Col>
          </Row>

          {/* FizLico section */}
          <Typography.Text strong style={{ display: 'block', marginBottom: 8 }}>
            Физическое лицо
          </Typography.Text>
          <div style={{ marginBottom: 12 }}>
            <Button
              icon={<SearchOutlined />}
              onClick={() => setFizPickerOpen(true)}
            >
              {selectedFizLico ? 'Изменить физ. лицо' : 'Выбрать физ. лицо'}
            </Button>
            {selectedFizLico && (
              <Typography.Text type="secondary" style={{ marginLeft: 8 }}>
                {selectedFizLico.fio} {selectedFizLico.inn ? `(ИНН: ${selectedFizLico.inn})` : ''}
              </Typography.Text>
            )}
          </div>

          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="fio" label="ФИО">
                <Input readOnly placeholder="Заполняется из справочника" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="inn" label="ИНН">
                <Input placeholder="ИНН физ. лица" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="address" label="Адрес">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="phone" label="Телефон">
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={6}>
              <Form.Item name="passportSeria" label="Паспорт Серия">
                <Input maxLength={2} placeholder="AA" />
              </Form.Item>
            </Col>
            <Col span={6}>
              <Form.Item name="passportNomer" label="Паспорт Номер">
                <Input maxLength={7} placeholder="1234567" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="passportKemVydan" label="Кем выдан">
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="passportDataVydachi" label="Дата выдачи">
                <DatePicker style={{ width: '100%' }} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="summa" label="Сумма">
                <InputNumber style={{ width: '100%' }} min={0} step={1000} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={12}>
            <Col span={12}>
              <Form.Item name="kolvo" label="Кол-во путевок">
                <InputNumber style={{ width: '100%' }} min={0} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="status" label="Статус">
                <Select allowClear>
                  <Option value="draft">Черновик</Option>
                  <Option value="active">Активный</Option>
                  <Option value="closed">Закрыт</Option>
                </Select>
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* FizLico Picker Modal */}
      <FizLicoPickerModal
        open={fizPickerOpen}
        onSelect={handleFizLicoSelect}
        onCancel={() => setFizPickerOpen(false)}
      />

      {/* Voucher Picker Modal */}
      <VoucherPickerModal
        open={voucherPickerOpen}
        nakladnoyId={selectedNakladnoy?.id ?? null}
        onClose={() => setVoucherPickerOpen(false)}
      />
    </div>
  );
}
