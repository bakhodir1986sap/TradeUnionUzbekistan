import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, DatePicker, InputNumber, App, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined, PrinterOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import dayjs from 'dayjs';
import api from '../../api/axios';

interface PlatejkaRecord {
  rec_ID: number;
  plat_DATE?: string;
  plat_NUM?: string;
  platelNAME?: string;
  platelINN?: string;
  poluchNAME?: string;
  poluchINN?: string;
  summa?: number;
  comment?: string;
}

export function PlatejnoyePorucheniyePage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<PlatejkaRecord | null>(null);

  const { data = [], isLoading } = useQuery<PlatejkaRecord[]>({
    queryKey: ['platejki'],
    queryFn: () => api.get('/api/platejnoe-poruchenie').then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<PlatejkaRecord>) =>
      editing
        ? api.put(`/api/platejnoe-poruchenie/${editing.rec_ID}`, values).then(r => r.data)
        : api.post('/api/platejnoe-poruchenie', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['platejki'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/platejnoe-poruchenie/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['platejki'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: PlatejkaRecord) => {
    setEditing(rec);
    form.setFieldsValue({ ...rec, plat_DATE: rec.plat_DATE ? dayjs(rec.plat_DATE) : undefined });
    setModalOpen(true);
  };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить платежное поручение?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: '№', dataIndex: 'plat_NUM', width: 100 },
    { title: 'Дата', dataIndex: 'plat_DATE' },
    { title: 'Плательщик', dataIndex: 'platelNAME' },
    { title: 'ИНН плат.', dataIndex: 'platelINN', width: 120 },
    { title: 'Получатель', dataIndex: 'poluchNAME' },
    { title: 'ИНН получ.', dataIndex: 'poluchINN', width: 120 },
    { title: 'Сумма', dataIndex: 'summa', width: 120 },
    {
      title: t('common.actions'), width: 120,
      render: (_: unknown, rec: PlatejkaRecord) => (
        <Space>
          <Button size="small" icon={<PrinterOutlined />} onClick={() => window.print()} />
          <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
          <Button size="small" danger icon={<DeleteOutlined />} onClick={() => confirmDelete(rec.rec_ID)} />
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>{t('nav.platejnoe')}</Typography.Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
      </div>

      <Table columns={columns} dataSource={data} rowKey="rec_ID" loading={isLoading} size="small" pagination={{ pageSize: 20 }} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={600}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate({ ...v, plat_DATE: v.plat_DATE?.toISOString() })}>
          <Form.Item name="plat_NUM" label="Номер" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="plat_DATE" label="Дата">
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="platelNAME" label="Плательщик" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="platelINN" label="ИНН плательщика">
            <Input />
          </Form.Item>
          <Form.Item name="platelRASCHET" label="Р/с плательщика">
            <Input />
          </Form.Item>
          <Form.Item name="platelBANKNAME" label="Банк плательщика">
            <Input />
          </Form.Item>
          <Form.Item name="platelBANKMFO" label="МФО банка плательщика">
            <Input />
          </Form.Item>
          <Form.Item name="poluchNAME" label="Получатель" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="poluchINN" label="ИНН получателя">
            <Input />
          </Form.Item>
          <Form.Item name="poluchRASCHET" label="Р/с получателя">
            <Input />
          </Form.Item>
          <Form.Item name="poluchBANKNAME" label="Банк получателя">
            <Input />
          </Form.Item>
          <Form.Item name="poluchBANKMFO" label="МФО банка получателя">
            <Input />
          </Form.Item>
          <Form.Item name="summa" label="Сумма" rules={[{ required: true }]}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="comment" label="Назначение платежа">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
