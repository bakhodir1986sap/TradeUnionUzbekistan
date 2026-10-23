import { useRef, useState, useEffect, CSSProperties } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, DatePicker,
  Row, Col, Typography, App, Statistic, Card, Checkbox,
} from 'antd';
import { OrganizationPickerModal } from '../../components/OrganizationPickerModal';
import { PlusOutlined, EditOutlined, SearchOutlined, DeleteOutlined, PrinterOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Option } = Select;

// ─── Types ──────────────────────────────────────────────────────────────────

interface OtraslItem {
  id: number;
  name: string;
}

interface Stats {
  totalNakladnoys: number;
  totalVouchers: number;
  totalAmount: number;
}

interface NakladnoyRow {
  id: number;
  dataPost?: string;
  numDoc?: string;
  orgName?: string;
  numKol?: number;
  symma?: number;
  nameDov?: string;
}

interface VoucherRow {
  id: number;
  sanatoriumName?: string;
  serialBegin?: string;
  restBeginDate?: string;
  price?: number;
  statusPutyovki?: string;
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

interface Organization {
  id: number;
  name: string;
  inn: string;
  branchName?: string;
}

interface NakladnoyFormValues {
  dataPost: dayjs.Dayjs | null;
  numDoc: string;
  nameDov?: string;
}

interface SchetFakturaParty {
  name?: string;
  inn?: string;
  rs?: string;
  mfo?: string;
  bank?: string;
  okonx?: string;
  adress?: string;
  telefon?: string;
  phone?: string;
  nameDir?: string;
  nameZaveduyushiy?: string;
  nameVedSpes?: string;
}

interface SchetFakturaLine {
  id: number;
  sanatoriumName?: string;
  serialBegin?: string;
  restBeginDate?: string;
  price?: number;
}

interface SchetFakturaData {
  id: number;
  numDoc?: string;
  dataPost?: string;
  numKol?: number;
  symma?: number;
  nameDov?: string;
  buyer?: SchetFakturaParty;
  supplier?: SchetFakturaParty;
  lines: SchetFakturaLine[];
}

// ─── Main page ───────────────────────────────────────────────────────────────

export function RasxodPutyovokPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();

  // Filters
  const currentYear = new Date().getFullYear();
  const [filterYear, setFilterYear] = useState<number>(currentYear);
  const [filterQuarter, setFilterQuarter] = useState<number | undefined>(undefined);
  const [filterOtrasl, setFilterOtrasl] = useState<number | undefined>(undefined);

  // Selection
  const [selectedNakladnoy, setSelectedNakladnoy] = useState<NakladnoyRow | null>(null);

