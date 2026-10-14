import { App, Form, Input, Select } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import { usersApi } from '../../api/users';
import { rolesApi } from '../../api/roles';
import { CrudTable } from '../../components/CrudTable';

interface UserRecord {
  id: number;
  userName?: string;
  email?: string;
  fullName?: string;
  userType?: string;
  organizationName?: string;
  createdAt?: string;
  roleId?: number;
}

export function UsersPage() {
  const { t } = useTranslation();
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const { data, isLoading, isError, refetch } = useQuery<{ items: UserRecord[]; total: number }>({
    queryKey: ['users'],
    queryFn: () => usersApi.getAll(),
  });
  const users = data?.items ?? [];

  const { data: roles = [] } = useQuery<{ id: number; name: string }[]>({
    queryKey: ['roles'],
    queryFn: () => rolesApi.getAll(),
  });

  const saveMutation = useMutation({
    mutationFn: (payload: { values: Partial<UserRecord> & { password?: string }; editing: UserRecord | null }) =>
      payload.editing
        ? usersApi.update(payload.editing.id, payload.values)
        : usersApi.create(payload.values),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
    onError: () => message.error(t('common.error')),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => usersApi.delete(id),
    onSuccess: () => {
      message.success(t('common.success'));
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
  });

  const columns = [
    { title: t('admin.loginField'), dataIndex: 'userName' },
    { title: t('admin.fullName'), dataIndex: 'fullName' },
    { title: 'Email', dataIndex: 'email' },
    { title: t('admin.organization'), dataIndex: 'organizationName' },
    { title: t('admin.userType'), dataIndex: 'userType' },
    { title: t('admin.regDate'), dataIndex: 'createdAt' },
  ];

  return (
    <CrudTable<UserRecord>
      title={t('admin.usersTitle')}
      columns={columns}
      data={users}
      isLoading={isLoading}
      isError={isError}
      onRetry={refetch}
      isSaving={saveMutation.isPending}
      isDeleting={deleteMutation.isPending}
      onSave={(values, editing) =>
        saveMutation.mutateAsync({
          values: values as Partial<UserRecord> & { password?: string },
          editing,
        })
      }
      onDelete={(id) => deleteMutation.mutate(id)}
      deleteConfirmTitle="Удалить пользователя?"
      formFields={
        <>
          <Form.Item name="userName" label={t('admin.loginField')} rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="password" label={t('admin.password')}>
            <Input.Password placeholder="Оставьте пустым, чтобы не менять" />
          </Form.Item>
          <Form.Item name="fullName" label={t('admin.fullName')}>
            <Input />
          </Form.Item>
          <Form.Item name="email" label="Email">
            <Input />
          </Form.Item>
          <Form.Item name="roleId" label="Роль">
            <Select>
              {roles.map(r => (
                <Select.Option key={r.id} value={r.id}>{r.name}</Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="userType" label={t('admin.userType')}>
            <Select>
              <Select.Option value="admin">Администратор</Select.Option>
              <Select.Option value="operator">Оператор</Select.Option>
              <Select.Option value="user">Пользователь</Select.Option>
            </Select>
          </Form.Item>
        </>
      }
    />
  );
}
