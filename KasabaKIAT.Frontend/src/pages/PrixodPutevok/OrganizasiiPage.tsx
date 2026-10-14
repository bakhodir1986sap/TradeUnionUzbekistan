import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select,
  App, Typography, Row, Col, Divider, Upload, Tooltip,
} from 'antd';
import type { ColumnsType } from 'antd/es/table';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined,
  FileExcelOutlined, UploadOutlined, DownloadOutlined, BranchesOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../api/axios';

const { Option } = Select;

/* ─── Types ────────────────────────────────────────────────────────────────── */

interface Otrasl {
  id_Ot: number;
  name_Ot: string;
}

interface OrgRecord {
  id_Org: number;
  name_Org?: string;
  inn?: string;
  raschetnyiSchet?: string;
  mfo?: string;
  adres?: string;
  tel?: string;
  rukovoditel?: string;
  predsedatel?: string;
  id_Ot?: number;
  otraslName?: string;
  region?: string;
  gorod?: string;
  email?: string;
  numDogovora?: string;
  dataDogovora?: string;
}

interface OrgFilterParams {
  name?: string;
  inn?: string;
  id_Ot?: number;
  page: number;
  pageSize: number;
}

interface OrgFilterResult {
  items: OrgRecord[];
  totalCount: number;
  totalCountNeot: number;
  totalCountOpr: number;
}

interface ChangeOtraslPayload {
  orgIds: number[];
  id_Ot: number;
}

/* ─── Page ──────────────────────────────────────────────────────────────────── */

