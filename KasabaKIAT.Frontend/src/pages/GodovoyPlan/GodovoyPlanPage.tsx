import { App, Form, Input, InputNumber, Select } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { useState } from 'react';
import { godovoyPlanApi } from '../../api/godovoyPlan';
import { CrudTable } from '../../components/CrudTable';

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
  const { message } = App.useApp();
  const queryClient = useQueryClient();
  const [filterYear, setFilterYear] = useState<number>(new Date().getFullYear());

  const { data = [], isLoading, isError, refetch } = useQuery<GodovoyPlanRecord[]>({
    queryKey: ['godovoy-plan', filterYear],
    queryFn: () => godovoyPlanApi.getAll({ year: filterYear }),
  });

  const saveMutation = useMutation({
    mutationFn: (payload: { values: Partial<GodovoyPlanRecord>; editing: GodovoyPlanRecord | null }) =>
      payload.editing
        ? godovoyPlanApi.update(payload.editing.id, payload.values)
        : godovoyPlanApi.create(payload.values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['godovoy-plan'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => godovoyPlanApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['godovoy-plan'] });
    },
  });

  const columns = [
    { title: t('godovoyPlan.year'), dataIndex: 'year', width: 80 },
    { title: t('godovoyPlan.quarter'), dataIndex: 'quarter', width: 100 },
    { title: t('godovoyPlan.sanatorium'), dataIndex: 'sanatoriumName' },
    { title: t('godovoyPlan.region'), dataIndex: 'regionName' },
    { title: t('godovoyPlan.price'), dataIndex: 'price' },
    { title: t('godovoyPlan.quantity'), dataIndex: 'quantity' },
    { title: t('common.status'), dataIndex: 'status' },
  ];

  return (
    <CrudTable<GodovoyPlanRecord>
      title={t('godovoyPlan.title')}
      columns={columns}
      data={data}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
      isSaving={saveMutation.isPending}
      isDeleting={deleteMutation.isPending}
      onSave={(values, editing) =>
        saveMutation.mutateAsync({ values: values as Partial<GodovoyPlanRecord>, editing })
      }
      onDelete={(id) => deleteMutation.mutate(id)}
      deleteConfirmTitle="Удалить запись?"
      extraHeaderContent={
        <Select value={filterYear} onChange={setFilterYear} style={{ width: 100 }}>
          {Array.from({ length: 10 }, (_, i) => new Date().getFullYear() - 2 + i).map(y => (
            <Option key={y} value={y}>{y}</Option>
          ))}
        </Select>
      }
      formFields={
        <>
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
        </>
      }
    />
  );
}
