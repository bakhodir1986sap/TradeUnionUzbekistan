import { useState } from 'react';
import {
  Button, Table, Space, Modal, Input, Select, DatePicker,
  Row, Col, Typography, App,
} from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { useQuery } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import { OrganizationPickerModal } from '../../components/OrganizationPickerModal';
import api from '../../api/axios';
import dayjs from 'dayjs';
import type { ColumnType } from 'antd/es/table';

const { Title, Text } = Typography;

// ─── Types ───────────────────────────────────────────────────────────────────

interface Organization {
  id: number;
  name: string;
  inn: string;
  branchName?: string;
}

interface FizLiso {
  id: number;
  fullName: string;
  inn: string;
  organizationName?: string;
}

interface TotalResult {
  putyovkaCount: number;
  summaNakladnix: number;
}

interface NakladnoyRow {
  id: number;
  data_post: string;
  num_doc: string;
  num_kol: number;
  symma: number;
}

interface VoucherDetailRow {
  id: number;
  sanatName: string;
  dataVidachi: string;
  num_put: string;
  fio: string;
  dataRojd: string;
  dataZaezda: string;
  dataViезda: string;
  price: number;
  oplata: number;
  profileType: string;
}

// ─── FizLiso Picker Modal ─────────────────────────────────────────────────────

interface FizLisoPickerProps {
  open: boolean;
  onClose: () => void;
  onSelect: (f: FizLiso) => void;
}

