import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Select, InputNumber, App, Typography,
} from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import api from '../../api/axios';

const { Option } = Select;

interface GodovoyPlanRecord {
  id: number;
  year: number;
  quarter: number;
  sanatoriumId?: number;
  campId?: number;
  regionId?: number;
  price?: number;
  quantity?: number;
  status?: string;
  sanatoriumName?: string;
  regionName?: string;
}

export function GodovoyPlanPage() {
  const { t } = useTranslation();
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<GodovoyPlanRecord | null>(null);
  const [filterYear, setFilterYear] = useState<number>(new Date().getFullYear());

  const { data = [], isLoading } = useQuery<GodovoyPlanRecord[]>({
    queryKey: ['godovoy-plan', filterYear],
    queryFn: () => api.get(`/api/godovoy-plan?year=${filterYear}`).then(r => r.data),
  });

  const saveMutation = useMutation({
    mutationFn: (values: Partial<GodovoyPlanRecord>) =>
      editing
        ? api.put(`/api/godovoy-plan/${editing.id}`, values).then(r => r.data)
        : api.post('/api/godovoy-plan', values).then(r => r.data),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['godovoy-plan'] });
      setModalOpen(false);
      form.resetFields();
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/godovoy-plan/${id}`),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['godovoy-plan'] });
    },
  });

  const openAdd = () => {
    setEditing(null);
    form.resetFields();
    setModalOpen(true);
  };

  const openEdit = (rec: GodovoyPlanRecord) => {
    setEditing(rec);
    form.setFieldsValue(rec);
    setModalOpen(true);
  };

  const confirmDelete = (id: number) => {
    modal.confirm({
      title: 'Удалить запись?',
      onOk: () => deleteMutation.mutate(id),
    });
  };

  const columns = [
    { title: t('godovoyPlan.year'), dataIndex: 'year', width: 80 },
    { title: t('godovoyPlan.quarter'), dataIndex: 'quarter', width: 100 },
    { title: t('godovoyPlan.sanatorium'), dataIndex: 'sanatoriumName' },
    { title: t('godovoyPlan.region'), dataIndex: 'regionName' },
    { title: t('godovoyPlan.price'), dataIndex: 'price' },
    { title: t('godovoyPlan.quantity'), dataIndex: 'quantity' },
    { title: t('common.status'), dataIndex: 'status' },
    {
      title: t('common.actions'),
      width: 100,
      render: (_: unknown, rec: GodovoyPlanRecord) => (
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
        <Typography.Title level={4} style={{ margin: 0 }}>{t('godovoyPlan.title')}</Typography.Title>
        <Space>
          <Select value={filterYear} onChange={setFilterYear} style={{ width: 100 }}>
            {[2022, 2023, 2024, 2025, 2026].map(y => <Option key={y} value={y}>{y}</Option>)}
          </Select>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
            {t('godovoyPlan.addPlan')}
          </Button>
        </Space>
      </div>

      <Table
        columns={columns}
        dataSource={data}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={{ pageSize: 20 }}
      />

      <Modal
        title={editing ? t('godovoyPlan.editPlan') : t('godovoyPlan.addPlan')}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={() => setModalOpen(false)}
        confirmLoading={saveMutation.isPending}
      >
        <Form form={form} layout="vertical" onFinish={v => saveMutation.mutate(v)}>
          <Form.Item name="year" label={t('godovoyPlan.year')} rules={[{ required: true }]}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="quarter" label={t('godovoyPlan.quarter')} rules={[{ required: true }]}>
            <Select>
              {[1, 2, 3, 4].map(q => <Option key={q} value={q}>{q}</Option>)}
            </Select>
          </Form.Item>
          <Form.Item name="sanatoriumId" label={t('godovoyPlan.sanatorium')}>
            <InputNumber style={{ width: '100%' }} placeholder="ID санатория" />
          </Form.Item>
          <Form.Item name="regionId" label={t('godovoyPlan.region')}>
            <InputNumber style={{ width: '100%' }} placeholder="ID региона" />
          </Form.Item>
          <Form.Item name="price" label={t('godovoyPlan.price')}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="quantity" label={t('godovoyPlan.quantity')}>
            <InputNumber style={{ width: '100%' }} />
          </Form.Item>
          <Form.Item name="status" label={t('common.status')}>
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
