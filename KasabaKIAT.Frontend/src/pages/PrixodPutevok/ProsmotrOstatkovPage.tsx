import { useState, useRef } from 'react';
import { Button, Table, Space, Input, Select, Typography, App, Row, Col, DatePicker } from 'antd';
import { SearchOutlined, DownloadOutlined, ClearOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import type { ColumnType } from 'antd/es/table';
import type { FilterDropdownProps } from 'antd/es/table/interface';
import type { InputRef } from 'antd';
import dayjs from 'dayjs';
import api from '../../api/axios';

const { Title, Text } = Typography;
const { RangePicker } = DatePicker;

interface OstatokVoucher {
  id: number;
  sanatName: string;
  numPut: string;
  dataZaezda: string;
  price: number;
}

interface OstatokRaw {
  id: number;
  name_San?: string;
  num_Put?: string;
  dataNach?: string;
  price?: number;
}

interface Sanatorium {
  id: number;
  name: string;
}

interface Filters {
  sanatId: number | null;
  numPut: string;
  dateFrom: string;
  dateTo: string;
  price: string;
}

const defaultFilters: Filters = {
  sanatId: null,
  numPut: '',
  dateFrom: '',
  dateTo: '',
  price: '',
};

export function ProsmotrOstatkovPage() {
  const { message } = App.useApp();
  const [filters, setFilters] = useState<Filters>(defaultFilters);
  const [appliedFilters, setAppliedFilters] = useState<Filters>(defaultFilters);
  const numPutInputRef = useRef<InputRef>(null);
  const priceInputRef = useRef<InputRef>(null);

  const { data: sanatoriums = [] } = useQuery<Sanatorium[]>({
    queryKey: ['sanatoriums'],
    queryFn: async () => {
      const res = await api.get('/api/sanatoriums');
      return res.data;
    },
  });

  const { data: vouchers = [], isFetching, isError, refetch } = useQuery<OstatokVoucher[]>({
    queryKey: ['prosmotr-ostatkov', appliedFilters],
    queryFn: async () => {
      const params: Record<string, string> = {};
      if (appliedFilters.sanatId) params.sanatId = String(appliedFilters.sanatId);
      if (appliedFilters.numPut) params.numPut = appliedFilters.numPut;
      if (appliedFilters.dateFrom) params.dateFrom = appliedFilters.dateFrom;
      if (appliedFilters.dateTo) params.dateTo = appliedFilters.dateTo;
      if (appliedFilters.price) params.price = appliedFilters.price;
      const res = await api.get('/api/prosmotr-ostatkov', { params });
      const raw: OstatokRaw[] = res.data;
      return raw.map(r => ({
        id: r.id,
        sanatName: r.name_San ?? '',
        numPut: r.num_Put ?? '',
        dataZaezda: r.dataNach ?? '',
        price: r.price ?? 0,
      }));
    },
  });

  const handleApplyFilters = () => {
    setAppliedFilters({ ...filters });
  };

  const handleClearFilters = () => {
    setFilters(defaultFilters);
    setAppliedFilters(defaultFilters);
  };

  const handleExport = async () => {
    try {
      const res = await api.get('/api/prosmotr-ostatkov/export', {
        responseType: 'blob',
      });
      const url = window.URL.createObjectURL(new Blob([res.data]));
      const link = document.createElement('a');
      link.href = url;
      const contentDisposition = res.headers['content-disposition'];
      let fileName = 'prosmotr-ostatkov.xlsx';
      if (contentDisposition) {
        const match = contentDisposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/);
        if (match && match[1]) {
          fileName = match[1].replace(/['"]/g, '');
        }
      }
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка при экспорте данных');
    }
  };

  const getTextFilterDropdown = (
    filterKey: 'numPut' | 'price',
    inputRef: React.RefObject<InputRef | null>
  ) =>
    ({ confirm, clearFilters, close }: FilterDropdownProps) => (
      <div style={{ padding: 8 }}>
        <Input
          ref={inputRef}
          placeholder="Поиск..."
          value={filters[filterKey]}
          onChange={(e) => setFilters((prev) => ({ ...prev, [filterKey]: e.target.value }))}
          onPressEnter={() => {
            setAppliedFilters((prev) => ({ ...prev, [filterKey]: filters[filterKey] }));
            confirm();
          }}
          style={{ marginBottom: 8, display: 'block' }}
        />
        <Space>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            size="small"
            onClick={() => {
              setAppliedFilters((prev) => ({ ...prev, [filterKey]: filters[filterKey] }));
              confirm();
            }}
          >
            Найти
          </Button>
          <Button
            size="small"
            onClick={() => {
              setFilters((prev) => ({ ...prev, [filterKey]: '' }));
              setAppliedFilters((prev) => ({ ...prev, [filterKey]: '' }));
              if (clearFilters) clearFilters();
            }}
          >
            Сбросить
          </Button>
          <Button size="small" type="link" onClick={close}>
            Закрыть
          </Button>
        </Space>
      </div>
    );

  const getSanatoriumFilterDropdown =
    () =>
    ({ confirm, clearFilters, close }: FilterDropdownProps) => (
      <div style={{ padding: 8, minWidth: 220 }}>
        <Select
          style={{ width: '100%', marginBottom: 8 }}
          placeholder="Выберите санаторий"
          allowClear
          value={filters.sanatId}
          onChange={(val) => setFilters((prev) => ({ ...prev, sanatId: val ?? null }))}
          options={sanatoriums.map((s) => ({ label: s.name, value: s.id }))}
        />
        <Space>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            size="small"
            onClick={() => {
              setAppliedFilters((prev) => ({ ...prev, sanatId: filters.sanatId }));
              confirm();
            }}
          >
            Найти
          </Button>
          <Button
            size="small"
            onClick={() => {
              setFilters((prev) => ({ ...prev, sanatId: null }));
              setAppliedFilters((prev) => ({ ...prev, sanatId: null }));
              if (clearFilters) clearFilters();
            }}
          >
            Сбросить
          </Button>
          <Button size="small" type="link" onClick={close}>
            Закрыть
          </Button>
        </Space>
      </div>
    );

  const getDateFilterDropdown =
    () =>
    ({ confirm, clearFilters, close }: FilterDropdownProps) => (
      <div style={{ padding: 8 }}>
        <RangePicker
          style={{ marginBottom: 8, display: 'block' }}
          value={
            filters.dateFrom && filters.dateTo
              ? [dayjs(filters.dateFrom), dayjs(filters.dateTo)]
              : null
          }
          onChange={(dates) => {
            if (dates && dates[0] && dates[1]) {
              setFilters((prev) => ({
                ...prev,
                dateFrom: dates[0]!.format('YYYY-MM-DD'),
                dateTo: dates[1]!.format('YYYY-MM-DD'),
              }));
            } else {
              setFilters((prev) => ({ ...prev, dateFrom: '', dateTo: '' }));
            }
          }}
          format="DD.MM.YYYY"
        />
        <Space>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            size="small"
            onClick={() => {
              setAppliedFilters((prev) => ({
                ...prev,
                dateFrom: filters.dateFrom,
                dateTo: filters.dateTo,
              }));
              confirm();
            }}
          >
            Найти
          </Button>
          <Button
            size="small"
            onClick={() => {
              setFilters((prev) => ({ ...prev, dateFrom: '', dateTo: '' }));
              setAppliedFilters((prev) => ({ ...prev, dateFrom: '', dateTo: '' }));
              if (clearFilters) clearFilters();
            }}
          >
            Сбросить
          </Button>
          <Button size="small" type="link" onClick={close}>
            Закрыть
          </Button>
        </Space>
      </div>
    );

  const columns: ColumnType<OstatokVoucher>[] = [
    {
      title: '№',
      key: 'index',
      width: 60,
      render: (_: unknown, __: OstatokVoucher, index: number) => index + 1,
    },
    {
      title: 'Санаторий',
      dataIndex: 'sanatName',
      key: 'sanatName',
      filterDropdown: getSanatoriumFilterDropdown(),
      filterIcon: (filtered: boolean) => (
        <SearchOutlined style={{ color: filtered ? '#1677ff' : undefined }} />
      ),
      filteredValue: appliedFilters.sanatId ? [String(appliedFilters.sanatId)] : null,
    },
    {
      title: 'Номера путевок',
      dataIndex: 'numPut',
      key: 'numPut',
      filterDropdown: getTextFilterDropdown('numPut', numPutInputRef),
      filterIcon: (filtered: boolean) => (
        <SearchOutlined style={{ color: filtered ? '#1677ff' : undefined }} />
      ),
      filteredValue: appliedFilters.numPut ? [appliedFilters.numPut] : null,
    },
    {
      title: 'Дата заезда',
      dataIndex: 'dataZaezda',
      key: 'dataZaezda',
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
      filterDropdown: getDateFilterDropdown(),
      filterIcon: (filtered: boolean) => (
        <SearchOutlined style={{ color: filtered ? '#1677ff' : undefined }} />
      ),
      filteredValue:
        appliedFilters.dateFrom && appliedFilters.dateTo
          ? [appliedFilters.dateFrom, appliedFilters.dateTo]
          : null,
    },
    {
      title: 'Цена',
      dataIndex: 'price',
      key: 'price',
      align: 'right',
      render: (val: number) =>
        val != null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
      filterDropdown: getTextFilterDropdown('price', priceInputRef),
      filterIcon: (filtered: boolean) => (
        <SearchOutlined style={{ color: filtered ? '#1677ff' : undefined }} />
      ),
      filteredValue: appliedFilters.price ? [appliedFilters.price] : null,
    },
  ];

  const totalCount = vouchers.length;
  const totalSum = vouchers.reduce((acc, v) => acc + (v.price || 0), 0);

  const hasActiveFilters =
    appliedFilters.sanatId !== null ||
    appliedFilters.numPut !== '' ||
    appliedFilters.dateFrom !== '' ||
    appliedFilters.dateTo !== '' ||
    appliedFilters.price !== '';

  return (
    <div style={{ padding: 24 }}>
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Просмотр остатков путевок
          </Title>
        </Col>
        <Col>
          <Space>
            {hasActiveFilters && (
            <Button
              icon={<ClearOutlined />}
              onClick={handleClearFilters}
              disabled={isFetching}
            >
              Сбросить фильтры
            </Button>
            )}
            <Button
              type="primary"
              icon={<DownloadOutlined />}
              onClick={handleExport}
              disabled={isFetching}
            >
              Экспорт
            </Button>
          </Space>
        </Col>
      </Row>

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table<OstatokVoucher>
        rowKey="id"
        columns={columns}
        dataSource={vouchers}
        loading={isFetching}
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: (total) => `Всего: ${total}` }}
        bordered
        size="small"
        scroll={{ x: 'max-content' }}
        onChange={handleApplyFilters}
        footer={() => (
          <Row justify="end" gutter={32}>
            <Col>
              <Text strong>Количество: </Text>
              <Text>{totalCount}</Text>
            </Col>
            <Col>
              <Text strong>Сумма: </Text>
              <Text>
                {totalSum.toLocaleString('ru-RU', {
                  minimumFractionDigits: 2,
                  maximumFractionDigits: 2,
                })}
              </Text>
            </Col>
          </Row>
        )}
      />
    </div>
  );
}
