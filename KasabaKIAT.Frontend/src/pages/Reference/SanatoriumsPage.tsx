import { App, Form, Input } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { referenceApi } from '../../api/reference';
import { CrudTable } from '../../components/CrudTable';

interface SanRecord {
  id: number;
  name_San?: string;
  num_Kod?: string;
  kurortSanID?: number | null;
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
    onError: () => message.error(t('common.error')),
  });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 70 },
    { title: 'Наименование', dataIndex: 'name_San' },
    { title: 'Код', dataIndex: 'num_Kod', width: 120 },
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
          <Form.Item name="name_San" label="Наименование" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="num_Kod" label="Код">
            <Input />
          </Form.Item>
        </>
      }
    />
  );
}
