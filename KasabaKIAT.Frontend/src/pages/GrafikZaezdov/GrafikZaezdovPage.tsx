import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Select, DatePicker, App, Typography, InputNumber,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import dayjs from 'dayjs';
import api from '../../api/axios';

const { Option } = Select;

interface GrafikRecord {
  id: number;
  year?: number;
  sanatoriumId?: number;
  sanatoriumName?: string;
  zaezdDate?: string;
  days?: number;
  quantity?: number;
  quarter?: number;
}

export function GrafikZaezdovPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<GrafikRecord | null>(null);
  const [filterYear, setFilterYear] = useState<number>(new Date().getFullYear());

  const { data = [], isLoading } = useQuery<GrafikRecord[]>({
    queryKey: ['grafik-zaezdov', filterYear],
    queryFn: () => api.get(`/api/grafik-zaezdov?year=${filterYear}`).then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<GrafikRecord>) =>
      editing
        ? api.put(`/api/grafik-zaezdov/${editing.id}`, values).then(r => r.data)
        : api.post('/api/grafik-zaezdov', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['grafik-zaezdov'] });
      setModalOpen(false);
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/grafik-zaezdov/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['grafik-zaezdov'] });
    },
  });

  const openAdd = () => { setEditing(null); form.resetFields(); setModalOpen(true); };
  const openEdit = (rec: GrafikRecord) => {
    setEditing(rec);
    form.setFieldsValue({ ...rec, zaezdDate: rec.zaezdDate ? dayjs(rec.zaezdDate) : undefined });
    setModalOpen(true);
  };
  const confirmDelete = (id: number) =>
    modal.confirm({ title: 'Удалить?', onOk: () => deleteMutation.mutate(id) });

  const columns = [
    { title: t('godovoyPlan.year'), dataIndex: 'year', width: 80 },
    { title: t('godovoyPlan.quarter'), dataIndex: 'quarter', width: 80 },
    { title: t('godovoyPlan.sanatorium'), dataIndex: 'sanatoriumName' },
    { title: 'Дата заезда', dataIndex: 'zaezdDate' },
    { title: 'Дней', dataIndex: 'days', width: 80 },
    { title: t('godovoyPlan.quantity'), dataIndex: 'quantity', width: 100 },
    {
      title: t('common.actions'), width: 100,
      render: (_: unknown, rec: GrafikRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>{t('nav.grafikZaezdov')}</Typography.Title>
        <Space>
          <Select value={filterYear} onChange={setFilterYear} style={{ width: 100 }}>
            {[2023, 2024, 2025, 2026].map(y => <Option key={y} value={y}>{y}</Option>)}
          </Select>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>{t('common.add')}</Button>
        </Space>
      </div>

      <Table columns={columns} dataSource={data} rowKey="id" loading={isLoading} size="small" pagination={{ pageSize: 20 }} />

      <Modal
        title={editing ? t('common.edit') : t('common.add')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate({ ...v, zaezdDate: v.zaezdDate?.toISOString() })}>
          <Form.Item name="year" label={t('godovoyPlan.year')} rules={[{ required: true }]}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="quarter" label={t('godovoyPlan.quarter')}>
            <Select>{[1, 2, 3, 4].map(q => <Option key={q} value={q}>{q}</Option>)}</Select>
          </Form.Item>
          <Form.Item name="sanatoriumId" label={t('godovoyPlan.sanatorium')}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="zaezdDate" label="Дата заезда">
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="days" label="Дней">
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="quantity" label={t('godovoyPlan.quantity')}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
