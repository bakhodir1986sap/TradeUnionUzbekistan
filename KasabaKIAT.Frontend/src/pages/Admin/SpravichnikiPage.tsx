import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, App, Typography, Tabs } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

interface RefRecord {
  id: number;
  code?: string;
  nameRu?: string;
  nameUz?: string;
  nameUzl?: string;
}

function ReferenceTab({ endpoint }: { endpoint: string; label: string }) {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<RefRecord | null>(null);

  const { data = [], isLoading } = useQuery<RefRecord[]>({
    queryKey: ['ref', endpoint],
    queryFn: () => api.get(`/api/reference/${endpoint}`).then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<RefRecord>) =>
      editing
        ? api.put(`/api/reference/${endpoint}/${editing.id}`, values).then(r => r.data)
        : api.post(`/api/reference/${endpoint}`, values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['ref', endpoint] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/reference/${endpoint}/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['ref', endpoint] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: RefRecord) => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить запись?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: t('common.code'), dataIndex: 'code', width: 120 },
    { title: t('common.nameRu'), dataIndex: 'nameRu' },
    { title: t('common.nameUz'), dataIndex: 'nameUz' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: RefRecord) => (
        <Space>
          <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          <Button size="small" danger icon={<DeleteOutlined />} onClick={() => confirmDelete(rec.id)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'flex-end', marginBottom: 12 }}>
        <Button type="primary" size="small" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
      </div>
      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={{ pageSize: 15 }} />
      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="code" label={t('common.code')}>
            <Input />
          </Form.Item>
          <Form.Item name="nameRu" label={t('common.nameRu')} rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="nameUz" label={t('common.nameUz')}>
            <Input />
          </Form.Item>
          <Form.Item name="nameUzl" label={t('common.nameUzl')}>
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}

export function SpravichnikiPage() {
  const { t } = useTranslation();

  const tabItems = [
    { key: 'regions', label: t('reference.regions'), children: <ReferenceTab endpoint="regions" label={t('reference.regions')} /> },
    { key: 'branches', label: t('reference.branches'), children: <ReferenceTab endpoint="branches" label={t('reference.branches')} /> },
    { key: 'countries', label: t('reference.countries'), children: <ReferenceTab endpoint="countries" label={t('reference.countries')} /> },
    { key: 'positions', label: t('reference.positions'), children: <ReferenceTab endpoint="positions" label={t('reference.positions')} /> },
    { key: 'camps', label: t('reference.camps'), children: <ReferenceTab endpoint="camps" label={t('reference.camps')} /> },
    { key: 'nations', label: 'Национальности', children: <ReferenceTab endpoint="nations" label="Национальности" /> },
    { key: 'degrees', label: 'Степени', children: <ReferenceTab endpoint="degrees" label="Степени" /> },
    { key: 'sections', label: 'Секции', children: <ReferenceTab endpoint="sections" label="Секции" /> },
  ];

  return (
    <div>
      <Typography.Title level={4} style={{ marginBottom: 16 }}>{t('reference.title')}</Typography.Title>
      <Tabs items={tabItems} tabPosition="left" style={{ minHeight: 400 }} />
    </div>
  );
}
