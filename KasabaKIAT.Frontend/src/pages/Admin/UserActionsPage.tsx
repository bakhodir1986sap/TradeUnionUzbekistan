import { useState } from 'react';
import { Table, Input, DatePicker, Space, Typography } from 'antd';
import { useQuery } from '@tanstack/react-query';
import { useTranslation } from 'react-i18next';
import dayjs from 'dayjs';
import api from '../../api/axios';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

interface UserActionRecord {
  id: number;
  user_id?: number;
  action_name?: string;
  action_date?: string;
  description?: string;
}

export function UserActionsPage() {
  const { t } = useTranslation();
  const [search, setSearch] = useState('');
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs | null, dayjs.Dayjs | null] | null>(null);

  const { data = [], isLoading, isError, refetch } = useQuery<UserActionRecord[]>({
    queryKey: ['user-actions'],
    queryFn: () => api.get('/api/user-actions').then(r => r.data.items),
  });

  const filtered = data.filter(row => {
    const matchText =
      !search ||
      row.action_name?.toLowerCase().includes(search.toLowerCase()) ||
      row.description?.toLowerCase().includes(search.toLowerCase());
    const matchDate =
      !dateRange ||
      !dateRange[0] ||
      !dateRange[1] ||
      (row.action_date &&
        dayjs(row.action_date).isAfter(dateRange[0]) &&
        dayjs(row.action_date).isBefore(dateRange[1].add(1, 'day')));
    return matchText && matchDate;
  });

  const columns = [
    { title: 'ID', dataIndex: 'id', width: 60 },
    { title: 'Пользователь ID', dataIndex: 'user_id', width: 130 },
    { title: 'Действие', dataIndex: 'action_name' },
    { title: 'Дата', dataIndex: 'action_date', width: 160 },
    { title: 'Описание', dataIndex: 'description' },
  ];

  return (
    <div>
      <Typography.Title level={4} style={{ marginBottom: 16 }}>{t('admin.userActionsTitle')}</Typography.Title>

      <Space style={{ marginBottom: 16 }}>
        <Input.Search
          placeholder={t('common.search')}
          value={search}
          onChange={e => setSearch(e.target.value)}
          style={{ width: 280 }}
          allowClear
        />
        <DatePicker.RangePicker
          onChange={v => setDateRange(v as [dayjs.Dayjs | null, dayjs.Dayjs | null])}
        />
      </Space>

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table
        columns={columns}
        dataSource={filtered}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={{ pageSize: 20 }}
      />
    </div>
  );
}
