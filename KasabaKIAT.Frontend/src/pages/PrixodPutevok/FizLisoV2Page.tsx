import { useState } from 'react';
import {
  Table,
  Input,
  Button,
  Space,
  Typography,
  Breadcrumb,
  Card,
  Row,
  Col,
  App,
} from 'antd';
import { SearchOutlined, ReloadOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import type { ColumnsType, TablePaginationConfig } from 'antd/es/table';
import type { FilterValue, SorterResult } from 'antd/es/table/interface';
import api from '../../api/axios';

const { Title } = Typography;

interface FizLisoRecord {
  id: number;
  oblSovProfName: string;
  name_Org: string;
  inn: string;
  adress: string;
  telefon: string;
  pas_ser: string;
  pas_Num: string;
  pas_KD: string;
  pas_Data: string;
}

interface FizLisoResponse {
  result: FizLisoRecord[];
  count: number;
}

interface TableParams {
  pagination: TablePaginationConfig;
  sortField?: string;
  sortOrder?: string;
  search?: string;
}

export function FizLisoV2Page() {
  App.useApp();

  const [searchText, setSearchText] = useState('');
  const [tableParams, setTableParams] = useState<TableParams>({
    pagination: {
      current: 1,
      pageSize: 10,
      showSizeChanger: true,
      showTotal: (total) => `Всего: ${total}`,
    },
  });

  const fetchData = async (): Promise<FizLisoResponse> => {
    const params: Record<string, unknown> = {
      skip: ((tableParams.pagination.current ?? 1) - 1) * (tableParams.pagination.pageSize ?? 10),
      take: tableParams.pagination.pageSize ?? 10,
    };

    if (tableParams.sortField) {
      params.sortField = tableParams.sortField;
      params.sortOrder = tableParams.sortOrder;
    }

    if (searchText) {
      params.search = searchText;
    }

    const response = await api.post('/PrixodPutevok/GetFizLisoData', params);
    return response.data;
  };

  const { data, isLoading, isError, refetch } = useQuery<FizLisoResponse>({
    queryKey: ['fizlisov2', tableParams, searchText],
    queryFn: fetchData,
  });

  const handleTableChange = (
    pagination: TablePaginationConfig,
    _filters: Record<string, FilterValue | null>,
    sorter: SorterResult<FizLisoRecord> | SorterResult<FizLisoRecord>[]
  ) => {
    const singleSorter = Array.isArray(sorter) ? sorter[0] : sorter;
    setTableParams({
      pagination,
      sortField: singleSorter?.field as string | undefined,
      sortOrder: singleSorter?.order ?? undefined,
    });
  };

  const handleSearch = () => {
    setTableParams((prev) => ({
      ...prev,
      pagination: { ...prev.pagination, current: 1 },
    }));
  };

  const handleReset = () => {
    setSearchText('');
    setTableParams({
      pagination: {
        current: 1,
        pageSize: 10,
        showSizeChanger: true,
        showTotal: (total) => `Всего: ${total}`,
      },
    });
  };

  const columns: ColumnsType<FizLisoRecord> = [
    {
      title: 'Область',
      dataIndex: 'oblSovProfName',
      key: 'oblSovProfName',
      sorter: true,
      ellipsis: true,
      minWidth: 150,
    },
    {
      title: 'ФИО',
      dataIndex: 'name_Org',
      key: 'name_Org',
      sorter: true,
      ellipsis: true,
      minWidth: 200,
    },
    {
      title: 'ИНН',
      dataIndex: 'inn',
      key: 'inn',
      sorter: true,
      ellipsis: true,
      minWidth: 120,
    },
    {
      title: 'Адрес',
      dataIndex: 'adress',
      key: 'adress',
      sorter: true,
      ellipsis: true,
      minWidth: 200,
    },
    {
      title: 'Телефон',
      dataIndex: 'telefon',
      key: 'telefon',
      sorter: true,
      ellipsis: true,
      minWidth: 130,
    },
    {
      title: 'Паспорт Серия',
      dataIndex: 'pas_ser',
      key: 'pas_ser',
      sorter: true,
      ellipsis: true,
      minWidth: 130,
    },
    {
      title: 'Паспорт Номер',
      dataIndex: 'pas_Num',
      key: 'pas_Num',
      sorter: true,
      ellipsis: true,
      minWidth: 140,
    },
    {
      title: 'Кем выдан',
      dataIndex: 'pas_KD',
      key: 'pas_KD',
      sorter: true,
      ellipsis: true,
      minWidth: 180,
    },
    {
      title: 'Дата выдачи',
      dataIndex: 'pas_Data',
      key: 'pas_Data',
      sorter: true,
      ellipsis: true,
      minWidth: 130,
      render: (value: string) => {
        if (!value) return '-';
        try {
          return new Date(value).toLocaleDateString('ru-RU');
        } catch {
          return value;
        }
      },
    },
  ];

  return (
    <div style={{ padding: '24px' }}>
      <Row className="mb-4" align="middle" gutter={[16, 16]}>
        <Col flex="auto">
          <Title level={4} style={{ margin: 0 }}>
            База данных физических лиц
          </Title>
          <Breadcrumb
            style={{ marginTop: '4px' }}
            items={[
              {
                title: 'Справочник физических лиц',
              },
            ]}
          />
        </Col>
      </Row>

      <Card>
        <Space style={{ marginBottom: 16 }} wrap>
          <Input
            placeholder="Поиск..."
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
            onPressEnter={handleSearch}
            prefix={<SearchOutlined />}
            style={{ width: 300 }}
            allowClear
          />
          <Button
            type="primary"
            icon={<SearchOutlined />}
            onClick={handleSearch}
          >
            Поиск
          </Button>
          <Button
            icon={<ReloadOutlined />}
            onClick={handleReset}
          >
            Сбросить
          </Button>
          <Button
            onClick={() => refetch()}
            loading={isLoading}
          >
            Обновить
          </Button>
        </Space>

        {isError && (
          <div style={{ color: 'red', marginBottom: 16 }}>
            Ошибка загрузки данных. Пожалуйста, попробуйте снова.
          </div>
        )}

        <Table<FizLisoRecord>
          columns={columns}
          dataSource={data?.result ?? []}
          rowKey="id"
          loading={isLoading}
          pagination={{
            ...tableParams.pagination,
            total: data?.count ?? 0,
          }}
          onChange={handleTableChange}
          scroll={{ x: 'max-content' }}
          size="middle"
          bordered
        />
      </Card>
    </div>
  );
}
