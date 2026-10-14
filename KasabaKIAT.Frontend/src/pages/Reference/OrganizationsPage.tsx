import { useState } from 'react';
import { App, Form, Input } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { organizationsApi } from '../../api/organizations';
import { CrudTable } from '../../components/CrudTable';

interface OrgRecord {
  id: number;
  name?: string;
  inn?: string;
  address?: string;
  branchName?: string;
  regionName?: string;
}

export function OrganizationsPage() {
  const { t } = useTranslation();
  const { message } = App.useApp();
  const queryClient = useQueryClient();
  const [search, setSearch] = useState('');

  const { data = [], isLoading, isError, refetch } = useQuery<OrgRecord[]>({
    queryKey: ['organizations'],
    queryFn: () => organizationsApi.getAll(),
  });

  const filtered = data.filter(r =>
    !search ||
    r.name?.toLowerCase().includes(search.toLowerCase()) ||
    r.inn?.includes(search)
  );

  const saveMutation = useMutation({
    mutationFn: (payload: { values: Partial<OrgRecord>; editing: OrgRecord | null }) =>
      payload.editing
        ? organizationsApi.update(payload.editing.id, payload.values)
        : organizationsApi.create(payload.values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => organizationsApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['organizations'] });
    },
  });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Наименование', dataIndex: 'name' },
    { title: 'ИНН', dataIndex: 'inn', width: 140 },
    { title: 'Отрасль', dataIndex: 'branchName' },
    { title: 'Регион', dataIndex: 'regionName' },
    { title: 'Адрес', dataIndex: 'address' },
  ];

  return (
    <CrudTable<OrgRecord>
      title={t('nav.organizacii')}
      columns={columns}
      data={filtered}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
      isSaving={saveMutation.isPending}
      isDeleting={deleteMutation.isPending}
      onSave={(values, editing) =>
        saveMutation.mutateAsync({ values: values as Partial<OrgRecord>, editing })
      }
      onDelete={(id) => deleteMutation.mutate(id)}
      deleteConfirmTitle="Удалить организацию?"
      modalWidth={500}
      extraHeaderContent={
        <Input.Search
          placeholder="Поиск по названию или ИНН"
          value={search}
          onChange={e => setSearch(e.target.value)}
          style={{ width: 260 }}
          allowClear
          prefix={<SearchOutlined />}
        />
      }
      formFields={
        <>
          <Form.Item name="name" label="Наименование" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="inn" label="ИНН">
            <Input />
          </Form.Item>
          <Form.Item name="address" label="Адрес">
            <Input />
          </Form.Item>
        </>
      }
    />
  );
}
