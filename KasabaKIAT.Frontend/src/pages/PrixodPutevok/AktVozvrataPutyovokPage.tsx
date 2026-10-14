import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, DatePicker,
  Row, Col, Typography, App, Statistic, Card, Checkbox,
} from 'antd';
import {
  PlusOutlined, PrinterOutlined, DeleteOutlined, EditOutlined, SearchOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../api/axios';
import dayjs, { Dayjs } from 'dayjs';

const { Title } = Typography;
const { Option } = Select;

// ─── Interfaces ─────────────────────────────────────────────────────────────

interface AktVozvrat {
  id: number;
  data_post: string;
  num_doc: string;
  orgName: string;
  sanatoriyName: string;
  num_put: string;
  symmap: number;
}

interface AktStats {
  putyovkaCount: number;
  summaNakladnix: number;
}

interface AktListResponse {
  akts: AktVozvrat[];
  stats: AktStats;
}

interface Organization {
  id: number;
  name: string;
  inn?: string;
}

interface AvailableVoucher {
  id: number;
  sanatName: string;
  num_put: string;
  data_post: string;
  symma: number;
}

// ─── Organization Picker Modal ────────────────────────────────────────────────

interface OrgPickerProps {
  open: boolean;
  onClose: () => void;
  onSelect: (org: Organization) => void;
}

function OrganizationPickerModal({ open, onClose, onSelect }: OrgPickerProps) {
  const { message } = App.useApp();
  const [search, setSearch] = useState('');
  const [results, setResults] = useState<Organization[]>([]);
  const [searching, setSearching] = useState(false);

  const handleSearch = async () => {
    setSearching(true);
    try {
      const res = await api.get<Organization[]>('/api/organizations', {
        params: { search: search.trim() },
      });
      setResults(res.data);
    } catch {
      message.error('Ошибка при поиске организаций');
    } finally {
      setSearching(false);
    }
  };

  const handleClose = () => {
    setSearch('');
    setResults([]);
    onClose();
  };

  const columns = [
    { title: 'Название', dataIndex: 'name', key: 'name' },
    { title: 'ИНН', dataIndex: 'inn', key: 'inn' },
    {
      title: '',
      key: 'action',
      width: 90,
      render: (_: unknown, record: Organization) => (
        <Button type="primary" size="small" onClick={() => { onSelect(record); handleClose(); }}>
          Выбрать
        </Button>
      ),
    },
  ];

  return (
    <Modal
      title="Поиск организации"
      open={open}
      onCancel={handleClose}
      footer={null}
      width={640}
    >
      <Space.Compact style={{ width: '100%', marginBottom: 16 }}>
        <Input
          placeholder="Поиск по названию или ИНН..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onPressEnter={handleSearch}
        />
        <Button
          type="primary"
          icon={<SearchOutlined />}
          loading={searching}
          onClick={handleSearch}
        >
          Найти
        </Button>
      </Space.Compact>
      <Table
        dataSource={results}
        columns={columns}
        rowKey="id"
        size="small"
        pagination={{ pageSize: 8 }}
      />
    </Modal>
  );
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function AktVozvrataPutyovokPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [addForm] = Form.useForm();
  const [izmForm] = Form.useForm();

  // ── Filter state
  const currentYear = dayjs().year();
  const [year, setYear] = useState<number>(currentYear);
  const [quarter, setQuarter] = useState<number>(1);

  // ── Main list selection
  const [selectedRowKeys, setSelectedRowKeys] = useState<number[]>([]);

  // ── Add modal
  const [addOpen, setAddOpen] = useState(false);
  const [selectedOrg, setSelectedOrg] = useState<Organization | null>(null);
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);
  const [selectedVoucherIds, setSelectedVoucherIds] = useState<number[]>([]);

  // ── IzmData modal
  const [izmOpen, setIzmOpen] = useState(false);

  // ── Year options
  const yearOptions = Array.from({ length: 10 }, (_, i) => currentYear - i);

  // ─── Queries ──────────────────────────────────────────────────────────────

  const { data: listData, isLoading } = useQuery<AktListResponse>({
    queryKey: ['akt-vozvrata', year, quarter],
    queryFn: () =>
      api.get('/api/akt-vozvrata', { params: { year, quarter } }).then((r) => r.data),
  });

  const akts = listData?.akts ?? [];
  const stats = listData?.stats ?? { putyovkaCount: 0, summaNakladnix: 0 };

  const { data: availableVouchers = [], isLoading: vouchersLoading } = useQuery<AvailableVoucher[]>({
    queryKey: ['akt-vozvrata-available-vouchers', selectedOrg?.id],
    queryFn: () =>
      api.get('/api/akt-vozvrata/available-vouchers', { params: { orgId: selectedOrg!.id } }).then((r) => r.data),
    enabled: !!selectedOrg?.id && addOpen,
  });

  // ─── Mutations ────────────────────────────────────────────────────────────

  const addMutation = useMutation({
    mutationFn: (payload: object) => api.post('/api/akt-vozvrata', payload).then((r) => r.data),
    onSuccess: () => {
      message.success('Акт успешно добавлен');
      queryClient.invalidateQueries({ queryKey: ['akt-vozvrata'] });
      setAddOpen(false);
      addForm.resetFields();
      setSelectedOrg(null);
      setSelectedVoucherIds([]);
    },
    onError: () => message.error('Ошибка при добавлении акта'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/akt-vozvrata/${id}`),
    onSuccess: () => {
      message.success('Запись удалена');
      queryClient.invalidateQueries({ queryKey: ['akt-vozvrata'] });
      setSelectedRowKeys([]);
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const izmMutation = useMutation({
    mutationFn: ({ id, newDate }: { id: number; newDate: string }) =>
      api.put(`/api/akt-vozvrata/${id}/date`, { newDate }).then((r) => r.data),
    onSuccess: () => {
      message.success('Дата возврата изменена');
      queryClient.invalidateQueries({ queryKey: ['akt-vozvrata'] });
      setIzmOpen(false);
      izmForm.resetFields();
    },
    onError: () => message.error('Ошибка при изменении даты'),
  });

  // ─── Handlers ─────────────────────────────────────────────────────────────

  const handleAddOk = () => {
    addForm.validateFields().then((values) => {
      if (!selectedOrg) {
        message.warning('Выберите организацию');
        return;
      }
      if (selectedVoucherIds.length === 0) {
        message.warning('Выберите путевки');
        return;
      }
      addMutation.mutate({
        data_post: (values.data_post as Dayjs).toISOString(),
        num_doc: values.num_doc,
        dover_fio: values.dover_fio,
        dover_num: values.dover_num,
        orgId: selectedOrg.id,
        voucherIds: selectedVoucherIds,
      });
    });
  };

  const handleOpenAdd = () => {
    addForm.resetFields();
    setSelectedOrg(null);
    setSelectedVoucherIds([]);
    setAddOpen(true);
  };

  const handleDelete = () => {
    if (selectedRowKeys.length === 0) {
      message.warning('Выберите запись для удаления');
      return;
    }
    modal.confirm({
      title: 'Удалить выбранные записи?',
      onOk: () => {
        selectedRowKeys.forEach((id) => deleteMutation.mutate(id));
      },
    });
  };

  const handlePrint = () => {
    if (selectedRowKeys.length === 0) {
      message.warning('Выберите запись для печати');
      return;
    }
    const id = selectedRowKeys[0];
    window.open(`${import.meta.env.VITE_API_URL}/api/akt-vozvrata/print/${id}`, '_blank');
  };

  const handleIzmOpen = () => {
    if (selectedRowKeys.length === 0) {
      message.warning('Выберите запись для изменения даты');
      return;
    }
    izmForm.resetFields();
    setIzmOpen(true);
  };

  const handleIzmOk = () => {
    izmForm.validateFields().then((values) => {
      const id = selectedRowKeys[0];
      izmMutation.mutate({
        id,
        newDate: (values.newDate as Dayjs).toISOString(),
      });
    });
  };

  const handleVoucherToggle = (id: number, checked: boolean) => {
    setSelectedVoucherIds((prev) =>
      checked ? [...prev, id] : prev.filter((v) => v !== id)
    );
  };

  // ─── Table columns ────────────────────────────────────────────────────────

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id', width: 60 },
    {
      title: 'Дата',
      dataIndex: 'data_post',
      key: 'data_post',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    { title: '№ документа', dataIndex: 'num_doc', key: 'num_doc' },
    { title: 'Организация', dataIndex: 'orgName', key: 'orgName' },
    { title: 'Санаторий', dataIndex: 'sanatoriyName', key: 'sanatoriyName' },
    { title: '№ путевки', dataIndex: 'num_put', key: 'num_put' },
    { title: 'Сумма', dataIndex: 'symmap', key: 'symmap' },
  ];

  // ─── Available vouchers table columns ────────────────────────────────────

  const voucherColumns = [
    {
      title: '',
      key: 'check',
      width: 40,
      render: (_: unknown, record: AvailableVoucher) => (
        <Checkbox
          checked={selectedVoucherIds.includes(record.id)}
          onChange={(e) => handleVoucherToggle(record.id, e.target.checked)}
        />
      ),
    },
    { title: 'ID', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Санаторий', dataIndex: 'sanatName', key: 'sanatName' },
    { title: '№ путевки', dataIndex: 'num_put', key: 'num_put' },
    {
      title: 'Дата',
      dataIndex: 'data_post',
      key: 'data_post',
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '',
    },
    { title: 'Сумма', dataIndex: 'symma', key: 'symma' },
  ];

  // ─── Render ───────────────────────────────────────────────────────────────

  return (
    <div style={{ padding: 24 }}>
      <Title level={3}>Акт возврата путевок</Title>

      {/* Filters */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col>
          <Select
            value={year}
            onChange={setYear}
            style={{ width: 100 }}
          >
            {yearOptions.map((y) => (
              <Option key={y} value={y}>{y}</Option>
            ))}
          </Select>
        </Col>
        <Col>
          <Select
            value={quarter}
            onChange={setQuarter}
            style={{ width: 120 }}
          >
            <Option value={1}>1 квартал</Option>
            <Option value={2}>2 квартал</Option>
            <Option value={3}>3 квартал</Option>
            <Option value={4}>4 квартал</Option>
          </Select>
        </Col>
      </Row>

      {/* Stats */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={6}>
          <Card>
            <Statistic title="Количество путевок" value={stats.putyovkaCount} />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Card>
            <Statistic title="Сумма накладных" value={stats.summaNakladnix} precision={2} />
          </Card>
        </Col>
      </Row>

      {/* Action buttons */}
      <Space style={{ marginBottom: 12 }}>
        <Button type="primary" icon={<PlusOutlined />} onClick={handleOpenAdd}>
          Добавить
        </Button>
        <Button icon={<PrinterOutlined />} onClick={handlePrint}>
          Печать
        </Button>
        <Button danger icon={<DeleteOutlined />} onClick={handleDelete} loading={deleteMutation.isPending}>
          Удалить
        </Button>
        <Button icon={<EditOutlined />} onClick={handleIzmOpen}>
          Изменить Дату
        </Button>
      </Space>

      {/* Main table */}
      <Table
        dataSource={akts}
        columns={columns}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={{ pageSize: 20 }}
        rowSelection={{
          type: 'checkbox',
          selectedRowKeys,
          onChange: (keys) => setSelectedRowKeys(keys as number[]),
        }}
      />

      {/* Add modal */}
      <Modal
        title="Добавить акт возврата"
        open={addOpen}
        onOk={handleAddOk}
        onCancel={() => setAddOpen(false)}
        confirmLoading={addMutation.isPending}
        width={700}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={addForm} layout="vertical">
          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item
                name="data_post"
                label="Дата документа"
                rules={[{ required: true, message: 'Укажите дату' }]}
              >
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item
                name="num_doc"
                label="№ документа"
                rules={[{ required: true, message: 'Укажите номер документа' }]}
              >
                <Input />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={16}>
            <Col xs={24} sm={12}>
              <Form.Item name="dover_fio" label="Доверенность ФИО">
                <Input placeholder="ФИО доверенного лица" />
              </Form.Item>
            </Col>
            <Col xs={24} sm={12}>
              <Form.Item name="dover_num" label="№ доверенности">
                <Input placeholder="Номер доверенности" />
              </Form.Item>
            </Col>
          </Row>

          <Form.Item label="Организация" required>
            <Space>
              <Button onClick={() => setOrgPickerOpen(true)}>
                Выбрать организацию
              </Button>
              {selectedOrg && (
                <span style={{ color: '#1677ff' }}>{selectedOrg.name}</span>
              )}
            </Space>
          </Form.Item>

          {selectedOrg && (
            <Form.Item label="Путевки для возврата">
              <Table
                dataSource={availableVouchers}
                columns={voucherColumns}
                rowKey="id"
                size="small"
                loading={vouchersLoading}
                pagination={false}
                scroll={{ y: 220 }}
              />
              {selectedVoucherIds.length > 0 && (
                <div style={{ marginTop: 4, color: '#888' }}>
                  Выбрано: {selectedVoucherIds.length}
                </div>
              )}
            </Form.Item>
          )}
        </Form>
      </Modal>

      {/* Organization picker sub-modal */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={(org) => {
          setSelectedOrg(org);
          setSelectedVoucherIds([]);
        }}
      />

      {/* IzmData modal */}
      <Modal
        title="Изменить дату возврата"
        open={izmOpen}
        onOk={handleIzmOk}
        onCancel={() => setIzmOpen(false)}
        confirmLoading={izmMutation.isPending}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={izmForm} layout="vertical">
          <Form.Item
            name="newDate"
            label="Новая дата возврата"
            rules={[{ required: true, message: 'Укажите новую дату' }]}
          >
            <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