  // Modals
  const [addModalOpen, setAddModalOpen] = useState(false);
  const [editModalOpen, setEditModalOpen] = useState(false);
  const [addVouchersOpen, setAddVouchersOpen] = useState(false);
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);
  const [orgPickerTarget, setOrgPickerTarget] = useState<'add' | 'edit'>('add');

  // Forms
  const [addForm] = Form.useForm<NakladnoyFormValues>();
  const [editForm] = Form.useForm<NakladnoyFormValues>();

  // Selected org for add/edit
  const [addOrg, setAddOrg] = useState<Organization | null>(null);
  const [editOrg, setEditOrg] = useState<Organization | null>(null);

  // Voucher search
  const [voucherSearch, setVoucherSearch] = useState('');
  const [selectedVoucherIds, setSelectedVoucherIds] = useState<number[]>([]);

  // Selected row in detail table
  const [selectedDetailRow, setSelectedDetailRow] = useState<VoucherRow | null>(null);

  // Schet-Faktura print modal
  const [schetFakturaOpen, setSchetFakturaOpen] = useState(false);
  const printRef = useRef<HTMLDivElement>(null);

  const { data: schetFakturaData, isFetching: schetFakturaLoading } = useQuery<SchetFakturaData>({
    queryKey: ['schet-faktura', selectedNakladnoy?.id],
    queryFn: () =>
      api.get(`/api/rasxod-putevok/${selectedNakladnoy!.id}/schet-faktura`).then(r => r.data),
    enabled: schetFakturaOpen && !!selectedNakladnoy,
    staleTime: 0,
  });

  const handlePrint = () => {
    const el = printRef.current;
    if (!el) return;
    const win = window.open('', '_blank', 'width=1400,height=900');
    if (!win) return;
    win.document.write(`<html><head><title>Счёт-Фактура</title>
<style>
  body { margin: 10px; font-size: 11px; font-family: Arial, sans-serif; }
  table { border-collapse: collapse; width: 100%; }
  td { padding: 2px 4px; }
  .b { border: 1px solid #000; }
  .bt { border-top: 1px solid #000; }
  .bb { border-bottom: 1px solid #000; }
  .bl { border-left: 1px solid #000; }
  .br { border-right: 1px solid #000; }
  .tc { text-align: center; }
  .tr { text-align: right; }
  .tl { text-align: left; }
  strong { font-weight: bold; }
  .page-break { page-break-after: always; }
</style>
</head><body onload="window.print();window.close();">${el.innerHTML}</body></html>`);
    win.document.close();
  };

  // ── Reference: Отрасли ──
  const { data: otraslList = [] } = useQuery<OtraslItem[]>({
    queryKey: ['otras'],
    queryFn: () => api.get('/api/otrasl').then(r => r.data),
  });

  // ── Main data ──
  const { data: mainData, isLoading: mainLoading, isError: mainError, refetch: refetchMain } = useQuery<{ nakladnoys: NakladnoyRow[]; stats: Stats }>({
    queryKey: ['rasxod-putevok', filterYear, filterQuarter, filterOtrasl],
    queryFn: () =>
      api.get('/api/rasxod-putevok', {
        params: {
          year: filterYear,
          quarter: filterQuarter,
          otraslId: filterOtrasl,
        },
      }).then(r => r.data),
  });

  const nakladnoys: NakladnoyRow[] = mainData?.nakladnoys ?? [];
  const stats: Stats = mainData?.stats ?? { totalNakladnoys: 0, totalVouchers: 0, totalAmount: 0 };

  // ── Detail vouchers for selected nakladnoy ──
  const { data: detailRows = [], isLoading: detailLoading } = useQuery<VoucherRow[]>({
    queryKey: ['rasxod-putevok-detail', selectedNakladnoy?.id],
    queryFn: () =>
      api.get(`/api/rasxod-putevok/${selectedNakladnoy!.id}/details`).then(r => r.data),
    enabled: !!selectedNakladnoy,
    staleTime: 0,
  });

  // ── Available vouchers for add-vouchers modal ──
  const { data: availableVouchers = [], isLoading: availLoading } = useQuery<AvailableVoucher[]>({
    queryKey: ['available-vouchers', voucherSearch],
    queryFn: () =>
      api.get('/api/rasxod-putevok/available-vouchers', { params: { search: voucherSearch } }).then(r => r.data),
    enabled: addVouchersOpen,
  });

  // ── Mutations ──
  const createMutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      api.post('/api/rasxod-putevok', values).then(r => r.data),
    onSuccess: () => {
      message.success('Накладная создана');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      setAddModalOpen(false);
      addForm.resetFields();
      setAddOrg(null);
    },
    onError: () => message.error('Ошибка при создании'),
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, values }: { id: number; values: Record<string, unknown> }) =>
      api.put(`/api/rasxod-putevok/${id}`, values).then(r => r.data),
    onSuccess: () => {
      message.success('Накладная обновлена');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      setEditModalOpen(false);
    },
    onError: () => message.error('Ошибка при обновлении'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/rasxod-putevok/${id}`),
    onSuccess: () => {
      message.success('Накладная удалена');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      if (selectedNakladnoy) setSelectedNakladnoy(null);
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const provodkaMutation = useMutation({
    mutationFn: (id: number) =>
      api.post(`/api/rasxod-putevok/${id}/provodka`).then(r => r.data),
    onSuccess: () => {
      message.success('Проводка выполнена');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
    },
    onError: () => message.error('Ошибка проводки'),
  });

  const addVouchersMutation = useMutation({
    mutationFn: ({ id, voucherIds }: { id: number; voucherIds: number[] }) =>
      api.post(`/api/rasxod-putevok/${id}/add-vouchers`, { voucherIds }).then(r => r.data),
    onSuccess: () => {
      message.success('Путевки добавлены');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-detail'] });
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      queryClient.invalidateQueries({ queryKey: ['available-vouchers'] });
      setAddVouchersOpen(false);
      setSelectedVoucherIds([]);
      setVoucherSearch('');
    },
    onError: () => message.error('Ошибка при добавлении путевок'),
  });

  const removeVoucherMutation = useMutation({
    mutationFn: ({ nakladnoyId, voucherId }: { nakladnoyId: number; voucherId: number }) =>
      api.delete(`/api/rasxod-putevok/${nakladnoyId}/vouchers/${voucherId}`),
    onSuccess: () => {
      message.success('Путевка удалена');
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-detail'] });
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      queryClient.invalidateQueries({ queryKey: ['available-vouchers'] });
      setSelectedDetailRow(null);
    },
    onError: () => message.error('Ошибка при удалении путевки'),
  });

  // ── Handlers ──
  const openAdd = () => {
    addForm.resetFields();
    setAddOrg(null);
    setAddModalOpen(true);
  };

  const openEdit = () => {
    if (!selectedNakladnoy) return;
    editForm.setFieldsValue({
      dataPost: selectedNakladnoy.dataPost ? dayjs(selectedNakladnoy.dataPost) : null,
      numDoc: selectedNakladnoy.numDoc,
      nameDov: selectedNakladnoy.nameDov,
    });
    setEditOrg({ id: 0, name: selectedNakladnoy.orgName ?? '', inn: '' });
    setEditModalOpen(true);
  };

  const handleAddSubmit = (values: NakladnoyFormValues) => {
    createMutation.mutate({
      year: filterYear,
      quarter: filterQuarter ?? 1,
      numDoc: values.numDoc,
      dataPost: values.dataPost?.toISOString(),
      idOrg: addOrg?.id,
      nameDov: values.nameDov,
    });
  };

  const handleEditSubmit = (values: NakladnoyFormValues) => {
    if (!selectedNakladnoy) return;
    updateMutation.mutate({
      id: selectedNakladnoy.id,
      values: {
        numDoc: values.numDoc,
        dataPost: values.dataPost?.toISOString(),
        idOrg: editOrg?.id,
        nameDov: values.nameDov,
      },
    });
  };

  const handleConfirmDelete = () => {
    if (!selectedNakladnoy) return;
    modal.confirm({
      title: 'Удалить накладную?',
      content: `№ ${selectedNakladnoy.numDoc}`,
      onOk: () => deleteMutation.mutate(selectedNakladnoy.id),
    });
  };

  const handleProvodka = () => {
    if (!selectedNakladnoy) return;
    modal.confirm({
      title: 'Выполнить проводку?',
      onOk: () => provodkaMutation.mutate(selectedNakladnoy.id),
    });
  };

  const handleOrgSelect = (org: Organization) => {
    if (orgPickerTarget === 'add') {
      setAddOrg(org);
    } else {
      setEditOrg(org);
    }
  };

  const handleAddVouchersSave = () => {
    if (!selectedNakladnoy || selectedVoucherIds.length === 0) return;
    addVouchersMutation.mutate({ id: selectedNakladnoy.id, voucherIds: selectedVoucherIds });
  };

  const handleRemoveVoucher = () => {
    if (!selectedNakladnoy || !selectedDetailRow) return;
    modal.confirm({
      title: 'Удалить путевку?',
      content: `№ ${selectedDetailRow.serialBegin ?? selectedDetailRow.id}`,
      onOk: () => removeVoucherMutation.mutate({
        nakladnoyId: selectedNakladnoy.id,
        voucherId: selectedDetailRow.id,
      }),
    });
  };

  const filteredAvailableVouchers = availableVouchers.filter(v =>
    !voucherSearch || v.numPut?.toLowerCase().includes(voucherSearch.toLowerCase()),
  );

  // ── Year options ──
  const yearOptions = Array.from({ length: 6 }, (_, i) => currentYear - 1 + i);

  // ── Columns ──
  const nakladnoyColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    {
      title: 'Дата', dataIndex: 'dataPost', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    { title: '№ Документа', dataIndex: 'numDoc', width: 130 },
    { title: 'Организация', dataIndex: 'orgName' },
    { title: 'Кол-во', dataIndex: 'numKol', width: 80 },
    { title: 'Сумма', dataIndex: 'symma', width: 110 },
  ];

  const voucherColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Санаторий', dataIndex: 'sanatoriumName' },
    { title: 'Номер путевки', dataIndex: 'serialBegin', width: 120 },
    {
      title: 'Дата заезда', dataIndex: 'restBeginDate', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    { title: 'Цена', dataIndex: 'price', width: 110 },
  ];

  const availableVoucherColumns = [
    {
      title: '',
      width: 40,
      render: (_: unknown, rec: AvailableVoucher) => (
        <Checkbox
          checked={selectedVoucherIds.includes(rec.id)}
          onChange={e => {
            setSelectedVoucherIds(prev =>
              e.target.checked ? [...prev, rec.id] : prev.filter(id => id !== rec.id),
            );
          }}
        />
      ),
    },
    { title: 'Санаторий', dataIndex: 'sanatoriumName' },
    { title: 'Номер путевки', dataIndex: 'numPut' },
    {
      title: 'Дата заезда', dataIndex: 'dataSan', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Дата выезда', dataIndex: 'dataSanEnd', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    { title: 'Цена', dataIndex: 'symmaPut', width: 110 },
  ];

  // ── Render ──
  return (
    <div style={{ padding: 16 }}>
      {/* Title */}
      <Typography.Title level={4} style={{ marginBottom: 16 }}>
        Расход путевок
      </Typography.Title>

      {/* Filters */}
      <Row gutter={12} style={{ marginBottom: 16 }}>
        <Col>
          <Select
            value={filterYear}
            onChange={setFilterYear}
            style={{ width: 100 }}
            placeholder="Год"
          >
            {yearOptions.map(y => <Option key={y} value={y}>{y}</Option>)}
          </Select>
        </Col>
        <Col>
          <Select
            value={filterQuarter}
            onChange={setFilterQuarter}
            style={{ width: 120 }}
            placeholder="Квартал"
            allowClear
          >
            {[1, 2, 3, 4].map(q => <Option key={q} value={q}>{q} квартал</Option>)}
          </Select>
        </Col>
        <Col>
          <Select
            value={filterOtrasl}
            onChange={setFilterOtrasl}
            style={{ width: 220 }}
            placeholder="Отрасль"
            allowClear
            showSearch
            optionFilterProp="children"
          >
            {otraslList.map(o => <Option key={o.id} value={o.id}>{o.name}</Option>)}
          </Select>
        </Col>
      </Row>

      {/* Stats */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={12} sm={8}>
          <Card size="small">
            <Statistic title="Накладных" value={stats.totalNakladnoys} />
          </Card>
        </Col>
        <Col xs={12} sm={8}>
          <Card size="small">
            <Statistic title="Путевок" value={stats.totalVouchers} />
          </Card>
        </Col>
        <Col xs={12} sm={8}>
          <Card size="small">
            <Statistic title="Сумма" value={stats.totalAmount} precision={2} />
          </Card>
        </Col>
      </Row>

      {/* Action buttons */}
      <Space style={{ marginBottom: 12 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
          Добавить
        </Button>
        <Button
          icon={<EditOutlined />}
          onClick={openEdit}
          disabled={!selectedNakladnoy}
        >
          Изменить
        </Button>
        <Button
          icon={<PrinterOutlined />}
          disabled={!selectedNakladnoy}
          onClick={() => setSchetFakturaOpen(true)}
        >
          Счёт-Фактура
        </Button>
      </Space>

      {/* Master-detail tables */}
      <Row gutter={12}>
        {/* LEFT: Накладные */}
        <Col xs={24} lg={12}>
          <Typography.Text strong style={{ display: 'block', marginBottom: 4 }}>
            Накладные
          </Typography.Text>
          <QueryErrorAlert isError={mainError} onRetry={refetchMain} />
          <Table<NakladnoyRow>
            columns={nakladnoyColumns}
            dataSource={nakladnoys}
            rowKey="id"
            loading={mainLoading}
            size="small"
            pagination={{ pageSize: 15 }}
            rowClassName={rec =>
              selectedNakladnoy?.id === rec.id ? 'ant-table-row-selected' : ''
            }
            onRow={rec => ({
              onClick: () => {
                setSelectedNakladnoy(prev => (prev?.id === rec.id ? null : rec));
                setSelectedDetailRow(null);
              },
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 500 }}
          />
        </Col>

        {/* RIGHT: Путевки */}
        <Col xs={24} lg={12}>
          <Typography.Text strong style={{ display: 'block', marginBottom: 4 }}>
            Путевки{selectedNakladnoy ? ` — ${selectedNakladnoy.numDoc}` : ''}
          </Typography.Text>
          <Space style={{ marginBottom: 8 }}>
            <Button
              type="primary"
              icon={<PlusOutlined />}
              disabled={!selectedNakladnoy}
              onClick={() => {
                setVoucherSearch('');
                setSelectedVoucherIds([]);
                setAddVouchersOpen(true);
              }}
            >
              Добавить
            </Button>
            <Button
              danger
              icon={<DeleteOutlined />}
              disabled={!selectedDetailRow}
              loading={removeVoucherMutation.isPending}
              onClick={handleRemoveVoucher}
            >
              Удалить
            </Button>
          </Space>
          <Table<VoucherRow>
            columns={voucherColumns}
            dataSource={detailRows}
            rowKey="id"
            loading={detailLoading}
            size="small"
            pagination={{ pageSize: 15 }}
            locale={{ emptyText: selectedNakladnoy ? 'Нет путевок' : 'Выберите накладную' }}
            rowClassName={rec =>
              selectedDetailRow?.id === rec.id ? 'ant-table-row-selected' : ''
            }
            onRow={rec => ({
              onClick: () => setSelectedDetailRow(prev => prev?.id === rec.id ? null : rec),
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 400 }}
          />
        </Col>
      </Row>

      {/* ── Add Modal ───────────────────────────────────────────────────────── */}
      <Modal
        title="Добавить накладную"
        open={addModalOpen}
        onOk={() => addForm.submit()}
        onCancel={() => setAddModalOpen(false)}
        confirmLoading={createMutation.isPending}
        width={520}
        destroyOnClose
      >
        <Form
          form={addForm}
          layout="vertical"
          onFinish={handleAddSubmit}
        >
          <Form.Item name="dataPost" label="Дата" rules={[{ required: true, message: 'Укажите дату' }]}>
            <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>

          <Form.Item name="numDoc" label="Номер документа" rules={[{ required: true, message: 'Укажите номер' }]}>
            <Input />
          </Form.Item>

          <Form.Item label="Покупатель">
            <Space>
              <Input
                value={addOrg?.name ?? ''}
                readOnly
                placeholder="Не выбрано"
                style={{ width: 300 }}
              />
              <Button
                onClick={() => {
                  setOrgPickerTarget('add');
                  setOrgPickerOpen(true);
                }}
              >
                Выбрать
              </Button>
            </Space>
          </Form.Item>

          <Form.Item name="nameDov" label="Доверенность">
            <Input />
          </Form.Item>
        </Form>
      </Modal>

      {/* ── Edit Modal ───────────────────────────────────────────────────────── */}
      <Modal
        title="Изменить накладную"
        open={editModalOpen}
        footer={null}
        onCancel={() => setEditModalOpen(false)}
        width={520}
        destroyOnClose
      >
        <Form
          form={editForm}
          layout="vertical"
          onFinish={handleEditSubmit}
        >
          <Form.Item name="dataPost" label="Дата" rules={[{ required: true, message: 'Укажите дату' }]}>
            <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>

          <Form.Item name="numDoc" label="Номер документа" rules={[{ required: true, message: 'Укажите номер' }]}>
            <Input />
          </Form.Item>

          <Form.Item label="Покупатель">
            <Space>
              <Input
                value={editOrg?.name ?? ''}
                readOnly
                placeholder="Не выбрано"
                style={{ width: 300 }}
              />
              <Button
                onClick={() => {
                  setOrgPickerTarget('edit');
                  setOrgPickerOpen(true);
                }}
              >
                Выбрать
              </Button>
            </Space>
          </Form.Item>

          <Form.Item name="nameDov" label="Доверенность">
            <Input />
          </Form.Item>

          {/* Action row */}
          <Row gutter={8} justify="end" style={{ marginTop: 8 }}>
            <Col>
              <Button
                onClick={handleProvodka}
                loading={provodkaMutation.isPending}
              >
                Проводка
              </Button>
            </Col>
            <Col>
              <Button
                type="primary"
                htmlType="submit"
                loading={updateMutation.isPending}
              >
                Изменить
              </Button>
            </Col>
            <Col>
              <Button
                danger
                onClick={() => {
                  setEditModalOpen(false);
                  handleConfirmDelete();
                }}
                loading={deleteMutation.isPending}
              >
                Удалить
              </Button>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* ── Add Vouchers Modal ───────────────────────────────────────────────── */}
      <Modal
        title="Добавить путевки"
        open={addVouchersOpen}
        onOk={handleAddVouchersSave}
        okText="Сохранить"
        onCancel={() => {
          setAddVouchersOpen(false);
          setSelectedVoucherIds([]);
          setVoucherSearch('');
        }}
        confirmLoading={addVouchersMutation.isPending}
        okButtonProps={{ disabled: selectedVoucherIds.length === 0 }}
        width={640}
        destroyOnClose
      >
        <Input
          prefix={<SearchOutlined />}
          placeholder="Поиск по номеру путевки"
          value={voucherSearch}
          onChange={e => setVoucherSearch(e.target.value)}
          allowClear
          style={{ marginBottom: 12 }}
        />
        <Table<AvailableVoucher>
          columns={availableVoucherColumns}
          dataSource={filteredAvailableVouchers}
          rowKey="id"
          loading={availLoading}
          size="small"
          pagination={{ pageSize: 10 }}
          scroll={{ x: 480 }}
        />
        {selectedVoucherIds.length > 0 && (
          <Typography.Text type="secondary" style={{ marginTop: 8, display: 'block' }}>
            Выбрано: {selectedVoucherIds.length} путевок
          </Typography.Text>
        )}
      </Modal>

      {/* ── Organization Picker ──────────────────────────────────────────────── */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={handleOrgSelect}
      />

      {/* ── Schet-Faktura Modal ──────────────────────────────────────────────── */}
      <Modal
        title={`Счёт-Фактура № ${schetFakturaData?.numDoc ?? '...'}`}
        open={schetFakturaOpen}
        onCancel={() => setSchetFakturaOpen(false)}
        footer={
          <Button
            type="primary"
            icon={<PrinterOutlined />}
            onClick={handlePrint}
            loading={schetFakturaLoading}
          >
            Печать
          </Button>
        }
        width="90vw"
        style={{ top: 20 }}
        destroyOnClose
      >
        {schetFakturaLoading && <Typography.Text>Загрузка...</Typography.Text>}
        {schetFakturaData && (
          <div ref={printRef}>
            <SchetFakturaDocument data={schetFakturaData} />
          </div>
        )}
      </Modal>
    </div>
  );
}

// ─── SchetFaktura document component (reusable for print) ─────────────────────

function SchetFakturaHalf({ data }: { data: SchetFakturaData }) {
  const d = data.dataPost ? dayjs(data.dataPost) : null;
  const naklDay   = d?.format('DD') ?? '';
  const naklMonth = d?.format('MMMM') ?? '';
  const naklYear  = d?.format('YYYY') ?? '';
  const s = data.supplier;
  const b = data.buyer;

  const tdStyle: CSSProperties = { padding: '1px 3px', fontSize: 11 };

  return (
    <table style={{ borderCollapse: 'collapse', width: '100%', fontSize: 11 }}>
      <tbody>
        <tr>
          <td colSpan={12} style={{ textAlign: 'center', ...tdStyle }}>
            <strong>НАКЛАДНАЯ-СЧЕТ-ФАКТУРА № {data.numDoc}</strong>
          </td>
          <td colSpan={6} style={{ textAlign: 'center', ...tdStyle }}>
            <strong>от &ldquo;{naklDay}&rdquo; {naklMonth} {naklYear}</strong>
          </td>
        </tr>
        <tr>
          <td colSpan={18} style={{ textAlign: 'center', ...tdStyle }}>
            <strong>к товарно отгрузочным документам №___ от &ldquo;__&rdquo;_____ ___года</strong>
          </td>
        </tr>
        <tr><td colSpan={18} style={tdStyle}>&nbsp;</td></tr>
        {/* Поставщик / Покупатель header */}
        {[
          ['Поставщик:', s?.name ?? '', 'Покупатель:', b?.name ?? ''],
          ['Адрес:', s?.adress ?? '', 'Адрес:', b?.adress ?? ''],
          ['Телефон:', s?.phone ?? s?.telefon ?? '', 'Телефон:', b?.telefon ?? ''],
          ['Р/с:', s?.rs ?? '', 'Р/с:', b?.rs ?? ''],
          ['Банк:', s?.bank ?? '', 'Банк:', b?.bank ?? ''],
          ['ИНН:', s?.inn ?? '', 'ИНН:', b?.inn ?? ''],
        ].map(([lbl1, val1, lbl2, val2], i) => (
          <tr key={i}>
            <td colSpan={3} style={{ borderLeft: '1px solid #000', borderTop: i === 0 ? '1px solid #000' : undefined, ...tdStyle }}><strong>{lbl1}</strong></td>
            <td colSpan={5} style={{ borderRight: '1px solid #000', borderTop: i === 0 ? '1px solid #000' : undefined, ...tdStyle }}><strong>{val1}</strong></td>
            <td colSpan={3} style={{ borderLeft: '1px solid #000', borderTop: i === 0 ? '1px solid #000' : undefined, ...tdStyle }}><strong>{lbl2}</strong></td>
            <td colSpan={7} style={{ borderRight: '1px solid #000', borderTop: i === 0 ? '1px solid #000' : undefined, ...tdStyle }}><strong>{val2}</strong></td>
          </tr>
        ))}
        {/* МФО / ОКОНХ row */}
        <tr>
          <td colSpan={3} style={{ borderLeft: '1px solid #000', ...tdStyle }}><strong>МФО:</strong></td>
          <td colSpan={2} style={tdStyle}><strong>{s?.mfo ?? ''}</strong></td>
          <td colSpan={2} style={{ textAlign: 'center', ...tdStyle }}><strong>ОКОНХ:</strong></td>
          <td colSpan={1} style={{ borderRight: '1px solid #000', ...tdStyle }}><strong>{s?.okonx ?? ''}</strong></td>
          <td colSpan={3} style={{ borderLeft: '1px solid #000', ...tdStyle }}><strong>МФО:</strong></td>
          <td colSpan={2} style={tdStyle}><strong>{b?.mfo ?? ''}</strong></td>
          <td colSpan={2} style={{ textAlign: 'right', ...tdStyle }}><strong>ОКОНХ:</strong></td>
          <td colSpan={3} style={{ borderRight: '1px solid #000', ...tdStyle }}><strong>{b?.okonx ?? ''}</strong></td>
        </tr>
        <tr><td colSpan={18} style={{ borderTop: '1px solid #000', borderBottom: '1px solid #000', ...tdStyle }}></td></tr>
        {/* Table header */}
        <tr>
          <td rowSpan={2} colSpan={1} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>№</strong></td>
          <td rowSpan={2} colSpan={4} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Наименование товаров (работ, услуг)</strong></td>
          <td rowSpan={2} colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>№ путевки</strong></td>
          <td rowSpan={2} colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Дата</strong></td>
          <td rowSpan={2} colSpan={1} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Ед.изм</strong></td>
          <td rowSpan={2} colSpan={1} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Кол-во</strong></td>
          <td rowSpan={2} colSpan={1} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Цена</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Акциз</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>НДС</strong></td>
          <td rowSpan={2} colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>Стоимость с учет. НДС</strong></td>
        </tr>
        <tr>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>%</td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>сум-ма</td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>%</td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>сум-ма</td>
        </tr>
        {/* Column numbers row — colSpans must match header: 1,4,2,2,1,1,1,1,1,1,1,2 */}
        <tr>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>1</strong></td>
          <td colSpan={4} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>2</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>3</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>4</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>5</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>6</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>7</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>9</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>10</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>11</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>12</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>13</strong></td>
        </tr>
        {/* Data rows */}
        {data.lines.map((line, idx) => (
          <tr key={line.id}>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{idx + 1}</strong></td>
            <td colSpan={4} style={{ border: '1px solid #000', textAlign: 'left', ...tdStyle }}><strong>{line.sanatoriumName}</strong></td>
            <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'right', ...tdStyle }}><strong>{line.serialBegin}</strong></td>
            <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{line.restBeginDate ? dayjs(line.restBeginDate).format('DD.MM.YYYY') : ''}</strong></td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>шт</td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>1</td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{line.price?.toLocaleString('ru-RU')}</strong></td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>без акциза</td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}></td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}>без НДС</td>
            <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}></td>
            <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{line.price?.toLocaleString('ru-RU')}</strong></td>
          </tr>
        ))}
        {/* Итого row */}
        <tr>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}></td>
          <td colSpan={4} style={{ border: '1px solid #000', textAlign: 'right', ...tdStyle }}><strong>ИТОГО:</strong></td>
          <td colSpan={2} style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td colSpan={2} style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{data.numKol}</strong></td>
          <td style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{data.symma?.toLocaleString('ru-RU')}</strong></td>
          <td style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td style={{ border: '1px solid #000', ...tdStyle }}></td>
          <td colSpan={2} style={{ border: '1px solid #000', textAlign: 'center', ...tdStyle }}><strong>{data.symma?.toLocaleString('ru-RU')}</strong></td>
        </tr>
        <tr>
          <td colSpan={18} style={{ borderTop: '1px solid #000', ...tdStyle }}><strong>Всего к оплате:</strong></td>
        </tr>
        <tr><td colSpan={18} style={tdStyle}>&nbsp;</td></tr>
        {/* Signatures */}
        <tr>
          <td colSpan={5} style={tdStyle}><strong>Руководителя</strong></td>
          <td colSpan={6} style={tdStyle}><strong>{s?.nameDir ?? ''}</strong></td>
          <td colSpan={7} style={tdStyle}><strong>Получил ____________________</strong></td>
        </tr>
        <tr>
          <td colSpan={5} style={tdStyle}><strong>Главный бухгалтер</strong></td>
          <td colSpan={6} style={tdStyle}><strong>{s?.nameZaveduyushiy ?? ''}</strong></td>
          <td colSpan={7} style={tdStyle}><strong>По доверенности</strong></td>
        </tr>
        <tr>
          <td colSpan={3} style={tdStyle}><strong>М.П.</strong></td>
          <td colSpan={8} style={tdStyle}></td>
          <td colSpan={7} style={tdStyle}></td>
        </tr>
        <tr>
          <td colSpan={5} style={tdStyle}><strong>Товар отпустил</strong></td>
          <td colSpan={6} style={tdStyle}><strong>{s?.nameVedSpes ?? ''}</strong></td>
          <td colSpan={7} style={{ borderBottom: '1px solid #000', ...tdStyle }}><strong>{data.nameDov ?? ''}</strong></td>
        </tr>
        <tr>
          <td colSpan={11} style={tdStyle}></td>
          <td colSpan={7} style={tdStyle}><strong>(Ф.И.О. получателя)</strong></td>
        </tr>
      </tbody>
    </table>
  );
}

function SchetFakturaDocument({ data }: { data: SchetFakturaData }) {
  const wrapRef = useRef<HTMLDivElement>(null);
  const innerRef = useRef<HTMLDivElement>(null);
  const [scale, setScale] = useState(1);
  const [innerH, setInnerH] = useState(0);
  const INNER_WIDTH = 1400;

  useEffect(() => {
    const wrap = wrapRef.current;
    const inner = innerRef.current;
    if (!wrap || !inner) return;
    const update = () => {
      const available = wrap.offsetWidth;
      const s = available > 0 ? Math.min(1, available / INNER_WIDTH) : 1;
      setScale(s);
      setInnerH(inner.scrollHeight * s);
    };
    update();
    const ro = new ResizeObserver(update);
    ro.observe(wrap);
    ro.observe(inner);
    return () => ro.disconnect();
  }, [data]);

  return (
    <div ref={wrapRef} style={{ width: '100%', overflow: 'hidden', height: innerH || 'auto' }}>
      <div
        ref={innerRef}
        style={{
          width: INNER_WIDTH,
          transformOrigin: 'top left',
          transform: `scale(${scale})`,
          display: 'flex',
          gap: 8,
        }}
      >
        <div style={{ flex: 1, minWidth: 0 }}>
          <SchetFakturaHalf data={data} />
        </div>
        <div style={{ flex: 1, minWidth: 0 }}>
          <SchetFakturaHalf data={data} />
        </div>
      </div>
    </div>
  );
}
