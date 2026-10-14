import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, DatePicker, InputNumber,
  App, Typography,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import dayjs from 'dayjs';
import api from '../../api/axios';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { Option } = Select;

interface PosPutRecord {
  id: number;
  id_San?: number;
  num_Put?: string;
  dataNach?: string;
  dataKon?: string;
  price?: number;
  mes?: string;
  tip?: string;
  status?: string;
  department_BASE?: string;
}

export function PrixodPutevokPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<PosPutRecord | null>(null);

  const { data = [], isLoading, isError, refetch } = useQuery<PosPutRecord[]>({
    queryKey: ['prixod-putevok'],
    queryFn: () => api.get('/api/prixod-putevok').then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<PosPutRecord>) =>
      editing
        ? api.put(`/api/prixod-putevok/${editing.id}`, values).then(r => r.data)
        : api.post('/api/prixod-putevok', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['prixod-putevok'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/prixod-putevok/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['prixod-putevok'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: PosPutRecord) => {
    setEditing(rec);
    form.setFieldsValue({
      ...rec,
      dataNach: rec.dataNach ? dayjs(rec.dataNach) : undefined,
      dataKon: rec.dataKon ? dayjs(rec.dataKon) : undefined,
    });
    setModalOpen(true);
  };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить путевку?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: '№ путевки', dataIndex: 'num_Put' },
    { title: 'Санаторий ID', dataIndex: 'id_San', width: 100 },
    { title: 'Дата начала', dataIndex: 'dataNach' },
    { title: 'Дата конца', dataIndex: 'dataKon' },
    { title: 'Цена', dataIndex: 'price' },
    { title: 'Мес.', dataIndex: 'mes', width: 80 },
    { title: 'Тип', dataIndex: 'tip', width: 80 },
    { title: t('common.status'), dataIndex: 'status' },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: PosPutRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>{t('nav.prixodPutevok')}</Typography.Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
      </div>

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={{ pageSize: 20 }} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate({
          ...v,
          dataNach: v.dataNach?.toISOString(),
          dataKon: v.dataKon?.toISOString(),
        })}>
          <Form.Item name="num_Put" label="№ путевки" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="id_San" label="Санаторий ID">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="dataNach" label="Дата начала">
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="dataKon" label="Дата конца">
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="price" label="Цена">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="mes" label="Мес.">
            <Input />
          </Form.Item>
          <Form.Item name="tip" label="Тип">
            <Input />
          </Form.Item>
          <Form.Item name="status" label={t('common.status')}>
            <Select>
              <Option value="active">Активный</Option>
              <Option value="used">Использован</Option>
              <Option value="cancelled">Отменен</Option>
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
