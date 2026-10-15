import { useState } from 'react';
import { Button, Table, Space, Modal, Form, InputNumber, Tabs, App, Typography, Tag, Select } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, SendOutlined, CheckOutlined, CloseOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { campZayavkiApi } from '../../api/campZayavki';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

interface ZayavkaRecord {
  id: number;
  campId?: number;
  campName?: string;
  serialBegin?: string;
  serialEnd?: string;
  restBeginDate?: string;
  restEndDate?: string;
  price?: number;
  quantityIn?: number;
  quantityRemain?: number;
  status?: string;
  modifyDate?: string;
}

interface CampOption {
  id: number;
  name: string;
}

const STATUS_COLORS: Record<string, string> = {
  '1': 'blue',
  '2': 'orange',
  '3': 'green',
  '4': 'red',
};

const STATUS_LABELS: Record<string, string> = {
  '1': 'Новая',
  '2': 'Отправлена',
  '3': 'Принята',
  '4': 'Отклонена',
};

const TAB_STATUS: Record<string, string> = {
  new: '1',
  sent: '2',
  accepted: '3',
  rejected: '4',
};

export function CampZayavkiPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<ZayavkaRecord | null>(null);
  const [activeTab, setActiveTab] = useState('new');

  const { data: camps = [] } = useQuery<CampOption[]>({
    queryKey: ['reference-camps'],
    queryFn: () => campZayavkiApi.getCamps(),
  });

  const { data = [], isLoading, isError, refetch } = useQuery<ZayavkaRecord[]>({
    queryKey: ['camp-zayavki', activeTab],
    queryFn: () => campZayavkiApi.getAll({ status: TAB_STATUS[activeTab] }),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
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
    mutationFn: ({ id, status }: { id: number; status: string }) =>
      campZayavkiApi.updateStatus(id, status),
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
    { title: 'Лагерь', dataIndex: 'campName' },
    { title: 'Серия (нач.)', dataIndex: 'serialBegin' },
    { title: 'Серия (кон.)', dataIndex: 'serialEnd' },
    { title: 'Кол-во', dataIndex: 'quantityIn', width: 80 },
    { title: 'Остаток', dataIndex: 'quantityRemain', width: 80 },
    { title: 'Цена', dataIndex: 'price', width: 100 },
    {
      title: t('common.status'), dataIndex: 'status', width: 110,
      render: (s: string) => <Tag color={STATUS_COLORS[s] ?? 'default'}>{STATUS_LABELS[s] ?? s}</Tag>,
    },
    {
      title: t('common.actions'), width: 200,
      render: (_: unknown, rec: ZayavkaRecord) => (
        <Space>
          {rec.status === '1' && (
            <Button size="small" icon={<SendOutlined />} onClick={() => transitionMutation.mutate({ id: rec.id, status: '2' })}>
              Отправить
            </Button>
          )}
          {rec.status === '2' && (
            <>
              <Button size="small" icon={<CheckOutlined />} type="primary" onClick={() => transitionMutation.mutate({ id: rec.id, status: '3' })}>
                Принять
              </Button>
              <Button size="small" icon={<CloseOutlined />} danger onClick={() => transitionMutation.mutate({ id: rec.id, status: '4' })}>
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
          <Form.Item name="campId" label="Лагерь" rules={[{ required: true }]}>
            <Select
              options={camps.map(c => ({ value: c.id, label: c.name }))}
              placeholder="Выберите лагерь"
            />
          </Form.Item>
          <Form.Item name="serialBegin" label="Серия начало">
            <input className="ant-input" />
          </Form.Item>
          <Form.Item name="serialEnd" label="Серия конец">
            <input className="ant-input" />
          </Form.Item>
          <Form.Item name="quantity" label="Количество">
            <InputNumber style={{ width: '100%' }} min={1} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
