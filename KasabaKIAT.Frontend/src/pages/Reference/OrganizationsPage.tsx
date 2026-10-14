import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, App, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SearchOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

interface OrgRecord {
  id: number;
  name?: string;
  inn?: string;
  address?: string;
  branchName?: string;
  regionName?: string;
}

export function OrganizationsPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<OrgRecord | null>(null);
  const [search, setSearch] = useState('');

  const { data = [], isLoading } = useQuery<OrgRecord[]>({
    queryKey: ['organizations'],
    queryFn: () => api.get('/api/organizations').then(r => r.data),
  });

  const filtered = data.filter(r =>
    !search ||
    r.name?.toLowerCase().includes(search.toLowerCase()) ||
    r.inn?.includes(search)
  );

  const saveMutation = useMutation({
    mutationFn: (values: Partial<OrgRecord>) =>
      editing
        ? api.put(`/api/organizations/${editing.id}`, values).then(r => r.data)
        : api.post('/api/organizations', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/organizations/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: OrgRecord) => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить организацию?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Наименование', dataIndex: 'name' },
    { title: 'ИНН', dataIndex: 'inn', width: 140 },
    { title: 'Отрасль', dataIndex: 'branchName' },
    { title: 'Регион', dataIndex: 'regionName' },
    { title: 'Адрес', dataIndex: 'address' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: OrgRecord) => (
        <Space>
          <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          <Button size="small" danger icon={<DeleteOutlined />} onClick={() => confirmDelete(rec.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>{t('nav.organizacii')}</Typography.Title>
        <Space>
          <Input.Search
            placeholder="Поиск по названию или ИНН"
            value={search}
            onChange={e => setSearch(e.target.value)}
            style={{ width: 260 }}
            allowClear
            prefix={<SearchOutlined />}
          />
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={filtered}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={{ pageSize: 20 }}
      />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={500}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="name" label="Наименование" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="inn" label="ИНН">
            <Input />
          </Form.Item>
          <Form.Item name="address" label="Адрес">
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
