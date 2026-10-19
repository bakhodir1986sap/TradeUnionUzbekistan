import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, InputNumber, Select, DatePicker,
  Row, Col, Typography, App, Statistic, Card, Checkbox,
} from 'antd';
import { OrganizationPickerModal } from '../../components/OrganizationPickerModal';
import {
  PlusOutlined, EditOutlined, DeleteOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Option } = Select;

// ─── Interfaces ──────────────────────────────────────────────────────────────

interface Stats {
  nakladnoyCount: number;
  putyovkaCount: number;
  summaNakladnix: number;
}

interface Nakladnoy {
  id: number;
  dataPost?: string;
  numDoc?: string;
  posName?: string;
  putyovkaCount?: number;
  summaLinked?: number;
  status?: string;
}

interface NakladnoyDtl {
  id: number;
  idPost?: number;
  idSan?: number;
  numPut?: string;
  dataSan?: string;
  symmaPut?: number;
  statusPutyovki?: string;
  sanName?: string;
}

interface RuchnoyResponse {
  nakladnoys: Nakladnoy[];
  stats: Stats;
}

interface Organization {
  id: number;
  name: string;
  inn?: string;
  industry?: string;
}

interface Sanatorium {
  id: number;
  name: string;
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function PrixodPutyovokRuchnoyPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();

  // Filters
  const currentYear = new Date().getFullYear();
  const [filterYear, setFilterYear] = useState<number>(currentYear);
  const [filterQuarter, setFilterQuarter] = useState<number>(1);

  // Selected invoice row
  const [selectedNakladnoy, setSelectedNakladnoy] = useState<Nakladnoy | null>(null);

  // Invoice modal (add/edit)
  const [invoiceModalOpen, setInvoiceModalOpen] = useState(false);
  const [editingInvoice, setEditingInvoice] = useState<Nakladnoy | null>(null);
  const [invoiceForm] = Form.useForm();

