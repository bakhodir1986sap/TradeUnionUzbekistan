import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, InputNumber, Tabs, App, Typography, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SendOutlined, CheckOutlined, CloseOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { campZayavkiApi } from '../../api/campZayavki';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

interface ZayavkaRecord {
  id: number;
  organizationName?: string;
  applicantName?: string;
  sanatoriumId?: number;
  status?: string;
  createdAt?: string;
  quantity?: number;
}

const STATUS_COLORS: Record<string, string> = {
  new: 'blue',
  sent: 'orange',
  accepted: 'green',
  rejected: 'red',
};

export function CampZayavkiPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ZayavkaRecord | null>(null);
  const [activeTab, setActiveTab] = useState('new');

  const { data = [], isLoading, isError, refetch } = useQuery<ZayavkaRecord[]>({
    queryKey: ['camp-zayavki', activeTab],
    queryFn: () => campZayavkiApi.getAll({ status: activeTab }),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<ZayavkaRecord>) =>
      editing
        ? campZayavkiApi.update(editing.id, values)
        : campZayavkiApi.create(values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['camp-zayavki'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const transitionMutation = useMutation({
    mutationFn: ({ id, action }: { id: number; action: string }) => {
      if (action === 'send') return campZayavkiApi.send(id);
      if (action === 'accept') return campZayavkiApi.accept(id);
      if (action === 'reject') return campZayavkiApi.reject(id, '');
      return Promise.reject(new Error(`Unknown action: ${action}`));
    },
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['camp-zayavki'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => campZayavkiApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['camp-zayavki'] });
    },
  });

  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить заявку?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: 'Организация', dataIndex: 'organizationName' },
    { title: 'Заявитель', dataIndex: 'applicantName' },
    { title: 'Санаторий ID', dataIndex: 'sanatoriumId', width: 110 },
    { title: 'Кол-во', dataIndex: 'quantity', width: 80 },
    { title: 'Дата', dataIndex: 'createdAt' },
    {
      title: t('common.status'), dataIndex: 'status', width: 100,
      render: (s: string) => <Tag color={STATUS_COLORS[s] ?? 'default'}>{s}</Tag>,
    },
    {
      title: t('common.actions'), width: 180,
      render: (_: unknown, rec: ZayavkaRecord) => (
        <Space>
          {rec.status === 'new' && (
            <Button size="small" icon={<SendOutlined />} onClick={() => transitionMutation.mutate({ id: rec.id, action: 'send' })}>
              Отправить
            </Button>
          )}
          {rec.status === 'sent' && (
            <>
              <Button size="small" icon={<CheckOutlined />} type="primary" onClick={() => transitionMutation.mutate({ id: rec.id, action: 'accept' })}>
                Принять
              </Button>
              <Button size="small" icon={<CloseOutlined />} danger onClick={() => transitionMutation.mutate({ id: rec.id, action: 'reject' })}>
                Откл.
              </Button>
            </>
          )}
          <Button size="small" icon={<EditOutlined />} onClick={() => { setEditing(rec); form.setFieldsValue(rec); setModalOpen(true); }} />
          <Button size="small" danger icon={<DeleteOutlined />} onClick={() => confirmDelete(rec.id)} />
        </Space>
      ),
    },
  ];

  const tabItems = [
    { key: 'new', label: 'Новые' },
    { key: 'sent', label: 'Отправленные' },
    { key: 'accepted', label: 'Принятые' },
    { key: 'rejected', label: 'Отклоненные' },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>{t('nav.campZayavki')}</Typography.Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => { setEditing(null); form.resetFields(); setModalOpen(true); }}>
          {t('common.add')}
        </Button>
      </div>

      <Tabs activeKey={activeTab} onChange={setActiveTab} items={tabItems} />

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={{ pageSize: 20 }} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="organizationName" label="Организация" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="applicantName" label="Заявитель">
            <Input />
          </Form.Item>
          <Form.Item name="sanatoriumId" label="Санаторий ID">
            <Input />
          </Form.Item>
          <Form.Item name="quantity" label="Количество">
            <InputNumber style={{ width: '100%' }} min={1} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
