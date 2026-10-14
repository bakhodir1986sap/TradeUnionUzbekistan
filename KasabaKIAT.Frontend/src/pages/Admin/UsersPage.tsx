import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, App, Typography,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

interface UserRecord {
  id: number;
  userName?: string;
  email?: string;
  fullName?: string;
  userType?: string;
  organizationName?: string;
  createdAt?: string;
  roleId?: number;
}

export function UsersPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<UserRecord | null>(null);

  const { data = [], isLoading } = useQuery<UserRecord[]>({
    queryKey: ['users'],
    queryFn: () => api.get('/api/users').then(r => r.data),
  });

  const { data: roles = [] } = useQuery<{ id: number; name: string }[]>({
    queryKey: ['roles'],
    queryFn: () => api.get('/api/roles').then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<UserRecord> & { password?: string }) =>
      editing
        ? api.put(`/api/users/${editing.id}`, values).then(r => r.data)
        : api.post('/api/users', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['users'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/users/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: UserRecord) => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить пользователя?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: t('admin.loginField'), dataIndex: 'userName' },
    { title: t('admin.fullName'), dataIndex: 'fullName' },
    { title: 'Email', dataIndex: 'email' },
    { title: t('admin.organization'), dataIndex: 'organizationName' },
    { title: t('admin.userType'), dataIndex: 'userType' },
    { title: t('admin.regDate'), dataIndex: 'createdAt' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: UserRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>{t('admin.usersTitle')}</Typography.Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
      </div>

      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={{ pageSize: 20 }} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="userName" label={t('admin.loginField')} rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          {!editing && (
            <Form.Item name="password" label={t('admin.password')} rules={[{ required: true }]}>
              <Input.Password />
            </Form.Item>
          )}
          <Form.Item name="fullName" label={t('admin.fullName')}>
            <Input />
          </Form.Item>
          <Form.Item name="email" label="Email">
            <Input />
          </Form.Item>
          <Form.Item name="roleId" label="Роль">
            <Select>
              {roles.map(r => (
                <Select.Option key={r.id} value={r.id}>{r.name}</Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="userType" label={t('admin.userType')}>
            <Select>
              <Select.Option value="admin">Администратор</Select.Option>
              <Select.Option value="operator">Оператор</Select.Option>
              <Select.Option value="user">Пользователь</Select.Option>
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
