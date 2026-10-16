import { useState } from 'react';
import {
  App, Button, Form, Input, Modal, Row, Col, Select, Space, Table, Typography,
} from 'antd';
import type { ColumnType } from 'antd/es/table';
import { PlusOutlined, BranchesOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { Resizable } from 'react-resizable';
import type { ResizeCallbackData } from 'react-resizable';
import 'react-resizable/css/styles.css';
import { organizationsApi, type OrgItem } from '../../api/organizations';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const ResizableTitle = (
  props: React.HTMLAttributes<HTMLElement> & {
    onResize?: (e: React.SyntheticEvent, data: ResizeCallbackData) => void;
    width?: number;
  }
) => {
  const { onResize, width, ...restProps } = props;
  if (!width || !onResize) return <th {...restProps} />;
  return (
    <Resizable
      width={width}
      height={0}
      handle={
        <span
          className="react-resizable-handle"
          onClick={e => e.stopPropagation()}
          style={{
            position: 'absolute', right: -5, bottom: 0,
            zIndex: 1, width: 10, height: '100%', cursor: 'col-resize',
          }}
        />
      }
      onResize={onResize}
      draggableOpts={{ enableUserSelectHack: false }}
    >
      <th {...restProps} style={{ ...restProps.style, position: 'relative' }} />
    </Resizable>
  );
};

const PAGE_SIZE = 50;

interface FilterState {
  name: string;
  inn: string;
  otraslId: number | null;
  page: number;
}

const defaultFilter: FilterState = { name: '', inn: '', otraslId: null, page: 1 };

export function OrganizationsPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [assignForm] = Form.useForm();

  const [pending, setPending] = useState<Omit<FilterState, 'page'>>(defaultFilter);
  const [activeFilter, setActiveFilter] = useState<FilterState>(defaultFilter);
  const [selectedRowKeys, setSelectedRowKeys] = useState<number[]>([]);
  const [addEditOpen, setAddEditOpen] = useState(false);
  const [editingRecord, setEditingRecord] = useState<OrgItem | null>(null);
  const [assignOpen, setAssignOpen] = useState(false);

  const { data: otrasls = [] } = useQuery({
    queryKey: ['org-otrasls'],
    queryFn: () => organizationsApi.getOtrasls(),
  });

  const { data: regions = [] } = useQuery({
    queryKey: ['org-regions'],
    queryFn: () => organizationsApi.getRegions(),
  });

  const otraslMap = Object.fromEntries(otrasls.map(o => [o.id, o.name_Ot]));
  const regionMap = Object.fromEntries(regions.map(r => [r.id, r.name_Reg]));

  const { data, isLoading, isError, refetch } = useQuery({
    queryKey: ['organizations-list', activeFilter],
    queryFn: () => organizationsApi.getList({
      name: activeFilter.name || undefined,
      inn: activeFilter.inn || undefined,
      otraslId: activeFilter.otraslId ?? undefined,
      page: activeFilter.page,
      pageSize: PAGE_SIZE,
    }),
  });

  const items: OrgItem[] = data?.items ?? [];
  const totalCount: number = data?.count ?? 0;

  const saveMutation = useMutation({
    mutationFn: (values: Partial<OrgItem>) =>
      editingRecord
        ? organizationsApi.update(editingRecord.id, values)
        : organizationsApi.create(values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations-list'] });
      setAddEditOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => organizationsApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations-list'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const changeOtraslMutation = useMutation({
    mutationFn: ({ ids, otraslId }: { ids: number[]; otraslId: number }) =>
      organizationsApi.changeOtraslMassive(ids, otraslId),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations-list'] });
      setAssignOpen(false);
      setSelectedRowKeys([]);
      assignForm.resetFields();
    },
    onError: () => message.error(t('common.error')),
  });

  const openAdd = () => {
    setEditingRecord(null);
    form.resetFields();
    setAddEditOpen(true);
  };

  const openEdit = (rec: OrgItem) => {
    setEditingRecord(rec);
    form.setFieldsValue({
      name_Org: rec.name_Org,
      inn: rec.inn,
      id_Ot: rec.id_Ot,
      soato_ID: rec.soato_ID,
      address: rec.address,
      name_Short: rec.name_Short,
      name_Full: rec.name_Full,
      departmentBase: rec.departmentBase,
      owner_CODE: rec.owner_CODE,
    });
    setAddEditOpen(true);
  };

  const handleDelete = (id: number) => {
    modal.confirm({
      title: 'Удалить организацию?',
      onOk: () => deleteMutation.mutate(id),
    });
  };

  const handleSearch = () => {
    setActiveFilter({ ...pending, page: 1 });
    setSelectedRowKeys([]);
  };

  const handleAssignOtrasl = () => {
    if (selectedRowKeys.length === 0) {
      message.warning('Выберите организации для назначения отрасли');
      return;
    }
    assignForm.resetFields();
    setAssignOpen(true);
  };

  const [colWidths, setColWidths] = useState<Record<string, number>>({
    id: 70, name_Org: 220, inn: 140, name_Short: 160,
    id_Ot: 180, soato_ID: 150, address: 200, _actions: 100,
  });

  const handleResize = (key: string) => (_: React.SyntheticEvent, { size }: ResizeCallbackData) => {
    setColWidths(prev => ({ ...prev, [key]: Math.max(50, size.width) }));
  };

  const columns: ColumnType<OrgItem>[] = [
    { title: 'ID', dataIndex: 'id', width: colWidths.id, onHeaderCell: () => ({ width: colWidths.id, onResize: handleResize('id') } as React.HTMLAttributes<HTMLElement>) },
    { title: 'Наименование', dataIndex: 'name_Org', ellipsis: true, width: colWidths.name_Org, onHeaderCell: () => ({ width: colWidths.name_Org, onResize: handleResize('name_Org') } as React.HTMLAttributes<HTMLElement>) },
    { title: 'ИНН', dataIndex: 'inn', width: colWidths.inn, onHeaderCell: () => ({ width: colWidths.inn, onResize: handleResize('inn') } as React.HTMLAttributes<HTMLElement>) },
    { title: 'Краткое название', dataIndex: 'name_Short', ellipsis: true, width: colWidths.name_Short, onHeaderCell: () => ({ width: colWidths.name_Short, onResize: handleResize('name_Short') } as React.HTMLAttributes<HTMLElement>) },
    {
      title: 'Отрасль',
      dataIndex: 'id_Ot',
      ellipsis: true,
      width: colWidths.id_Ot,
      onHeaderCell: () => ({ width: colWidths.id_Ot, onResize: handleResize('id_Ot') } as React.HTMLAttributes<HTMLElement>),
      render: (id: number | null) => (id ? (otraslMap[id] ?? id) : '—'),
    },
    {
      title: 'Регион',
      dataIndex: 'soato_ID',
      ellipsis: true,
      width: colWidths.soato_ID,
      onHeaderCell: () => ({ width: colWidths.soato_ID, onResize: handleResize('soato_ID') } as React.HTMLAttributes<HTMLElement>),
      render: (code: string | null) => {
        if (!code) return '—';
        const region = regions.find(r => String(r.id) === String(code) || r.code === code);
        return region ? region.name_Reg : code;
      },
    },
    { title: 'Адрес', dataIndex: 'address', ellipsis: true, width: colWidths.address, onHeaderCell: () => ({ width: colWidths.address, onResize: handleResize('address') } as React.HTMLAttributes<HTMLElement>) },
    {
      title: 'Действия',
      key: '_actions',
      width: colWidths._actions,
      fixed: 'right' as const,
      render: (_: unknown, rec: OrgItem) => (
        <Space>
          <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          <Button
            size="small"
            danger
            icon={<DeleteOutlined />}
            loading={deleteMutation.isPending}
            onClick={() => handleDelete(rec.id)}
          />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>
          {t('nav.organizacii')}
        </Typography.Title>
        <Space>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
            Добавить
          </Button>
          <Button icon={<BranchesOutlined />} onClick={handleAssignOtrasl}>
            Назначить отрасль
          </Button>
        </Space>
      </div>

      {/* Filters */}
      <Row gutter={8} style={{ marginBottom: 12 }} align="middle" wrap>
        <Col>
          <Input
            placeholder="Наименование"
            value={pending.name}
            onChange={e => setPending(p => ({ ...p, name: e.target.value }))}
            onPressEnter={handleSearch}
            allowClear
            style={{ width: 220 }}
          />
        </Col>
        <Col>
          <Input
            placeholder="ИНН"
            value={pending.inn}
            onChange={e => setPending(p => ({ ...p, inn: e.target.value }))}
            onPressEnter={handleSearch}
            allowClear
            style={{ width: 160 }}
          />
        </Col>
        <Col>
          <Select
            placeholder="Отрасль"
            value={pending.otraslId}
            onChange={v => setPending(p => ({ ...p, otraslId: v }))}
            allowClear
            style={{ width: 220 }}
            options={otrasls.map(o => ({ value: o.id, label: o.name_Ot }))}
            showSearch
            filterOption={(input, option) =>
              String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
          />
        </Col>
        <Col>
          <Button type="primary" onClick={handleSearch}>
            Поиск
          </Button>
        </Col>
        {totalCount > 0 && (
          <Col>
            <Typography.Text type="secondary">Всего: {totalCount}</Typography.Text>
          </Col>
        )}
      </Row>

      <QueryErrorAlert isError={isError} onRetry={refetch} />

      <Table<OrgItem>
        columns={columns}
        dataSource={items}
        rowKey="id"
        loading={isLoading}
        size="small"
        scroll={{ x: 1200 }}
        components={{ header: { cell: ResizableTitle } }}
        rowSelection={{
          type: 'checkbox',
          selectedRowKeys,
          onChange: keys => setSelectedRowKeys(keys as number[]),
        }}
        pagination={{
          current: activeFilter.page,
          pageSize: PAGE_SIZE,
          total: totalCount,
          showSizeChanger: false,
          showTotal: total => `Всего ${total}`,
          onChange: page => setActiveFilter(f => ({ ...f, page })),
        }}
      />

      {/* Add / Edit Modal */}
      <Modal
        title={editingRecord ? 'Редактировать организацию' : 'Добавить организацию'}
        open={addEditOpen}
        onOk={() => form.submit()}
        onCancel={() => setAddEditOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={680}
        okText="Сохранить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form
          form={form}
          layout="vertical"
          onFinish={values => saveMutation.mutate(values as Partial<OrgItem>)}
        >
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="name_Org" label="Наименование" rules={[{ required: true }]}>
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="inn" label="ИНН">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="name_Short" label="Краткое название">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="name_Full" label="Полное наименование">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="id_Ot" label="Отрасль">
                <Select
                  allowClear
                  showSearch
                  placeholder="Выберите отрасль"
                  options={otrasls.map(o => ({ value: o.id, label: o.name_Ot }))}
                  filterOption={(input, option) =>
                    String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="soato_ID" label="Регион">
                <Select
                  allowClear
                  showSearch
                  placeholder="Выберите регион"
                  options={regions.map(r => ({ value: String(r.id), label: r.name_Reg }))}
                  filterOption={(input, option) =>
                    String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
                  }
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="address" label="Адрес">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="departmentBase" label="База отдела">
                <Input />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="owner_CODE" label="Код владельца">
                <Input />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* Assign Otrasl Modal */}
      <Modal
        title="Назначить отрасль"
        open={assignOpen}
        onOk={() => assignForm.submit()}
        onCancel={() => setAssignOpen(false)}
        confirmLoading={changeOtraslMutation.isPending}
        width={400}
        okText="Назначить"
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form
          form={assignForm}
          layout="vertical"
          onFinish={values =>
            changeOtraslMutation.mutate({ ids: selectedRowKeys, otraslId: values.otraslId })
          }
        >
          <Typography.Text style={{ display: 'block', marginBottom: 12 }}>
            Отрасль будет назначена для {selectedRowKeys.length} выбранных организаций
          </Typography.Text>
          <Form.Item name="otraslId" label="Отрасль" rules={[{ required: true }]}>
            <Select
              showSearch
              placeholder="Выберите отрасль"
              options={otrasls.map(o => ({ value: o.id, label: o.name_Ot }))}
              filterOption={(input, option) =>
                String(option?.label ?? '').toLowerCase().includes(input.toLowerCase())
              }
            />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
