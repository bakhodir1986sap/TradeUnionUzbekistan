import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, DatePicker,
  Row, Col, Typography, App, Statistic, Card, Checkbox,
} from 'antd';
import { OrganizationPickerModal } from '../../components/OrganizationPickerModal';
import { PlusOutlined, EditOutlined, SearchOutlined } from '@ant-design/icons';
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
  recId: number;
  serialBegin?: string;
  serialEnd?: string;
  restBeginDate?: string;
  restEndDate?: string;
  price?: number;
  status?: string;
  sanatoriumId?: number;
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
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok-detail', selectedNakladnoy?.id] });
      queryClient.invalidateQueries({ queryKey: ['rasxod-putevok'] });
      setAddVouchersOpen(false);
      setSelectedVoucherIds([]);
      setVoucherSearch('');
    },
    onError: () => message.error('Ошибка при добавлении путевок'),
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

  const filteredAvailableVouchers = availableVouchers.filter(v =>
    !voucherSearch || v.serialBegin?.toLowerCase().includes(voucherSearch.toLowerCase()),
  );

  // ── Year options ──
  const yearOptions = Array.from({ length: 6 }, (_, i) => currentYear - 1 + i);

  // ── Columns ──
  const nakladnoyColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Дата', dataIndex: 'dataPost', width: 110 },
    { title: '№ Документа', dataIndex: 'numDoc', width: 130 },
    { title: 'Организация', dataIndex: 'orgName' },
    { title: 'Кол-во', dataIndex: 'numKol', width: 80 },
    { title: 'Сумма', dataIndex: 'symma', width: 110 },
  ];

  const voucherColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Санаторий', dataIndex: 'sanatoriumName' },
    { title: 'Серия (нач.)', dataIndex: 'serialBegin', width: 120 },
    { title: 'Дата заезда', dataIndex: 'restBeginDate', width: 110 },
    { title: 'Цена', dataIndex: 'price', width: 110 },
  ];

  const availableVoucherColumns = [
    {
      title: '',
      width: 40,
      render: (_: unknown, rec: AvailableVoucher) => (
        <Checkbox
          checked={selectedVoucherIds.includes(rec.recId)}
          onChange={e => {
            setSelectedVoucherIds(prev =>
              e.target.checked ? [...prev, rec.recId] : prev.filter(id => id !== rec.recId),
            );
          }}
        />
      ),
    },
    { title: 'Серия (нач.)', dataIndex: 'serialBegin' },
    { title: 'Серия (кон.)', dataIndex: 'serialEnd' },
    { title: 'Дата заезда', dataIndex: 'restBeginDate', width: 110 },
    { title: 'Цена', dataIndex: 'price', width: 110 },
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
          disabled={!selectedNakladnoy}
          onClick={() => {
            setVoucherSearch('');
            setSelectedVoucherIds([]);
            setAddVouchersOpen(true);
          }}
        >
          Добавить путевки
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
              onClick: () =>
                setSelectedNakladnoy(prev => (prev?.id === rec.id ? null : rec)),
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
          <Table<VoucherRow>
            columns={voucherColumns}
            dataSource={detailRows}
            rowKey="id"
            loading={detailLoading}
            size="small"
            pagination={{ pageSize: 15 }}
            locale={{ emptyText: selectedNakladnoy ? 'Нет путевок' : 'Выберите накладную' }}
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
          rowKey="recId"
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
    </div>
  );
}