export function OrganizasiiPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [changeOtraslForm] = Form.useForm();

  // modal state
  const [modalOpen, setModalOpen] = useState(false);
  const [changeOtraslModalOpen, setChangeOtraslModalOpen] = useState(false);
  const [editing, setEditing] = useState<OrgRecord | null>(null);

  // filter state
  const [filterName, setFilterName] = useState('');
  const [filterInn, setFilterInn] = useState('');
  const [filterOtrasl, setFilterOtrasl] = useState<number | undefined>(undefined);

  // pagination state
  const [curPage, setCurPage] = useState(0);
  const pageSize = 20;

  // selection for mass otrasl assignment
  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);

  // file upload state
  const [selectedOtraslFile, setSelectedOtraslFile] = useState<File | null>(null);

  // active filter params (applied on search click)
  const [activeFilter, setActiveFilter] = useState<OrgFilterParams>({
    name: '',
    inn: '',
    id_Ot: undefined,
    page: 0,
    pageSize,
  });

  /* ─── Queries ─────────────────────────────────────────────────────────────── */

  const { data: otrasls = [] } = useQuery<Otrasl[]>({
    queryKey: ['otrasls'],
    queryFn: () => api.get('/api/otrasl').then(r => r.data),
  });

  const { data: filterResult, isLoading } = useQuery<OrgFilterResult>({
    queryKey: ['organizasii-filter', activeFilter],
    queryFn: () =>
      api
        .get('/api/organizasii', {
          params: {
            name: activeFilter.name || undefined,
            inn: activeFilter.inn || undefined,
            id_Ot: activeFilter.id_Ot || undefined,
            page: activeFilter.page,
            pageSize: activeFilter.pageSize,
          },
        })
        .then(r => r.data),
  });

  const organizations: OrgRecord[] = filterResult?.items ?? [];
  const totalCount = filterResult?.totalCount ?? 0;
  const totalCountNeot = filterResult?.totalCountNeot ?? 0;
  const totalCountOpr = filterResult?.totalCountOpr ?? 0;

  /* ─── Mutations ───────────────────────────────────────────────────────────── */

  const saveMutation = useMutation({
    mutationFn: (values: Partial<OrgRecord>) =>
      editing
        ? api.put(`/api/organizasii/${editing.id_Org}`, values).then(r => r.data)
        : api.post('/api/organizasii', values).then(r => r.data),
    onSuccess: () => {
      message.success('Сохранено успешно');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
      setModalOpen(false);
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/organizasii/${id}`),
    onSuccess: () => {
      message.success('Организация удалена');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const changeOtraslMutation = useMutation({
    mutationFn: (payload: ChangeOtraslPayload) =>
      api.post('/api/organizasii/change-otrasl', payload).then(r => r.data),
    onSuccess: () => {
      message.success('Отрасль назначена успешно');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
      setChangeOtraslModalOpen(false);
      setSelectedRowKeys([]);
    },
    onError: () => message.error('Ошибка при назначении отрасли'),
  });

  const exportMutation = useMutation({
    mutationFn: () =>
      api
        .get('/api/organizasii/export', {
          responseType: 'blob',
          params: {
            name: activeFilter.name || undefined,
            inn: activeFilter.inn || undefined,
            id_Ot: activeFilter.id_Ot || undefined,
          },
        })
        .then(r => r.data),
    onSuccess: (blob: Blob) => {
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'Organizasii.xlsx';
      a.click();
      URL.revokeObjectURL(url);
    },
    onError: () => message.error('Ошибка при экспорте'),
  });

  const uploadOtraslMutation = useMutation({
    mutationFn: (file: File) => {
      const formData = new FormData();
      formData.append('OtraslFile', file);
      return api.post('/api/organizasii/upload-otrasl', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
    },
    onSuccess: () => {
      message.success('Отраслевка загружена успешно');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
      setSelectedOtraslFile(null);
    },
    onError: () => message.error('Ошибка при загрузке файла'),
  });

  /* ─── Handlers ────────────────────────────────────────────────────────────── */

  const handleSearch = () => {
    setCurPage(0);
    setActiveFilter({ name: filterName, inn: filterInn, id_Ot: filterOtrasl, page: 0, pageSize });
  };

  const handlePrev = () => {
    const newPage = curPage - 1;
    setCurPage(newPage);
    setActiveFilter(prev => ({ ...prev, page: newPage }));
  };

  const handleNext = () => {
    const newPage = curPage + 1;
    setCurPage(newPage);
    setActiveFilter(prev => ({ ...prev, page: newPage }));
  };

  const openAdd = () => {
    setEditing(null);
    form.resetFields();
    setModalOpen(true);
  };

  const openEdit = (rec: OrgRecord) => {
    setEditing(rec);
    form.setFieldsValue(rec);
    setModalOpen(true);
  };

  const confirmDelete = (id: number) =>
    modal.confirm({
      title: 'Удалить организацию?',
      content: 'Это действие нельзя отменить.',
      okText: 'Удалить',
      okType: 'danger',
      cancelText: 'Отмена',
      onOk: () => deleteMutation.mutate(id),
    });

  const openChangeOtrasl = () => {
    if (selectedRowKeys.length === 0) {
      message.warning('Выберите организации для назначения отрасли');
      return;
    }
    changeOtraslForm.resetFields();
    setChangeOtraslModalOpen(true);
  };

  const handleChangeOtraslSubmit = (values: { id_Ot: number }) => {
    changeOtraslMutation.mutate({
      orgIds: selectedRowKeys as number[],
      id_Ot: values.id_Ot,
    });
  };

  const handleDownloadTemplate = async () => {
    try {
      const response = await api.get('/api/prixod-putevok/download-template-neopr', {
        responseType: 'blob',
      });
      const url = URL.createObjectURL(response.data);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'ShablonNeopr.xlsx';
      a.click();
      URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка при загрузке шаблона');
    }
  };

  const handleParseOtrasl = () => {
    if (!selectedOtraslFile) {
      message.warning('Выберите файл для загрузки');
      return;
    }
    uploadOtraslMutation.mutate(selectedOtraslFile);
  };

  /* ─── Table columns ───────────────────────────────────────────────────────── */

  const columns: ColumnsType<OrgRecord> = [
    {
      title: 'Номер',
      dataIndex: 'id_Org',
      width: 80,
      align: 'center',
    },
    {
      title: 'Отрасль',
      dataIndex: 'otraslName',
      width: 150,
      align: 'center',
      render: (v: string | undefined) => v ?? <span style={{ color: '#bbb' }}>—</span>,
    },
    {
      title: 'Название организации',
      dataIndex: 'name_Org',
      width: 200,
      align: 'center',
    },
    {
      title: 'ИНН Профкома',
      dataIndex: 'inn',
      width: 150,
      align: 'center',
    },
    {
      title: 'Расчетный счет',
      dataIndex: 'raschetnyiSchet',
      width: 180,
      align: 'center',
    },
    {
      title: 'МФО',
      dataIndex: 'mfo',
      width: 100,
      align: 'center',
    },
    {
      title: 'Адрес',
      dataIndex: 'adres',
      width: 180,
      align: 'center',
    },
    {
      title: 'Телефон',
      dataIndex: 'tel',
      width: 130,
      align: 'center',
    },
    {
      title: 'Руководитель',
      dataIndex: 'rukovoditel',
      width: 160,
      align: 'center',
    },
    {
      title: 'Председатель',
      dataIndex: 'predsedatel',
      width: 160,
      align: 'center',
    },
    {
      title: 'Город',
      dataIndex: 'gorod',
      width: 120,
      align: 'center',
    },
    {
      title: 'Email',
      dataIndex: 'email',
      width: 160,
      align: 'center',
    },
    {
      title: 'Действия',
      key: 'actions',
      width: 100,
      align: 'center',
      fixed: 'right' as const,
      render: (_: unknown, rec: OrgRecord) => (
        <Space>
          <Tooltip title="Редактировать">
            <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          </Tooltip>
          <Tooltip title="Удалить">
            <Button
              size="small"
              danger
              icon={<DeleteOutlined />}
              onClick={() => confirmDelete(rec.id_Org)}
            />
          </Tooltip>
        </Space>
      ),
    },
  ];

  /* ─── Render ──────────────────────────────────────────────────────────────── */

  return (
    <div>
      {/* Header */}
      <div style={{ marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>
          База данных организаций
        </Typography.Title>
        <Typography.Text type="secondary">Справочник Организаций</Typography.Text>
      </div>

      {/* Action buttons */}
      <Row gutter={8} style={{ marginBottom: 16 }}>
        <Col>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd} danger>
            Добавить
          </Button>
        </Col>
        <Col>
          <Button icon={<BranchesOutlined />} onClick={openChangeOtrasl} type="default" style={{ background: '#1677ff', color: '#fff', border: 'none' }}>
            Назначить отрасль
          </Button>
        </Col>
        <Col>
          <Button
            icon={<FileExcelOutlined />}
            onClick={() => exportMutation.mutate()}
            loading={exportMutation.isPending}
            style={{ background: '#52c41a', color: '#fff', border: 'none' }}
          >
            Экспорт
          </Button>
        </Col>
      </Row>

      {/* Filters */}
      <Row gutter={[16, 8]} style={{ marginBottom: 8 }}>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>Наименование</div>
            <Input
              placeholder="Наименование"
              value={filterName}
              onChange={e => setFilterName(e.target.value)}
              onPressEnter={handleSearch}
              allowClear
            />
          </div>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>ИНН</div>
            <Input
              placeholder="ИНН"
              value={filterInn}
              onChange={e => setFilterInn(e.target.value)}
              onPressEnter={handleSearch}
              allowClear
            />
          </div>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>Отрасль</div>
            <Select
              style={{ width: '100%' }}
              placeholder="Выберите отрасль"
              allowClear
              value={filterOtrasl}
              onChange={v => setFilterOtrasl(v)}
            >
              {otrasls.map(o => (
                <Option key={o.id_Ot} value={o.id_Ot}>
                  {o.name_Ot}
                </Option>
              ))}
            </Select>
          </div>
        </Col>
        <Col xs={24} sm={12} md={3}>
          <div>
            <div style={{ marginBottom: 4 }}>Общее кол-во</div>
            <Input value={totalCount} readOnly />
          </div>
        </Col>
      </Row>

      <Row gutter={[16, 8]} style={{ marginBottom: 8 }}>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>Не определенных</div>
            <Input value={totalCountNeot} readOnly />
          </div>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>Определенных</div>
            <Input value={totalCountOpr} readOnly />
          </div>
        </Col>
        <Col xs={24} sm={12} md={6} style={{ paddingTop: 28 }}>
          <Button
            href="/Otchot/OtchetOrganizasii"
            style={{ background: '#1677ff', color: '#fff', border: 'none' }}
          >
            Отчеты Организаций
          </Button>
        </Col>
        <Col xs={24} sm={12} md={6} style={{ paddingTop: 28 }}>
          <Button
            icon={<SearchOutlined />}
            style={{ background: '#52c41a', color: '#fff', border: 'none' }}
            onClick={handleSearch}
          >
            Поиск
          </Button>
        </Col>
      </Row>

      {/* Template download + file upload */}
      <Row gutter={[16, 8]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={6}>
          <Button
            icon={<DownloadOutlined />}
            onClick={handleDownloadTemplate}
            type="link"
            style={{ paddingLeft: 0 }}
          >
            Загрузить Шаблон
          </Button>
        </Col>
        <Col xs={24} sm={12} md={8}>
          <Upload
            accept=".xlsx,.xls"
            beforeUpload={file => {
              setSelectedOtraslFile(file);
              return false;
            }}
            showUploadList={selectedOtraslFile ? { showRemoveIcon: true } : false}
            onRemove={() => setSelectedOtraslFile(null)}
            maxCount={1}
          >
            <Button icon={<UploadOutlined />}>Выбрать файл отраслевки</Button>
          </Upload>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Button
            type="primary"
            onClick={handleParseOtrasl}
            disabled={!selectedOtraslFile}
            loading={uploadOtraslMutation.isPending}
          >
            Загрузить Отраслевку
          </Button>
        </Col>
      </Row>

      {/* Pagination */}
      {totalCount > 0 && (
        <Row style={{ marginBottom: 12 }}>
          <Col>
            <Space>
              <Button
                type="primary"
                disabled={curPage === 0}
                onClick={handlePrev}
              >
                &lt; Предыдущий
              </Button>
              <span>Страница {curPage + 1}</span>
              <Button
                type="primary"
                disabled={curPage >= Math.ceil(totalCount / pageSize) - 1}
                onClick={handleNext}
              >
                Следующий &gt;
              </Button>
            </Space>
          </Col>
        </Row>
      )}

      {/* Table */}
      <Table
        columns={columns}
        dataSource={organizations}
        rowKey="id_Org"
        loading={isLoading}
        size="small"
        pagination={false}
        scroll={{ x: 1600 }}
        rowSelection={{
          selectedRowKeys,
          onChange: keys => setSelectedRowKeys(keys),
        }}
        style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.09)' }}
      />

      {/* Add/Edit Organization Modal */}
      <Modal
        title={editing ? 'Редактировать организацию' : 'Добавить организацию'}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={680}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="name_Org" label="Название организации" rules={[{ required: true, message: 'Введите название' }]}>
                <Input placeholder="Название организации" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="inn" label="ИНН Профкома">
                <Input placeholder="ИНН" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="raschetnyiSchet" label="Расчетный счет">
                <Input placeholder="Расчетный счет" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="mfo" label="МФО">
                <Input placeholder="МФО" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="id_Ot" label="Отрасль">
                <Select placeholder="Выберите отрасль" allowClear>
                  {otrasls.map(o => (
                    <Option key={o.id_Ot} value={o.id_Ot}>
                      {o.name_Ot}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="region" label="Регион">
                <Input placeholder="Регион" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="gorod" label="Город">
                <Input placeholder="Город" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="adres" label="Адрес">
                <Input placeholder="Адрес" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="tel" label="Телефон">
                <Input placeholder="Телефон" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="email" label="Email">
                <Input placeholder="Email" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="rukovoditel" label="Руководитель">
                <Input placeholder="ФИО руководителя" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="predsedatel" label="Председатель">
                <Input placeholder="ФИО председателя" />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="numDogovora" label="Номер договора">
                <Input placeholder="Номер договора" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="dataDogovora" label="Дата договора">
                <Input placeholder="дд.мм.гггг" />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* Change Otrasl for selected Modal */}
      <Modal
        title={`Назначить отрасль (выбрано: ${selectedRowKeys.length})`}
        open={changeOtraslModalOpen}
        onOk={() => changeOtraslForm.submit()}
        onCancel={() => setChangeOtraslModalOpen(false)}
        confirmLoading={changeOtraslMutation.isPending}
        okText="Назначить"
        cancelText="Отмена"
        width={400}
      >
        <Form form={changeOtraslForm} layout="vertical" onFinish={handleChangeOtraslSubmit}>
          <Form.Item
            name="id_Ot"
            label="Отрасль"
            rules={[{ required: true, message: 'Выберите отрасль' }]}
          >
            <Select placeholder="Выберите отрасль" style={{ width: '100%' }}>
              {otrasls.map(o => (
                <Option key={o.id_Ot} value={o.id_Ot}>
                  {o.name_Ot}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Divider />
          <Typography.Text type="secondary">
            Отрасль будет назначена для {selectedRowKeys.length} выбранных организаций.
          </Typography.Text>
        </Form>
      </Modal>
    </div>
  );
}