function FizLisoPickerModal({ open, onClose, onSelect }: FizLisoPickerProps) {
  const [search, setSearch] = useState('');

  const { data: fizLisos = [], isFetching } = useQuery<FizLiso[]>({
    queryKey: ['fizliso-picker-danniye', search],
    queryFn: () =>
      api.get('/api/fizliso', { params: { search } }).then(r => r.data),
    enabled: open,
  });

  const columns: ColumnType<FizLiso>[] = [
    { title: 'ФИО', dataIndex: 'fullName' },
    { title: 'ИНН', dataIndex: 'inn', width: 130 },
    { title: 'Организация', dataIndex: 'organizationName' },
  ];

  return (
    <Modal
      title="Выбор физического лица"
      open={open}
      onCancel={onClose}
      footer={null}
      width={700}
      destroyOnClose
    >
      <Input
        prefix={<SearchOutlined />}
        placeholder="Поиск по ФИО / ИНН"
        value={search}
        onChange={e => setSearch(e.target.value)}
        allowClear
        style={{ marginBottom: 12 }}
      />
      <Table<FizLiso>
        columns={columns}
        dataSource={fizLisos}
        rowKey="id"
        size="small"
        loading={isFetching}
        pagination={{ pageSize: 8 }}
        onRow={rec => ({ onClick: () => { onSelect(rec); onClose(); } })}
        style={{ cursor: 'pointer' }}
      />
    </Modal>
  );
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function DanniyeVladelsaxPutyovokPage() {
  const { message } = App.useApp();

  // Filters
  const [beginDate, setBeginDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [selectedOrg, setSelectedOrg] = useState<Organization | null>(null);
  const [selectedFizLiso, setSelectedFizLiso] = useState<FizLiso | null>(null);

  // Applied filters (triggered on "Показать")
  const [appliedBeginDate, setAppliedBeginDate] = useState<string>('');
  const [appliedEndDate, setAppliedEndDate] = useState<string>('');
  const [appliedOrgId, setAppliedOrgId] = useState<number | null>(null);
  const [appliedFizLisoId, setAppliedFizLisoId] = useState<number | null>(null);
  const [queryEnabled, setQueryEnabled] = useState(false);

  // Selected nakladnoy row
  const [selectedNakladnoy, setSelectedNakladnoy] = useState<NakladnoyRow | null>(null);

  // Modals
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);
  const [fizLisoPickerOpen, setFizLisoPickerOpen] = useState(false);

  // ── Main data query ──
  const { data: mainData, isLoading: mainLoading, isError: mainError, refetch: refetchMain } = useQuery<{
    nakladnoys: NakladnoyRow[];
    total: TotalResult;
  }>({
    queryKey: ['danniye-vladelsax-putyovok', appliedBeginDate, appliedEndDate, appliedOrgId, appliedFizLisoId],
    queryFn: () =>
      api.get('/api/danniyevladelsaxputyovok', {
        params: {
          beginDate: appliedBeginDate || undefined,
          endDate: appliedEndDate || undefined,
          organizationId: appliedOrgId || undefined,
          fizLisoId: appliedFizLisoId || undefined,
        },
      }).then(r => r.data),
    enabled: queryEnabled,
    staleTime: 0,
  });

  const nakladnoys: NakladnoyRow[] = mainData?.nakladnoys ?? [];
  const totalRes: TotalResult = mainData?.total ?? { putyovkaCount: 0, summaNakladnix: 0 };

  // ── Detail vouchers for selected nakladnoy ──
  const { data: detailRows = [], isLoading: detailLoading } = useQuery<VoucherDetailRow[]>({
    queryKey: ['danniye-vladelsax-putyovok-detail', selectedNakladnoy?.id],
    queryFn: () =>
      api.get(`/api/danniyevladelsaxputyovok/${selectedNakladnoy!.id}/details`).then(r => r.data),
    enabled: !!selectedNakladnoy,
  });

  // ── Handlers ──
  const handleShowClicked = () => {
    if (!beginDate || !endDate) {
      message.warning('Укажите период (с и по)');
      return;
    }
    setAppliedBeginDate(beginDate);
    setAppliedEndDate(endDate);
    setAppliedOrgId(selectedOrg?.id ?? null);
    setAppliedFizLisoId(selectedFizLiso?.id ?? null);
    setQueryEnabled(true);
    setSelectedNakladnoy(null);
  };

  const handleOrgSelect = (org: Organization) => {
    setSelectedOrg(org);
  };

  const handleFizLisoSelect = (f: FizLiso) => {
    setSelectedFizLiso(f);
  };

  // ── Nakladnoy columns (left table) ──
  const nakladnoyColumns: ColumnType<NakladnoyRow>[] = [
    {
      title: 'Дата',
      dataIndex: 'data_post',
      key: 'data_post',
      width: 110,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
    {
      title: '№ документа',
      dataIndex: 'num_doc',
      key: 'num_doc',
      width: 130,
    },
    {
      title: 'Кол-во',
      dataIndex: 'num_kol',
      key: 'num_kol',
      width: 80,
      align: 'right' as const,
    },
    {
      title: 'Сумма',
      dataIndex: 'symma',
      key: 'symma',
      width: 120,
      align: 'right' as const,
      render: (val: number) =>
        val != null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
  ];

  // ── Voucher detail columns (right table) ──
  const voucherColumns: ColumnType<VoucherDetailRow>[] = [
    {
      title: '№',
      key: 'index',
      width: 50,
      render: (_: unknown, __: VoucherDetailRow, index: number) => index + 1,
    },
    {
      title: 'Санаторий',
      dataIndex: 'sanatName',
      key: 'sanatName',
    },
    {
      title: 'Дата выдачи',
      dataIndex: 'dataVidachi',
      key: 'dataVidachi',
      width: 110,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
    {
      title: '№ путевки',
      dataIndex: 'num_put',
      key: 'num_put',
      width: 120,
    },
    {
      title: 'ФИО',
      dataIndex: 'fio',
      key: 'fio',
    },
    {
      title: 'Дата рождения',
      dataIndex: 'dataRojd',
      key: 'dataRojd',
      width: 120,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
    {
      title: 'Дата заезда',
      dataIndex: 'dataZaezda',
      key: 'dataZaezda',
      width: 110,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
    {
      title: 'Дата выезда',
      dataIndex: 'dataViезda',
      key: 'dataViезда',
      width: 110,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
    {
      title: 'Цена',
      dataIndex: 'price',
      key: 'price',
      width: 110,
      align: 'right' as const,
      render: (val: number) =>
        val != null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
    {
      title: 'Оплата',
      dataIndex: 'oplata',
      key: 'oplata',
      width: 110,
      align: 'right' as const,
      render: (val: number) =>
        val != null
          ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : '',
    },
    {
      title: 'Профиль',
      dataIndex: 'profileType',
      key: 'profileType',
      width: 120,
    },
  ];

  // ── Render ──
  return (
    <div style={{ padding: 16 }}>
      {/* Header */}
      <Row justify="space-between" align="top" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Учет и контроль курортно-санаторных путевок
          </Title>
          <Text type="secondary" style={{ fontSize: 12 }}>
            Путевки / Данные о владельцах путевок
          </Text>
        </Col>
        <Col style={{ textAlign: 'right' }}>
          <Text strong style={{ display: 'block', marginBottom: 4 }}>
            Общее количество и сумма
          </Text>
          <Text style={{ display: 'block' }}>
            Количество: <Text strong>{totalRes.putyovkaCount}</Text>
          </Text>
          <Text style={{ display: 'block' }}>
            Сумма:{' '}
            <Text strong>
              {totalRes.summaNakladnix.toLocaleString('ru-RU', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2,
              })}
            </Text>
          </Text>
        </Col>
      </Row>

      {/* Filters */}
      <Row gutter={[12, 12]} style={{ marginBottom: 16 }} align="bottom">
        <Col xs={24} sm={12} md={6}>
          <Text style={{ display: 'block', marginBottom: 4 }}>Период с</Text>
          <DatePicker
            style={{ width: '100%' }}
            format="DD.MM.YYYY"
            placeholder="с"
            value={beginDate ? dayjs(beginDate) : null}
            onChange={d => setBeginDate(d ? d.format('YYYY-MM-DD') : '')}
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Text style={{ display: 'block', marginBottom: 4 }}>Период по</Text>
          <DatePicker
            style={{ width: '100%' }}
            format="DD.MM.YYYY"
            placeholder="по"
            value={endDate ? dayjs(endDate) : null}
            onChange={d => setEndDate(d ? d.format('YYYY-MM-DD') : '')}
          />
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Text style={{ display: 'block', marginBottom: 4 }}>ИНН предприятия</Text>
          <Select
            style={{ width: '100%' }}
            placeholder="Организация не выбрана"
            value={selectedOrg ? selectedOrg.id : undefined}
            allowClear
            onClear={() => setSelectedOrg(null)}
            open={false}
            onClick={() => setOrgPickerOpen(true)}
            notFoundContent={null}
            options={selectedOrg ? [{ label: selectedOrg.name, value: selectedOrg.id }] : []}
          />
          {selectedOrg && (
            <Text type="secondary" style={{ fontSize: 11 }}>
              {selectedOrg.name}
            </Text>
          )}
        </Col>
        <Col xs={24} sm={24} md={6}>
          <Space wrap>
            <Button onClick={() => setOrgPickerOpen(true)}>
              Выбрать Организацию
            </Button>
            <Button onClick={() => setFizLisoPickerOpen(true)}>
              Выбрать FizLiso
            </Button>
            <Button type="primary" onClick={handleShowClicked} loading={mainLoading}>
              Показать
            </Button>
          </Space>
          {selectedFizLiso && (
            <Text type="secondary" style={{ fontSize: 11, display: 'block', marginTop: 4 }}>
              Физ. лицо: {selectedFizLiso.fullName}
            </Text>
          )}
        </Col>
      </Row>

      {/* Master-detail tables */}
      <Row gutter={12}>
        {/* LEFT: Накладные */}
        <Col xs={24} lg={8}>
          <QueryErrorAlert isError={mainError} onRetry={refetchMain} />
          <Table<NakladnoyRow>
            columns={nakladnoyColumns}
            dataSource={nakladnoys}
            rowKey="id"
            loading={mainLoading}
            size="small"
            pagination={{ pageSize: 20, showSizeChanger: false }}
            rowClassName={rec =>
              selectedNakladnoy?.id === rec.id ? 'ant-table-row-selected' : ''
            }
            onRow={rec => ({
              onClick: () =>
                setSelectedNakladnoy(prev => (prev?.id === rec.id ? null : rec)),
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 400 }}
            locale={{ emptyText: 'Нет данных' }}
          />
        </Col>

        {/* RIGHT: Путевки (детальные данные) */}
        <Col xs={24} lg={16}>
          <Table<VoucherDetailRow>
            columns={voucherColumns}
            dataSource={detailRows}
            rowKey="id"
            loading={detailLoading}
            size="small"
            pagination={{ pageSize: 20, showSizeChanger: false }}
            scroll={{ x: 900 }}
            locale={{
              emptyText: selectedNakladnoy
                ? 'Нет данных по выбранному документу'
                : 'Выберите строку слева для просмотра путевок',
            }}
          />
        </Col>
      </Row>

      {/* Organization Picker Modal */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={handleOrgSelect}
      />

      {/* FizLiso Picker Modal */}
      <FizLisoPickerModal
        open={fizLisoPickerOpen}
        onClose={() => setFizLisoPickerOpen(false)}
        onSelect={handleFizLisoSelect}
      />
    </div>
  );
}
