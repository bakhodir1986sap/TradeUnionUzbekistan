import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, App, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

interface SanRecord {
  id: number;
  name?: string;
  address?: string;
  capacity?: number;
  regionName?: string;
  typeName?: string;
}

export function SanatoriumsPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<SanRecord | null>(null);

  const { data = [], isLoading } = useQuery<SanRecord[]>({
    queryKey: ['sanatoriums'],
    queryFn: () => api.get('/api/reference/sanatoriums').then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<SanRecord>) =>
      editing
        ? api.put(`/api/reference/sanatoriums/${editing.id}`, values).then(r => r.data)
        : api.post('/api/reference/sanatoriums', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['sanatoriums'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/reference/sanatoriums/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['sanatoriums'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: SanRecord) => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить санаторий?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Наименование', dataIndex: 'name' },
    { title: 'Регион', dataIndex: 'regionName' },
    { title: 'Тип', dataIndex: 'typeName' },
    { title: 'Вместимость', dataIndex: 'capacity', width: 120 },
    { title: 'Адрес', dataIndex: 'address' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: SanRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>Санатории</Typography.Title>
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
          <Form.Item name="name" label="Наименование" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="address" label="Адрес">
            <Input />
          </Form.Item>
          <Form.Item name="capacity" label="Вместимость">
            <Input type="number" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
