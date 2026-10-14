import { useState } from 'react';
import { Modal, Input, Table } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import api from '../api/axios';

export interface OrgOption {
  id: number;
  name: string;
  inn?: string;
  branchName?: string;
}

interface Props {
  open: boolean;
  onClose: () => void;
  onSelect: (org: OrgOption) => void;
}

const columns = [
  { title: 'Название', dataIndex: 'name' },
  { title: 'ИНН', dataIndex: 'inn', width: 140 },
  { title: 'Отрасль', dataIndex: 'branchName' },
];

export function OrganizationPickerModal({ open, onClose, onSelect }: Props) {
  const [search, setSearch] = useState('');

  const { data: orgs = [], isFetching } = useQuery<OrgOption[]>({
    queryKey: ['org-picker', search],
    queryFn: () =>
      api.get('/api/organizations', { params: { search } }).then(r => r.data),
    enabled: open,
  });

  const handleSelect = (org: OrgOption) => {
    onSelect(org);
    onClose();
  };

  return (
    <Modal
      title="Выбор организации"
      open={open}
      onCancel={onClose}
      footer={null}
      width={700}
      destroyOnClose
    >
      <Input
        prefix={<SearchOutlined />}
        placeholder="Поиск по названию / ИНН"
        value={search}
        onChange={e => setSearch(e.target.value)}
        allowClear
        style={{ marginBottom: 12 }}
      />
      <Table<OrgOption>
        columns={columns}
        dataSource={orgs}
        rowKey="id"
        size="small"
        loading={isFetching}
        pagination={{ pageSize: 10 }}
        onRow={rec => ({ onClick: () => handleSelect(rec), style: { cursor: 'pointer' } })}
      />
    </Modal>
  );
}
