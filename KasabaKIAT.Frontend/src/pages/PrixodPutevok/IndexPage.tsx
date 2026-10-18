import { useState } from 'react';
import { Button, Table, Space, Modal, DatePicker, Select, Typography, App } from 'antd';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../api/axios';
import dayjs from 'dayjs';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';

const { RangePicker } = DatePicker;
const { Title } = Typography;

interface KuInvoice {
  id: number;
  numDoc: string;
  dataDoc: string;
  tipVidachi: string;
  dogovor: string;
  zaKvartal: string;
  dovNum: string;
  dovDate: string;
  komVidan: string;
  kolPutevok: number;
  summa: number;
  vid: boolean;
}

interface KuDetail {
  id: number;
  sanat: string;
  numPut: string;
  tip: string;
  dataNach: string;
  dataKon: string;
  stoimost: number;
  summa: number;
  status: string;
}

interface ZbOption {
  id: number;
  name: string;
}

export function IndexPage() {
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const [zbId, setZbId] = useState<number | undefined>(undefined);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs | null, dayjs.Dayjs | null] | null>(null);
  const [selectedInvoice, setSelectedInvoice] = useState<KuInvoice | null>(null);
  const [detailModalOpen, setDetailModalOpen] = useState(false);

  const dateFrom = dateRange?.[0]?.format('YYYY-MM-DD');
  const dateTo = dateRange?.[1]?.format('YYYY-MM-DD');

  const { data: zbOptions = [], isLoading: zbLoading } = useQuery<ZbOption[]>({
    queryKey: ['zb-options'],
    queryFn: async () => {
      const res = await api.get('/api/spravochnik/profsoyuz');
      return res.data;
    },
  });

  const { data: invoices = [], isLoading: invoicesLoading, isError: invoicesError, refetch: refetchInvoices } = useQuery<KuInvoice[]>({
    queryKey: ['prixod-putevok-ku', zbId, dateFrom, dateTo],
    queryFn: async () => {
      const params: Record<string, string | number> = {};
      if (zbId !== undefined) params.zbId = zbId;
      if (dateFrom) params.dateFrom = dateFrom;
      if (dateTo) params.dateTo = dateTo;
      const res = await api.get('/api/prixod-putevok/ku', { params });
      return res.data;
    },
    enabled: true,
  });

  const { data: details = [], isLoading: detailsLoading } = useQuery<KuDetail[]>({
    queryKey: ['prixod-putevok-ku-details', selectedInvoice?.id],
    queryFn: async () => {
      const res = await api.get(`/api/prixod-putevok/ku/${selectedInvoice!.id}/details`);
      return res.data.details;
    },
    enabled: detailModalOpen && selectedInvoice !== null,
  });

  const oprixodovatMutation = useMutation({
    mutationFn: async (id: number) => {
      const res = await api.post(`/api/prixod-putevok/ku/${id}/oprixodovat`);
      return res.data;
    },
    onSuccess: () => {
      message.success('Документ успешно оприходован');
      queryClient.invalidateQueries({ queryKey: ['prixod-putevok-ku'] });
    },
    onError: () => {
      message.error('Ошибка при оприходовании документа');
    },
  });

  const handleExport = async () => {
    try {
      const params: Record<string, string | number> = {};
      if (zbId !== undefined) params.zbId = zbId;
      if (dateFrom) params.dateFrom = dateFrom;
      if (dateTo) params.dateTo = dateTo;
      const res = await api.get('/api/prixod-putevok/ku/export', {
        params,
        responseType: 'blob',
      });
      const url = window.URL.createObjectURL(new Blob([res.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'prixod_putevok_ku.xlsx');
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка при экспорте');
    }
  };

  const handleRowClick = (record: KuInvoice) => {
    setSelectedInvoice(record);
    setDetailModalOpen(true);
  };

  const handleOprixodovat = () => {
    if (!selectedInvoice) {
      message.warning('Выберите документ');
      return;
    }
    oprixodovatMutation.mutate(selectedInvoice.id);
  };

  const masterColumns = [
    {
      title: 'Номер документа',
      dataIndex: 'numDoc',
      key: 'numDoc',
    },
    {
      title: 'Дата документа',
      dataIndex: 'dataDoc',
      key: 'dataDoc',
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Тип выдачи',
      dataIndex: 'tipVidachi',
      key: 'tipVidachi',
    },
    {
      title: 'Договор',
      dataIndex: 'dogovor',
      key: 'dogovor',
    },
    {
      title: 'За квартал',
      dataIndex: 'zaKvartal',
      key: 'zaKvartal',
    },
    {
      title: 'Дов.№',
      dataIndex: 'dovNum',
      key: 'dovNum',
    },
    {
      title: 'Дата Дов.',
      dataIndex: 'dovDate',
      key: 'dovDate',
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Кому выдан дов',
      dataIndex: 'komVidan',
      key: 'komVidan',
    },
    {
      title: 'Кол-во путевок',
      dataIndex: 'kolPutevok',
      key: 'kolPutevok',
    },
    {
      title: 'Сумма',
      dataIndex: 'summa',
      key: 'summa',
      render: (val: number) =>
        val !== undefined && val !== null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
  ];

  const detailColumns = [
    {
      title: 'Санаторий',
      dataIndex: 'sanat',
      key: 'sanat',
    },
    {
      title: 'Номер путевки',
      dataIndex: 'numPut',
      key: 'numPut',
    },
    {
      title: 'Тип',
      dataIndex: 'tip',
      key: 'tip',
    },
    {
      title: 'Дата начала',
      dataIndex: 'dataNach',
      key: 'dataNach',
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Дата конца',
      dataIndex: 'dataKon',
      key: 'dataKon',
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Стоимость',
      dataIndex: 'stoimost',
      key: 'stoimost',
      render: (val: number) =>
        val !== undefined && val !== null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
    {
      title: 'Сумма',
      dataIndex: 'summa',
      key: 'summa',
      render: (val: number) =>
        val !== undefined && val !== null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
    {
      title: 'Статус',
      dataIndex: 'status',
      key: 'status',
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Title level={4}>Приход путевок КУ</Title>

      <Space style={{ marginBottom: 16 }} wrap>
        <Select
          placeholder="Выберите профсоюз"
          style={{ minWidth: 250 }}
          allowClear
          loading={zbLoading}
          value={zbId}
          onChange={(val) => setZbId(val)}
          options={zbOptions.map((zb) => ({ value: zb.id, label: zb.name }))}
        />
        <RangePicker
          format="DD.MM.YYYY"
          value={dateRange}
          onChange={(vals) =>
            setDateRange(vals as [dayjs.Dayjs | null, dayjs.Dayjs | null] | null)
          }
          placeholder={['Дата с (ZB)', 'Дата по (ZE)']}
        />
        <Button
          type="primary"
          onClick={handleOprixodovat}
          loading={oprixodovatMutation.isPending}
          disabled={!selectedInvoice || selectedInvoice.vid}
        >
          Оприходовать
        </Button>
        <Button onClick={handleExport}>Экспорт</Button>
      </Space>

      <QueryErrorAlert isError={invoicesError} onRetry={refetchInvoices} />
      <Table<KuInvoice>
        rowKey="id"
        dataSource={invoices}
        columns={masterColumns}
        loading={invoicesLoading}
        size="small"
        scroll={{ x: 'max-content' }}
        rowClassName={(record) =>
          selectedInvoice?.id === record.id ? 'ant-table-row-selected' : ''
        }
        onRow={(record) => ({
          onClick: () => handleRowClick(record),
          style: { cursor: 'pointer' },
        })}
        pagination={{ pageSize: 20, showSizeChanger: true }}
      />

      <Modal
        title={
          selectedInvoice
            ? `Детализация документа № ${selectedInvoice.numDoc}`
            : 'Детализация'
        }
        open={detailModalOpen}
        onCancel={() => setDetailModalOpen(false)}
        footer={[
          <Button key="close" onClick={() => setDetailModalOpen(false)}>
            Закрыть
          </Button>,
        ]}
        width={900}
      >
        <Table<KuDetail>
          rowKey="id"
          dataSource={details}
          columns={detailColumns}
          loading={detailsLoading}
          size="small"
          scroll={{ x: 'max-content' }}
          pagination={false}
        />
      </Modal>
    </div>
  );
}
