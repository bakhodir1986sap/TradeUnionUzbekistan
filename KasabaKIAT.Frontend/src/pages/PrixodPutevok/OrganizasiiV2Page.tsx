import { useState } from 'react';
import {
  Table,
  Input,
  Button,
  Space,
  App,
  Typography,
  Breadcrumb,
  Card,
} from 'antd';
import { SearchOutlined, FileExcelOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnType } from 'antd/es/table';
import api from '../../api/axios';

const { Title } = Typography;

interface OrganizasiiV2Item {
  id: number;
  oblSovProfName: string;
  regionName: string;
  name_Org: string;
  inn: string;
  rs: string;
  mfo: string;
  bank: string;
  okonx: string;
  adress: string;
  telefon: string;
  otraslName: string;
}

interface ApiResponse {
  result: OrganizasiiV2Item[];
  count: number;
}

interface TableParams {
  current: number;
  pageSize: number;
  sortField?: string;
  sortOrder?: string;
  search?: string;
}

export function OrganizasiiV2Page() {
  const { message } = App.useApp();
  const [searchText, setSearchText] = useState('');
  const [tableParams, setTableParams] = useState<TableParams>({
    current: 1,
    pageSize: 20,
  });

  const { data, isLoading, isError } = useQuery<ApiResponse>({
    queryKey: ['organizasiiv2', tableParams, searchText],
    queryFn: async () => {
      const response = await api.get('/api/organizasiiv2', {
        params: {
          skip: (tableParams.current - 1) * tableParams.pageSize,
          take: tableParams.pageSize,
          search: searchText || undefined,
          sortField: tableParams.sortField,
          sortOrder: tableParams.sortOrder,
        },
      });
      return response.data;
    },
    onError: () => {
      message.error('Ошибка при загрузке данных организаций');
    },
  } as any);

  const handleExcelExport = async () => {
    try {
      const response = await api.get('/api/organizasiiv2/export', {
        responseType: 'blob',
        params: {
          search: searchText || undefined,
        },
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'organizations.xlsx');
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
      message.success('Экспорт выполнен успешно');
    } catch {
      message.error('Ошибка при экспорте в Excel');
    }
  };

  const columns: ColumnType<OrganizasiiV2Item>[] = [
    {
      title: 'Область',
      dataIndex: 'oblSovProfName',
      key: 'oblSovProfName',
      sorter: true,
      minWidth: 150,
      ellipsis: true,
    },
    {
      title: 'Район',
      dataIndex: 'regionName',
      key: 'regionName',
      sorter: true,
      minWidth: 120,
      ellipsis: true,
    },
    {
      title: 'Название организации',
      dataIndex: 'name_Org',
      key: 'name_Org',
      sorter: true,
      minWidth: 200,
      ellipsis: true,
    },
    {
      title: 'ИНН Профкома',
      dataIndex: 'inn',
      key: 'inn',
      sorter: false,
      minWidth: 130,
      ellipsis: true,
    },
    {
      title: 'Расчетный счет',
      dataIndex: 'rs',
      key: 'rs',
      sorter: true,
      minWidth: 160,
      ellipsis: true,
    },
    {
      title: 'МФО',
      dataIndex: 'mfo',
      key: 'mfo',
      sorter: true,
      minWidth: 80,
      ellipsis: true,
    },
    {
      title: 'Банк',
      dataIndex: 'bank',
      key: 'bank',
      sorter: true,
      minWidth: 150,
      ellipsis: true,
    },
    {
      title: 'ОКЭД',
      dataIndex: 'okonx',
      key: 'okonx',
      sorter: true,
      minWidth: 100,
      ellipsis: true,
    },
    {
      title: 'Адрес',
      dataIndex: 'adress',
      key: 'adress',
      sorter: true,
      width: 180,
      ellipsis: true,
    },
    {
      title: 'Телефон',
      dataIndex: 'telefon',
      key: 'telefon',
      sorter: true,
      width: 140,
      ellipsis: true,
    },
    {
      title: 'Отрасль',
      dataIndex: 'otraslName',
      key: 'otraslName',
      sorter: true,
      minWidth: 150,
      ellipsis: true,
    },
  ];

  const handleTableChange = (
    pagination: { current?: number; pageSize?: number },
    _filters: Record<string, any>,
    sorter: any
  ) => {
    setTableParams({
      current: pagination.current ?? 1,
      pageSize: pagination.pageSize ?? 20,
      sortField: sorter.field,
      sortOrder: sorter.order,
    });
  };

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchText(e.target.value);
    setTableParams((prev) => ({ ...prev, current: 1 }));
  };

  if (isError) {
    return (
      <div style={{ padding: 24 }}>
        <Typography.Text type="danger">
          Ошибка при загрузке данных. Пожалуйста, попробуйте позже.
        </Typography.Text>
      </div>
    );
  }

  return (
    <div style={{ padding: 24 }}>
      <div
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'flex-start',
          marginBottom: 24,
        }}
      >
        <div>
          <Title level={4} style={{ marginBottom: 4 }}>
            База данных организаций
          </Title>
          <Breadcrumb
            items={[{ title: 'Справочник Организаций' }]}
          />
        </div>
      </div>

      <Card>
        <Space style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between', width: '100%' }}>
          <Input
            placeholder="Поиск..."
            prefix={<SearchOutlined />}
            value={searchText}
            onChange={handleSearchChange}
            style={{ width: 300 }}
            allowClear
          />
          <Button
            icon={<FileExcelOutlined />}
            onClick={handleExcelExport}
            style={{ backgroundColor: '#217346', borderColor: '#217346', color: '#fff' }}
          >
            Экспорт в Excel
          </Button>
        </Space>

        <Table<OrganizasiiV2Item>
          columns={columns}
          dataSource={data?.result ?? []}
          rowKey="id"
          loading={isLoading}
          scroll={{ x: 'max-content' }}
          size="small"
          pagination={{
            current: tableParams.current,
            pageSize: tableParams.pageSize,
            total: data?.count ?? 0,
            showSizeChanger: true,
            showTotal: (total, range) =>
              `${range[0]}-${range[1]} из ${total} записей`,
            pageSizeOptions: ['10', '20', '50', '100'],
          }}
          onChange={handleTableChange}
          bordered
        />
      </Card>
    </div>
  );
}
