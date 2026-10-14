import { App, Form, Input, InputNumber } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { referenceApi } from '../../api/reference';
import { CrudTable } from '../../components/CrudTable';

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
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const { data = [], isLoading, isError, refetch } = useQuery<SanRecord[]>({
    queryKey: ['sanatoriums'],
    queryFn: () => referenceApi.getSanatoriums(),
  });

  const saveMutation = useMutation({
    mutationFn: (payload: { values: Partial<SanRecord>; editing: SanRecord | null }) =>
      payload.editing
        ? referenceApi.updateSanatorium(payload.editing.id, payload.values)
        : referenceApi.createSanatorium(payload.values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['sanatoriums'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => referenceApi.deleteSanatorium(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['sanatoriums'] });
    },
  });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Наименование', dataIndex: 'name' },
    { title: 'Регион', dataIndex: 'regionName' },
    { title: 'Тип', dataIndex: 'typeName' },
    { title: 'Вместимость', dataIndex: 'capacity', width: 120 },
    { title: 'Адрес', dataIndex: 'address' },
  ];

  return (
    <CrudTable<SanRecord>
      title="Санатории"
      columns={columns}
      data={data}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
      isSaving={saveMutation.isPending}
      isDeleting={deleteMutation.isPending}
      onSave={(values, editing) =>
        saveMutation.mutateAsync({ values: values as Partial<SanRecord>, editing })
      }
      onDelete={(id) => deleteMutation.mutate(id)}
      deleteConfirmTitle="Удалить санаторий?"
      formFields={
        <>
          <Form.Item name="name" label="Наименование" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="address" label="Адрес">
            <Input />
          </Form.Item>
          <Form.Item name="capacity" label="Вместимость">
            <InputNumber style={{ width: '100%' }} min={0} />
          </Form.Item>
        </>
      }
    />
  );
}