  // Org picker
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);
  const [selectedOrg, setSelectedOrg] = useState<Organization | null>(null);

  // Voucher add modal
  const [voucherModalOpen, setVoucherModalOpen] = useState(false);
  const [voucherForm] = Form.useForm();
  const [generatedNumbers, setGeneratedNumbers] = useState<string[]>([]);
  const [checkedNumbers, setCheckedNumbers] = useState<string[]>([]);
  const [voucherStep, setVoucherStep] = useState<'form' | 'confirm'>('form');

  // ── Main data query ────────────────────────────────────────────────────────
  const { data, isLoading, isError, refetch } = useQuery<RuchnoyResponse>({
    queryKey: ['prixod-putevok-ruchnoy', filterYear, filterQuarter],
    queryFn: () =>
      api.get(`/api/prixod-putevok/ruchnoy?year=${filterYear}&quarter=${filterQuarter}`)
        .then(r => r.data),
  });

  const nakladnoys: Nakladnoy[] = data?.nakladnoys ?? [];
  const stats: Stats = data?.stats ?? { nakladnoyCount: 0, putyovkaCount: 0, summaNakladnix: 0 };

  // ── Details query ──────────────────────────────────────────────────────────
  const { data: details = [], isLoading: detailsLoading } = useQuery<NakladnoyDtl[]>({
    queryKey: ['prixod-putevok-ruchnoy-details', selectedNakladnoy?.id],
    queryFn: () =>
      api.get(`/api/prixod-putevok/ruchnoy/${selectedNakladnoy!.id}/details`)
        .then(r => r.data),
    enabled: !!selectedNakladnoy,
  });

  // ── Sanatoriums for voucher select ─────────────────────────────────────────
  const { data: sanatoriums = [] } = useQuery<Sanatorium[]>({
    queryKey: ['sanatoriums'],
    queryFn: () => api.get('/api/sanatoriums').then(r => r.data),
  });

  const invalidate = () => {
    queryClient.invalidateQueries({ queryKey: ['prixod-putevok-ruchnoy'] });
    queryClient.invalidateQueries({ queryKey: ['prixod-putevok-ruchnoy-details'] });
  };

  // ── Invoice mutations ──────────────────────────────────────────────────────
  const saveInvoiceMutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      editingInvoice
        ? api.put(`/api/prixod-putevok/ruchnoy/${editingInvoice.id}`, values).then(r => r.data)
        : api.post('/api/prixod-putevok/ruchnoy', values).then(r => r.data),
    onSuccess: () => {
      message.success('Сохранено');
      invalidate();
      setInvoiceModalOpen(false);
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const deleteInvoiceMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/prixod-putevok/ruchnoy/${id}`),
    onSuccess: () => {
      message.success('Удалено');
      if (selectedNakladnoy?.id === deleteInvoiceMutation.variables) {
        setSelectedNakladnoy(null);
      }
      invalidate();
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const provodkaMutation = useMutation({
    mutationFn: (id: number) =>
      api.post(`/api/prixod-putevok/ruchnoy/${id}/provodka`).then(r => r.data),
    onSuccess: () => {
      message.success('Проводка выполнена');
      invalidate();
    },
    onError: () => message.error('Ошибка проводки'),
  });

  // ── Voucher mutations ──────────────────────────────────────────────────────
  const addVouchersMutation = useMutation({
    mutationFn: (payload: Record<string, unknown>) =>
      api.post(`/api/prixod-putevok/ruchnoy/${selectedNakladnoy!.id}/vouchers`, payload)
        .then(r => r.data),
    onSuccess: () => {
      message.success('Путевки добавлены');
      invalidate();
      setVoucherModalOpen(false);
      setVoucherStep('form');
      setGeneratedNumbers([]);
      setCheckedNumbers([]);
      voucherForm.resetFields();
    },
    onError: () => message.error('Ошибка при добавлении путевок'),
  });

  const deleteVoucherMutation = useMutation({
    mutationFn: (voucherId: number) =>
      api.delete(`/api/prixod-putevok/ruchnoy/vouchers/${voucherId}`),
    onSuccess: () => {
      message.success('Путевка удалена');
      invalidate();
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const bronMutation = useMutation({
    mutationFn: (voucherId: number) =>
      api.put(`/api/prixod-putevok/ruchnoy/vouchers/${voucherId}/bron`).then(r => r.data),
    onSuccess: () => { message.success('Бронь установлена'); invalidate(); },
    onError: () => message.error('Ошибка'),
  });

  const unbronMutation = useMutation({
    mutationFn: (voucherId: number) =>
      api.put(`/api/prixod-putevok/ruchnoy/vouchers/${voucherId}/unbron`).then(r => r.data),
    onSuccess: () => { message.success('Бронь снята'); invalidate(); },
    onError: () => message.error('Ошибка'),
  });

  const changeDateMutation = useMutation({
    mutationFn: ({ voucherId, newDate }: { voucherId: number; newDate: string }) =>
      api.put(`/api/prixod-putevok/ruchnoy/vouchers/${voucherId}/date`, { newDate }).then(r => r.data),
    onSuccess: () => { message.success('Дата изменена'); invalidate(); setChangeDateModalOpen(false); changeDateForm.resetFields(); },
    onError: () => message.error('Ошибка при изменении даты'),
  });

  // ── Helpers ────────────────────────────────────────────────────────────────
  const openAddInvoice = () => {
    setEditingInvoice(null);
    setSelectedOrg(null);
    invoiceForm.resetFields();
    setInvoiceModalOpen(true);
  };

  const openEditInvoice = () => {
    if (!selectedNakladnoy) { message.warning('Выберите накладную'); return; }
    setEditingInvoice(selectedNakladnoy);
    invoiceForm.setFieldsValue({
      ...selectedNakladnoy,
      dataPost: selectedNakladnoy.dataPost ? dayjs(selectedNakladnoy.dataPost) : undefined,
    });
    setInvoiceModalOpen(true);
  };

  const confirmDeleteInvoice = () => {
    if (!selectedNakladnoy) { message.warning('Выберите накладную'); return; }
    modal.confirm({
      title: 'Удалить накладную?',
      onOk: () => deleteInvoiceMutation.mutate(selectedNakladnoy.id),
    });
  };

  const handleProvodka = () => {
    if (!selectedNakladnoy) { message.warning('Выберите накладную'); return; }
    modal.confirm({
      title: 'Выполнить проводку?',
      onOk: () => provodkaMutation.mutate(selectedNakladnoy.id),
    });
  };

  const handleInvoiceFinish = (values: Record<string, unknown>) => {
    const payload = {
      ...values,
      dataPost: values.dataPost ? (values.dataPost as dayjs.Dayjs).toISOString() : undefined,
      posName: selectedOrg?.name ?? values.posName,
      year: filterYear,
      quarter: filterQuarter,
    };
    saveInvoiceMutation.mutate(payload);
  };

  // Voucher step 1: generate list of numbers from range
  const handleVoucherGenerate = (values: { beginNumber: number; endNumber: number; sanatId: number; sanatDate: dayjs.Dayjs; price: number }) => {
    const nums: string[] = [];
    for (let i = values.beginNumber; i <= values.endNumber; i++) {
      nums.push(String(i));
    }
    setGeneratedNumbers(nums);
    setCheckedNumbers(nums);
    setVoucherStep('confirm');
  };

  const handleVoucherConfirm = () => {
    const values = voucherForm.getFieldsValue();
    addVouchersMutation.mutate({
      sanatId: values.sanatId,
      sanatDate: values.sanatDate?.toISOString(),
      price: values.price,
      numbers: checkedNumbers,
    });
  };

  // Voucher action helpers (require single selected detail row)
  const [selectedDetail, setSelectedDetail] = useState<NakladnoyDtl | null>(null);
  const [changeDateModalOpen, setChangeDateModalOpen] = useState(false);
  const [changeDateForm] = Form.useForm();

  const requireDetail = (fn: () => void) => {
    if (!selectedDetail) { message.warning('Выберите путевку'); return; }
    fn();
  };

  const confirmDeleteVoucher = () =>
    requireDetail(() =>
      modal.confirm({
        title: 'Удалить путевку?',
        onOk: () => deleteVoucherMutation.mutate(selectedDetail!.id),
      })
    );

  const handleBron = () => requireDetail(() => bronMutation.mutate(selectedDetail!.id));
  const handleUnbron = () => requireDetail(() => unbronMutation.mutate(selectedDetail!.id));

  const yearOptions = Array.from({ length: 10 }, (_, i) => currentYear - 3 + i);

  // ── Table columns ──────────────────────────────────────────────────────────
  const invoiceColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Дата', dataIndex: 'dataPost', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '' },
    { title: '№ документа', dataIndex: 'numDoc' },
    { title: 'Отправитель', dataIndex: 'posName' },
    { title: 'Кол-во', dataIndex: 'putyovkaCount', width: 80 },
    { title: 'Сумма', dataIndex: 'summaLinked', width: 100 },
  ];

  const detailColumns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Санаторий', dataIndex: 'sanName' },
    { title: '№ путевки', dataIndex: 'numPut', width: 110 },
    { title: 'Дата', dataIndex: 'dataSan', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '' },
    { title: 'Сумма', dataIndex: 'symmaPut', width: 100 },
    { title: 'Статус', dataIndex: 'statusPutyovki' },
  ];

  // ── Render ─────────────────────────────────────────────────────────────────
  return (
    <div>
      {/* Title + Filters */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>
          Приход путевок (ручной)
        </Typography.Title>
        <Space>
          <Select
            value={filterYear}
            onChange={setFilterYear}
            style={{ width: 90 }}
          >
            {yearOptions.map(y => <Option key={y} value={y}>{y}</Option>)}
          </Select>
          <Select
            value={filterQuarter}
            onChange={setFilterQuarter}
            style={{ width: 110 }}
          >
            {[1, 2, 3, 4].map(q => <Option key={q} value={q}>Квартал {q}</Option>)}
          </Select>
        </Space>
      </div>

      {/* Stats row */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col span={8}>
          <Card size="small">
            <Statistic title="Накладных" value={stats.nakladnoyCount} />
          </Card>
        </Col>
        <Col span={8}>
          <Card size="small">
            <Statistic title="Путевок" value={stats.putyovkaCount} />
          </Card>
        </Col>
        <Col span={8}>
          <Card size="small">
            <Statistic
              title="Сумма накладных"
              value={stats.summaNakladnix}
              precision={2}
            />
          </Card>
        </Col>
      </Row>

      {/* Two-table layout */}
      <Row gutter={12}>
        {/* LEFT — Invoices (Nakladnoys) */}
        <Col span={12}>
          <Card
            size="small"
            title="Накладные"
            extra={
              <Space>
                <Button size="small" type="primary" icon={<PlusOutlined />} onClick={openAddInvoice}>
                  Добавить
                </Button>
                <Button size="small" icon={<EditOutlined />} onClick={openEditInvoice}
                  disabled={!selectedNakladnoy}>
                  Изменить
                </Button>
                <Button size="small" danger icon={<DeleteOutlined />} onClick={confirmDeleteInvoice}
                  disabled={!selectedNakladnoy}>
                  Удалить
                </Button>
                <Button size="small" onClick={handleProvodka} disabled={!selectedNakladnoy}
                  loading={provodkaMutation.isPending}>
                  Проводка
                </Button>
              </Space>
            }
          >
            <QueryErrorAlert isError={isError} onRetry={refetch} />
            <Table<Nakladnoy>
              columns={invoiceColumns}
              dataSource={nakladnoys}
              rowKey="id"
              loading={isLoading}
              size="small"
              pagination={{ pageSize: 15, size: 'small' }}
              rowSelection={{
                type: 'radio',
                selectedRowKeys: selectedNakladnoy ? [selectedNakladnoy.id] : [],
                onChange: (_, rows) => {
                  setSelectedNakladnoy(rows[0] ?? null);
                  setSelectedDetail(null);
                },
              }}
              onRow={rec => ({
                onClick: () => {
                  setSelectedNakladnoy(rec);
                  setSelectedDetail(null);
                },
                style: {
                  cursor: 'pointer',
                  background: selectedNakladnoy?.id === rec.id ? '#e6f4ff' : undefined,
                },
              })}
            />
          </Card>
        </Col>

        {/* RIGHT — Voucher details (NakladnoyDtls) */}
        <Col span={12}>
          <Card
            size="small"
            title="Путевки"
            extra={
              <Space>
                <Button
                  size="small"
                  type="primary"
                  icon={<PlusOutlined />}
                  disabled={!selectedNakladnoy}
                  onClick={() => {
                    setVoucherStep('form');
                    setGeneratedNumbers([]);
                    setCheckedNumbers([]);
                    voucherForm.resetFields();
                    setVoucherModalOpen(true);
                  }}
                >
                  Добавить
                </Button>
                <Button size="small" danger icon={<DeleteOutlined />} onClick={confirmDeleteVoucher}
                  disabled={!selectedDetail}>
                  Удалить
                </Button>
                <Button size="small" onClick={handleBron} disabled={!selectedDetail}
                  loading={bronMutation.isPending}>
                  Бронь
                </Button>
                <Button size="small" onClick={handleUnbron} disabled={!selectedDetail}
                  loading={unbronMutation.isPending}>
                  Снять Бронь
                </Button>
                <Button size="small" disabled={!selectedDetail}
                  onClick={() => requireDetail(() => { changeDateForm.resetFields(); setChangeDateModalOpen(true); })}>
                  Изм.Дату
                </Button>                <Button size="small" disabled={!selectedDetail}>
                  Печать
                </Button>
              </Space>
            }
          >
            <Table<NakladnoyDtl>
              columns={detailColumns}
              dataSource={details}
              rowKey="id"
              loading={detailsLoading}
              size="small"
              pagination={{ pageSize: 15, size: 'small' }}
              rowSelection={{
                type: 'radio',
                selectedRowKeys: selectedDetail ? [selectedDetail.id] : [],
                onChange: (_, rows) => setSelectedDetail(rows[0] ?? null),
              }}
              onRow={rec => ({
                onClick: () => setSelectedDetail(rec),
                style: {
                  cursor: 'pointer',
                  background: selectedDetail?.id === rec.id ? '#e6f4ff' : undefined,
                },
              })}
            />
          </Card>
        </Col>
      </Row>

      {/* ── Invoice Add/Edit Modal ── */}
      <Modal
        title={editingInvoice ? 'Изменить накладную' : 'Добавить накладную'}
        open={invoiceModalOpen}
        onOk={() => invoiceForm.submit()}
        onCancel={() => setInvoiceModalOpen(false)}
        confirmLoading={saveInvoiceMutation.isPending}
        width={480}
      >
        <Form form={invoiceForm} layout="vertical" onFinish={handleInvoiceFinish}>
          <Form.Item
            name="dataPost"
            label="Дата"
            rules={[{ required: true, message: 'Укажите дату' }]}
          >
            <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>
          <Form.Item
            name="numDoc"
            label="Номер документа"
          >
            <Input />
          </Form.Item>
          <Form.Item label="Отправитель">
            <Space.Compact style={{ width: '100%' }}>
              <Input
                value={selectedOrg?.name ?? ''}
                readOnly
                placeholder="Выберите организацию..."
                style={{ flex: 1 }}
              />
              <Button onClick={() => setOrgPickerOpen(true)}>Выбрать</Button>
            </Space.Compact>
          </Form.Item>
          <Form.Item
            name="numDov"
            label="Номер доверенности"
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>

      {/* ── Organization Picker Modal ── */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={org => {
          setSelectedOrg(org);
          setOrgPickerOpen(false);
        }}
      />

      {/* ── Add Vouchers Modal ── */}
      <Modal
        title="Добавить путевки"
        open={voucherModalOpen}
        onCancel={() => {
          setVoucherModalOpen(false);
          setVoucherStep('form');
          setGeneratedNumbers([]);
        }}
        footer={
          voucherStep === 'form'
            ? [
                <Button key="cancel" onClick={() => setVoucherModalOpen(false)}>Отмена</Button>,
                <Button key="next" type="primary" onClick={() => voucherForm.submit()}>
                  Далее
                </Button>,
              ]
            : [
                <Button key="back" onClick={() => setVoucherStep('form')}>Назад</Button>,
                <Button key="ok" type="primary" loading={addVouchersMutation.isPending}
                  onClick={handleVoucherConfirm}>
                  Сохранить
                </Button>,
              ]
        }
        width={520}
      >
        {voucherStep === 'form' ? (
          <Form form={voucherForm} layout="vertical" onFinish={handleVoucherGenerate}>
            <Form.Item
              name="sanatId"
              label="Санаторий"
              rules={[{ required: true, message: 'Укажите санаторий' }]}
            >
              <Select placeholder="Выберите санаторий">
                {sanatoriums.map(s => (
                  <Option key={s.id} value={s.id}>{s.name}</Option>
                ))}
              </Select>
            </Form.Item>
            <Row gutter={12}>
              <Col span={12}>
                <Form.Item
                  name="beginNumber"
                  label="Номер с"
                  rules={[{ required: true, message: 'Укажите начало' }]}
                >
                  <InputNumber style={{ width: '100%' }} min={1} />
                </Form.Item>
              </Col>
              <Col span={12}>
                <Form.Item
                  name="endNumber"
                  label="Номер по"
                  rules={[{ required: true, message: 'Укажите конец' }]}
                >
                  <InputNumber style={{ width: '100%' }} min={1} />
                </Form.Item>
              </Col>
            </Row>
            <Form.Item
              name="sanatDate"
              label="Дата"
              rules={[{ required: true, message: 'Укажите дату' }]}
            >
              <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
            </Form.Item>
            <Form.Item
              name="price"
              label="Цена"
              rules={[{ required: true, message: 'Укажите цену' }]}
            >
              <InputNumber style={{ width: '100%' }} min={0} />
            </Form.Item>
          </Form>
        ) : (
          <div>
            <Typography.Text strong style={{ display: 'block', marginBottom: 8 }}>
              Сгенерированные номера путевок ({generatedNumbers.length} шт.):
            </Typography.Text>
            <div style={{ maxHeight: 300, overflowY: 'auto', border: '1px solid #d9d9d9', borderRadius: 6, padding: '8px 12px' }}>
              <Checkbox.Group
                value={checkedNumbers}
                onChange={vals => setCheckedNumbers(vals as string[])}
              >
                <Row gutter={[8, 4]}>
                  {generatedNumbers.map(n => (
                    <Col span={6} key={n}>
                      <Checkbox value={n}>{n}</Checkbox>
                    </Col>
                  ))}
                </Row>
              </Checkbox.Group>
            </div>
            <Typography.Text type="secondary" style={{ marginTop: 8, display: 'block' }}>
              Выбрано: {checkedNumbers.length} из {generatedNumbers.length}
            </Typography.Text>
          </div>
        )}
      </Modal>
      {/* ── Change Date Modal ── */}
      <Modal
        title="Изменить дату путевки"
        open={changeDateModalOpen}
        onOk={() => changeDateForm.submit()}
        onCancel={() => setChangeDateModalOpen(false)}
        confirmLoading={changeDateMutation.isPending}
        width={320}
      >
        <Form form={changeDateForm} layout="vertical"
          onFinish={(vals) => changeDateMutation.mutate({ voucherId: selectedDetail!.id, newDate: (vals.newDate as dayjs.Dayjs).toISOString() })}>
          <Form.Item name="newDate" label="Новая дата" rules={[{ required: true, message: 'Укажите дату' }]}>
            <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
