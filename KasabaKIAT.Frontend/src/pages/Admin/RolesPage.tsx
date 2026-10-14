import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, App, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

interface RoleRecord {
  id: number;
  name?: string;
  description?: string;
}

export function RolesPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<RoleRecord | null>(null);

  const { data = [], isLoading } = useQuery<RoleRecord[]>({
    queryKey: ['roles'],
    queryFn: () => api.get('/api/roles').then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<RoleRecord>) =>
      editing
        ? api.put(`/api/roles/${editing.id}`, values).then(r => r.data)
        : api.post('/api/roles', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['roles'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/roles/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: RoleRecord) => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить роль?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Название', dataIndex: 'name' },
    { title: 'Описание', dataIndex: 'description' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: RoleRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>{t('admin.rolesTitle')}</Typography.Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
      </div>

      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={false} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="name" label="Название" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Описание">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
