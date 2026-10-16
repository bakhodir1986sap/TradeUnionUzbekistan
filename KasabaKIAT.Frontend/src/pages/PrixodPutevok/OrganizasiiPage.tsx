import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select,
  App, Typography, Row, Col, Divider, Upload, Tooltip,
} from 'antd';
import type { ColumnsType } from 'antd/es/table';
import {
  PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined,
  FileExcelOutlined, UploadOutlined, BranchesOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import { organizationsApi, type OrgItem, type OtraslOption } from '../../api/organizations';

const { Option } = Select;

interface ActiveFilter {
  name: string;
  inn: string;
  otraslId: number | undefined;
  page: number;
  pageSize: number;
}

export function OrganizasiiPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [changeOtraslForm] = Form.useForm();

  const [modalOpen, setModalOpen] = useState(false);
  const [changeOtraslModalOpen, setChangeOtraslModalOpen] = useState(false);
  const [editing, setEditing] = useState<OrgItem | null>(null);

  const [filterName, setFilterName] = useState('');
  const [filterInn, setFilterInn] = useState('');
  const [filterOtrasl, setFilterOtrasl] = useState<number | undefined>(undefined);

  const pageSize = 20;
  const [curPage, setCurPage] = useState(1);
  const [activeFilter, setActiveFilter] = useState<ActiveFilter>({
    name: '', inn: '', otraslId: undefined, page: 1, pageSize,
  });

  const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
  const [selectedOtraslFile, setSelectedOtraslFile] = useState<File | null>(null);

  /* ─── Queries ─────────────────────────────────────────────────────────────── */

  const { data: otrasls = [] } = useQuery<OtraslOption[]>({
    queryKey: ['otrasls-organizations'],
    queryFn: () => organizationsApi.getOtrasls(),
  });

  const { data: filterResult, isLoading, isError, refetch } = useQuery({
    queryKey: ['organizasii-filter', activeFilter],
    queryFn: () =>
      organizationsApi.getList({
        name: activeFilter.name || undefined,
        inn: activeFilter.inn || undefined,
        otraslId: activeFilter.otraslId ?? undefined,
        page: activeFilter.page,
        pageSize: activeFilter.pageSize,
      }),
  });

  const organizations: OrgItem[] = filterResult?.items ?? [];
  const totalCount = filterResult?.count ?? 0;
  const totalCountNeot = filterResult?.totalCountNeot ?? 0;
  const totalCountOpr = filterResult?.totalCountOpr ?? 0;

  /* ─── Mutations ───────────────────────────────────────────────────────────── */

  const saveMutation = useMutation({
    mutationFn: (values: Partial<OrgItem>) =>
      editing
        ? organizationsApi.update(editing.id, values)
        : organizationsApi.create(values),
    onSuccess: () => {
      message.success('Сохранено успешно');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
      setModalOpen(false);
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => organizationsApi.delete(id),
    onSuccess: () => {
      message.success('Организация удалена');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
    },
    onError: () => message.error('Ошибка при удалении'),
  });

  const changeOtraslMutation = useMutation({
    mutationFn: ({ ids, otraslId }: { ids: number[]; otraslId: number }) =>
      organizationsApi.changeOtraslMassive(ids, otraslId),
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
      organizationsApi.export({
        name: activeFilter.name || undefined,
        inn: activeFilter.inn || undefined,
        otraslId: activeFilter.otraslId ?? undefined,
      }),
    onSuccess: (blob: Blob) => {
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'Organizasii.csv';
      a.click();
      URL.revokeObjectURL(url);
    },
    onError: () => message.error('Ошибка при экспорте'),
  });

  const uploadOtraslMutation = useMutation({
    mutationFn: (file: File) => organizationsApi.uploadOtrasl(file),
    onSuccess: () => {
      message.success('Отраслевка загружена успешно');
      queryClient.invalidateQueries({ queryKey: ['organizasii-filter'] });
      setSelectedOtraslFile(null);
    },
    onError: () => message.error('Ошибка при загрузке файла'),
  });

  /* ─── Handlers ────────────────────────────────────────────────────────────── */

  const handleSearch = () => {
    setCurPage(1);
    setActiveFilter({ name: filterName, inn: filterInn, otraslId: filterOtrasl, page: 1, pageSize });
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

  const openEdit = (rec: OrgItem) => {
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
      ids: selectedRowKeys as number[],
      otraslId: values.id_Ot,
    });
  };

  /* ─── Table columns ───────────────────────────────────────────────────────── */

  const columns: ColumnsType<OrgItem> = [
    { title: 'ID', dataIndex: 'id', width: 70, align: 'center' },
    {
      title: 'Отрасль',
      dataIndex: 'id_Ot',
      width: 160,
      align: 'center',
      render: (v: number | null | undefined) => {
        const found = otrasls.find(o => o.id === v);
        return found ? found.name_Ot : <span style={{ color: '#bbb' }}>—</span>;
      },
    },
    { title: 'Название организации', dataIndex: 'name_Org', width: 200, align: 'center' },
    { title: 'ИНН', dataIndex: 'inn', width: 120, align: 'center' },
    { title: 'Р/С', dataIndex: 'rs', width: 160, align: 'center' },
    { title: 'МФО', dataIndex: 'mfo', width: 80, align: 'center' },
    { title: 'Банк', dataIndex: 'bank', width: 140, align: 'center' },
    { title: 'Адрес', dataIndex: 'adress', width: 180, align: 'center' },
    { title: 'Телефон', dataIndex: 'telefon', width: 130, align: 'center' },
    { title: 'Email', dataIndex: 'e_Mail', width: 160, align: 'center' },
    { title: 'Директор', dataIndex: 'director', width: 160, align: 'center' },
    { title: 'Бухгалтер', dataIndex: 'buxgalter', width: 160, align: 'center' },
    {
      title: 'Действия',
      key: 'actions',
      width: 90,
      align: 'center',
      fixed: 'right' as const,
      render: (_: unknown, rec: OrgItem) => (
        <Space>
          <Tooltip title="Редактировать">
            <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          </Tooltip>
          <Tooltip title="Удалить">
            <Button size="small" danger icon={<DeleteOutlined />} onClick={() => confirmDelete(rec.id)} />
          </Tooltip>
        </Space>
      ),
    },
  ];

  /* ─── Render ──────────────────────────────────────────────────────────────── */

  return (
    <div>
      <div style={{ marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>База данных организаций</Typography.Title>
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
          <Button icon={<BranchesOutlined />} onClick={openChangeOtrasl} style={{ background: '#1677ff', color: '#fff', border: 'none' }}>
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
                <Option key={o.id} value={o.id}>{o.name_Ot}</Option>
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
            <div style={{ marginBottom: 4 }}>Не определённых</div>
            <Input value={totalCountNeot} readOnly />
          </div>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <div>
            <div style={{ marginBottom: 4 }}>Определённых</div>
            <Input value={totalCountOpr} readOnly />
          </div>
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

      {/* File upload for otrasl assignment */}
      <Row gutter={[16, 8]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={8}>
          <Upload
            accept=".csv"
            fileList={selectedOtraslFile ? [{ uid: '1', name: selectedOtraslFile.name, status: 'done' }] : []}
            beforeUpload={file => { setSelectedOtraslFile(file); return false; }}
            onRemove={() => setSelectedOtraslFile(null)}
            maxCount={1}
          >
            <Button icon={<UploadOutlined />}>Выбрать файл отраслевки (CSV)</Button>
          </Upload>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Button
            type="primary"
            onClick={() => selectedOtraslFile && uploadOtraslMutation.mutate(selectedOtraslFile)}
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
              <Button type="primary" disabled={curPage === 1} onClick={handlePrev}>
                &lt; Предыдущий
              </Button>
              <span>Страница {curPage}</span>
              <Button type="primary" disabled={curPage >= Math.ceil(totalCount / pageSize)} onClick={handleNext}>
                Следующий &gt;
              </Button>
            </Space>
          </Col>
        </Row>
      )}

      {/* Table */}
      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table
        columns={columns}
        dataSource={organizations}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={false}
        scroll={{ x: 1800 }}
        rowSelection={{ selectedRowKeys, onChange: keys => setSelectedRowKeys(keys) }}
        style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.09)' }}
      />

      {/* Add/Edit Modal */}
      <Modal
        title={editing ? 'Редактировать организацию' : 'Добавить организацию'}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={720}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="name_Org" label="Название организации" rules={[{ required: true, message: 'Введите название' }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="inn" label="ИНН" rules={[{ required: true, message: 'Введите ИНН' }]}>
                <Input maxLength={9} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="id_Ot" label="Отрасль">
                <Select placeholder="Выберите отрасль" allowClear>
                  {otrasls.map(o => (
                    <Option key={o.id} value={o.id}>{o.name_Ot}</Option>
                  ))}
                </Select>
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="rs" label="Расчетный счет">
                <Input maxLength={20} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="mfo" label="МФО">
                <Input maxLength={5} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="bank" label="Банк">
                <Input maxLength={90} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="adress" label="Адрес">
                <Input maxLength={100} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="telefon" label="Телефон">
                <Input maxLength={70} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="e_Mail" label="Email">
                <Input maxLength={90} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="okonx" label="ОКЭД">
                <Input maxLength={5} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="director" label="Директор">
                <Input maxLength={200} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="buxgalter" label="Бухгалтер">
                <Input maxLength={200} />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="departmentBase" label="Код отдела">
                <Input maxLength={255} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="owner_CODE" label="Код владельца">
                <Input maxLength={255} />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* Change Otrasl Modal */}
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
          <Form.Item name="id_Ot" label="Отрасль" rules={[{ required: true, message: 'Выберите отрасль' }]}>
            <Select placeholder="Выберите отрасль" style={{ width: '100%' }}>
              {otrasls.map(o => (
                <Option key={o.id} value={o.id}>{o.name_Ot}</Option>
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
