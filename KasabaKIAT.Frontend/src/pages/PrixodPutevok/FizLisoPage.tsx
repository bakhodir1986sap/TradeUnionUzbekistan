import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, DatePicker,
  Row, Col, Typography, App,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined,
  FileExcelOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Option } = Select;

// ─── Types ───────────────────────────────────────────────────────────────────

interface FizLicoRecord {
  id: number;
  otraslName?: string;
  name_Org?: string;
  inn?: string;
  adress?: string;
  telefon?: string;
  pas_Ser?: string;
  pas_Num?: string;
  pas_KD?: string;
  pas_Data?: string;
  regionName?: string;
  otraslId?: number;
  regionId?: number;
}

interface OtraslItem {
  id: number;
  name?: string;
}

interface RegionItem {
  id: number;
  name?: string;
}

// ─── FizLisoPage ─────────────────────────────────────────────────────────────

export function FizLisoPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();

  // Filters
  const [filterName, setFilterName] = useState('');
  const [filterInn, setFilterInn] = useState('');
  const [filterSeria, setFilterSeria] = useState('');
  const [filterNomer, setFilterNomer] = useState('');

  // Applied filters (only updated on "Поиск" click)
  const [appliedFilters, setAppliedFilters] = useState({
    name: '',
    inn: '',
    seria: '',
    nomer: '',
  });

  // Modal state
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<FizLicoRecord | null>(null);

  // ── Queries ───────────────────────────────────────────────────────────────

  const { data: records = [], isLoading, isError, refetch } = useQuery<FizLicoRecord[]>({
    queryKey: ['fizliso', appliedFilters],
    queryFn: () =>
      api
        .get('/api/fizliso', {
          params: {
            name: appliedFilters.name || undefined,
            inn: appliedFilters.inn || undefined,
            seria: appliedFilters.seria || undefined,
            nomer: appliedFilters.nomer || undefined,
          },
        })
        .then(r => r.data),
  });

  const { data: otraslList = [] } = useQuery<OtraslItem[]>({
    queryKey: ['otrasl-list'],
    queryFn: () => api.get('/api/otrasl').then(r => r.data),
  });

  const { data: regionList = [] } = useQuery<RegionItem[]>({
    queryKey: ['region-list'],
    queryFn: () => api.get('/api/region').then(r => r.data),
  });

  // ── Mutations ─────────────────────────────────────────────────────────────

  const saveMutation = useMutation({
    mutationFn: (values: Partial<FizLicoRecord>) =>
      editing
        ? api.put(`/api/fizliso/${editing.id}`, values).then(r => r.data)
        : api.post('/api/fizliso', values).then(r => r.data),
    onSuccess: () => {
      message.success('Сохранено');
      queryClient.invalidateQueries({ queryKey: ['fizliso'] });
      setModalOpen(false);
      form.resetFields();
      setEditing(null);
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/fizliso/${id}`),
    onSuccess: () => {
      message.success('Удалено');
      queryClient.invalidateQueries({ queryKey: ['fizliso'] });
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  // ── Handlers ─────────────────────────────────────────────────────────────

  const handleSearch = () => {
    setAppliedFilters({
      name: filterName,
      inn: filterInn,
      seria: filterSeria,
      nomer: filterNomer,
    });
  };

  const handleExport = () => {
    window.open(
      `/api/fizliso/export?name=${encodeURIComponent(appliedFilters.name)}&inn=${encodeURIComponent(appliedFilters.inn)}&seria=${encodeURIComponent(appliedFilters.seria)}&nomer=${encodeURIComponent(appliedFilters.nomer)}`,
      '_blank',
    );
  };

  const openAdd = () => {
    setEditing(null);
    form.resetFields();
    setModalOpen(true);
  };

  const openEdit = (rec: FizLicoRecord) => {
    setEditing(rec);
    form.setFieldsValue({
      ...rec,
      pas_Data: rec.pas_Data ? dayjs(rec.pas_Data) : undefined,
    });
    setModalOpen(true);
  };

  const confirmDelete = (id: number) =>
    modal.confirm({
      title: 'Удалить запись?',
      content: 'Это действие нельзя отменить.',
      okText: 'Удалить',
      okButtonProps: { danger: true },
      onOk: () => deleteMutation.mutate(id),
    });

  const handleFormFinish = (values: Record<string, unknown>) => {
    saveMutation.mutate({
      ...values,
      pas_Data: values.pas_Data
        ? (values.pas_Data as dayjs.Dayjs).toISOString()
        : undefined,
    });
  };

  // ── Table columns ─────────────────────────────────────────────────────────

  const columns = [
    {
      title: 'Отрасль',
      dataIndex: 'otraslName',
      width: 150,
      align: 'center' as const,
    },
    {
      title: 'ФИО',
      dataIndex: 'name_Org',
      width: 200,
      align: 'center' as const,
    },
    {
      title: 'ИНН',
      dataIndex: 'inn',
      width: 130,
      align: 'center' as const,
    },
    {
      title: 'Адрес',
      dataIndex: 'adress',
      width: 150,
      align: 'center' as const,
    },
    {
      title: 'Телефон',
      dataIndex: 'telefon',
      width: 130,
      align: 'center' as const,
    },
    {
      title: 'Паспорт Серия',
      dataIndex: 'pas_Ser',
      width: 120,
      align: 'center' as const,
    },
    {
      title: 'Паспорт Номер',
      dataIndex: 'pas_Num',
      width: 130,
      align: 'center' as const,
    },
    {
      title: 'Кем выдан',
      dataIndex: 'pas_KD',
      width: 150,
      align: 'center' as const,
    },
    {
      title: 'Дата выдачи',
      dataIndex: 'pas_Data',
      width: 120,
      align: 'center' as const,
      render: (v: string) =>
        v ? dayjs(v).format('DD.MM.YYYY') : '—',
    },
    {
      title: 'Район',
      dataIndex: 'regionName',
      width: 150,
      align: 'center' as const,
    },
    {
      title: 'Действия',
      width: 90,
      align: 'center' as const,
      render: (_: unknown, rec: FizLicoRecord) => (
        <Space size={4}>
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
        </Space>
      ),
    },
  ];

  // ── Render ────────────────────────────────────────────────────────────────

  return (
    <div>
      {/* Header */}
      <div style={{ marginBottom: 16 }}>
        <Row align="middle" justify="space-between" style={{ marginBottom: 8 }}>
          <Col>
            <Typography.Title level={4} style={{ margin: 0 }}>
              База данных физических лиц
            </Typography.Title>
            <Typography.Text type="secondary">
              Справочник физических лиц
            </Typography.Text>
          </Col>
          <Col>
            <Space>
              <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
                Добавить
              </Button>
              <Button icon={<FileExcelOutlined />} onClick={handleExport}>
                Экспорт
              </Button>
            </Space>
          </Col>
        </Row>
      </div>

      {/* Filters */}
      <div style={{ marginBottom: 16 }}>
        <Row gutter={12} align="bottom">
          <Col xs={24} sm={6}>
            <div style={{ marginBottom: 4 }}>
              <Typography.Text>Наименование</Typography.Text>
            </div>
            <Input
              placeholder="Наименование"
              value={filterName}
              onChange={e => setFilterName(e.target.value)}
              onPressEnter={handleSearch}
              allowClear
            />
          </Col>
          <Col xs={24} sm={6}>
            <div style={{ marginBottom: 4 }}>
              <Typography.Text>ИНН</Typography.Text>
            </div>
            <Input
              placeholder="ИНН"
              value={filterInn}
              onChange={e => setFilterInn(e.target.value)}
              maxLength={9}
              onPressEnter={handleSearch}
              allowClear
            />
          </Col>
          <Col xs={24} sm={5}>
            <div style={{ marginBottom: 4 }}>
              <Typography.Text>Паспорт серия</Typography.Text>
            </div>
            <Input
              placeholder="серия"
              value={filterSeria}
              onChange={e => setFilterSeria(e.target.value)}
              onPressEnter={handleSearch}
              allowClear
            />
          </Col>
          <Col xs={24} sm={5}>
            <div style={{ marginBottom: 4 }}>
              <Typography.Text>Паспорт номер</Typography.Text>
            </div>
            <Input
              placeholder="номер"
              value={filterNomer}
              onChange={e => setFilterNomer(e.target.value)}
              onPressEnter={handleSearch}
              allowClear
            />
          </Col>
          <Col xs={24} sm={2}>
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleSearch}
              style={{ width: '100%' }}
            >
              Поиск
            </Button>
          </Col>
        </Row>
      </div>

      {/* Table */}
      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table
        columns={columns}
        dataSource={records}
        rowKey="id"
        loading={isLoading}
        size="small"
        scroll={{ x: 1300 }}
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: total => `Всего: ${total}` }}
      />

      {/* Add / Edit Modal */}
      <Modal
        title={editing ? 'Редактировать физическое лицо' : 'Добавить физическое лицо'}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => {
          setModalOpen(false);
          form.resetFields();
          setEditing(null);
        }}
        confirmLoading={saveMutation.isPending}
        width={680}
        destroyOnClose
      >
        <Form form={form} layout="vertical" onFinish={handleFormFinish}>
          {/* ФИО */}
          <Form.Item
            name="name_Org"
            label="ФИО"
            rules={[{ required: true, message: 'заполнить поле' }]}
          >
            <Input maxLength={100} placeholder="Фамилия Имя Отчество" />
          </Form.Item>

          {/* ИНН */}
          <Form.Item
            name="inn"
            label="ИНН"
            rules={[{ required: true, message: 'заполнить поле' }]}
          >
            <Input maxLength={9} placeholder="ИНН (9 цифр)" />
          </Form.Item>

          {/* Отрасль */}
          <Form.Item
            name="otraslId"
            label="Отрасль"
            rules={[{ required: true, message: 'заполнить поле' }]}
          >
            <Select
              placeholder="Выберите отрасль"
              showSearch
              optionFilterProp="children"
              allowClear
            >
              {otraslList.map(o => (
                <Option key={o.id} value={o.id}>
                  {o.name}
                </Option>
              ))}
            </Select>
          </Form.Item>

          <Row gutter={12}>
            {/* Адрес */}
            <Col span={12}>
              <Form.Item name="adress" label="Адрес">
                <Input placeholder="Адрес проживания" />
              </Form.Item>
            </Col>
            {/* Телефон */}
            <Col span={12}>
              <Form.Item name="telefon" label="Телефон">
                <Input placeholder="+998 XX XXX XX XX" />
              </Form.Item>
            </Col>
          </Row>

          <Row gutter={12}>
            {/* Паспорт серия */}
            <Col span={8}>
              <Form.Item name="pas_Ser" label="Паспорт серия">
                <Input maxLength={2} placeholder="AA" />
              </Form.Item>
            </Col>
            {/* Паспорт номер */}
            <Col span={8}>
              <Form.Item name="pas_Num" label="Паспорт номер">
                <Input maxLength={7} placeholder="1234567" />
              </Form.Item>
            </Col>
            {/* Дата выдачи */}
            <Col span={8}>
              <Form.Item name="pas_Data" label="Дата выдачи">
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
          </Row>

          {/* Кем выдан */}
          <Form.Item name="pas_KD" label="Кем выдан">
            <Input placeholder="Орган, выдавший паспорт" />
          </Form.Item>

          {/* Район */}
          <Form.Item name="regionId" label="Район">
            <Select
              placeholder="Выберите район"
              showSearch
              optionFilterProp="children"
              allowClear
            >
              {regionList.map(r => (
                <Option key={r.id} value={r.id}>
                  {r.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
