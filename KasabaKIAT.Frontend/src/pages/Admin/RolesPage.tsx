import { App, Form, Input } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { rolesApi } from '../../api/roles';
import { CrudTable } from '../../components/CrudTable';

interface RoleRecord {
  id: number;
  name?: string;
  description?: string;
}

export function RolesPage() {
  const { t } = useTranslation();
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const { data = [], isLoading, isError, refetch } = useQuery<RoleRecord[]>({
    queryKey: ['roles'],
    queryFn: () => rolesApi.getAll(),
  });

  const saveMutation = useMutation({
    mutationFn: (payload: { values: Partial<RoleRecord>; editing: RoleRecord | null }) =>
      payload.editing
        ? rolesApi.update(payload.editing.id, payload.values)
        : rolesApi.create(payload.values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => rolesApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['roles'] });
    },
  });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Название', dataIndex: 'name' },
    { title: 'Описание', dataIndex: 'description' },
  ];

  return (
    <CrudTable<RoleRecord>
      title={t('admin.rolesTitle')}
      columns={columns}
      data={data}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
      isSaving={saveMutation.isPending}
      isDeleting={deleteMutation.isPending}
      onSave={(values, editing) =>
        saveMutation.mutateAsync({ values: values as Partial<RoleRecord>, editing })
      }
      onDelete={(id) => deleteMutation.mutate(id)}
      deleteConfirmTitle="Удалить роль?"
      formFields={
        <>
          <Form.Item name="name" label="Название" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="description" label="Описание">
            <Input.TextArea rows={3} />
          </Form.Item>
        </>
      }
    />
  );
}
