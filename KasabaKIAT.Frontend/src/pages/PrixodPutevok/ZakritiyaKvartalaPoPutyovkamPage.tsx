import { useState } from 'react';
import {
  Button,
  Table,
  Select,
  Typography,
  App,
  Row,
  Col,
  Space,
  Input,
  Checkbox,
} from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { ColumnType } from 'antd/es/table';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Title, Text } = Typography;
const { Option } = Select;

// ─── Types ───────────────────────────────────────────────────────────────────

interface YearItem {
  Id: number;
  Year: number;
}

interface KvartalItem {
  ID: number;
  Name_Kv: string;
}

interface Organization {
  ID: number;
  Name_Org: string;
  INN?: string;
}

interface Putyovka {
  id: number;
  sanatName: string;
  num_put: string;
  symma: number;
  data_post: string;
}

interface ZakritiyeResult {
  PutyovkaCount: number;
  SummaNakladnix: number;
}

// ─── Organization Picker Modal ────────────────────────────────────────────────

import { Modal } from 'antd';

interface OrgPickerProps {
  open: boolean;
  onClose: () => void;
  onSelect: (org: Organization) => void;
}

function OrganizationPickerModal({ open, onClose, onSelect }: OrgPickerProps) {
  const [search, setSearch] = useState('');

  const { data: orgs = [], isFetching } = useQuery<Organization[]>({
    queryKey: ['org-picker-zakritiye', search],
    queryFn: () =>
      api.get('/api/organizations', { params: { search } }).then((r) => r.data),
    enabled: open,
  });

  const columns: ColumnType<Organization>[] = [
    {
      title: 'Название организации',
      dataIndex: 'Name_Org',
    },
    {
      title: 'ИНН',
      dataIndex: 'INN',
      width: 140,
    },
  ];

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
        placeholder="Поиск по названию или ИНН"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        allowClear
        style={{ marginBottom: 12 }}
      />
      <Table<Organization>
        columns={columns}
        dataSource={orgs}
        rowKey="ID"
        size="small"
        loading={isFetching}
        pagination={{ pageSize: 10 }}
        onRow={(rec) => ({
          onClick: () => {
            onSelect(rec);
            onClose();
          },
          style: { cursor: 'pointer' },
        })}
      />
    </Modal>
  );
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function ZakritiyaKvartalaPoPutyovkamPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();

  const currentYear = new Date().getFullYear();

  // ── Filters ──
  const [selectedYear, setSelectedYear] = useState<YearItem | null>(null);
  const [selectedKvartal, setSelectedKvartal] = useState<KvartalItem | null>(null);
  const [selectedOrganization, setSelectedOrganization] = useState<Organization | null>(null);
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);

  // ── Selected vouchers ──
  const [selectedIds, setSelectedIds] = useState<number[]>([]);

  // ── Reference: Years ──
  const { data: years = [] } = useQuery<YearItem[]>({
    queryKey: ['years-zakritiye'],
    queryFn: () => api.get('/api/reference/years').then((r) => r.data),
    initialData: Array.from({ length: 6 }, (_, i) => ({
      Id: currentYear - 2 + i,
      Year: currentYear - 2 + i,
    })),
  });

  // ── Reference: Kvartals ──
  const { data: kvartals = [] } = useQuery<KvartalItem[]>({
    queryKey: ['kvartals'],
    queryFn: () => api.get('/api/reference/kvartals').then((r) => r.data),
    initialData: [
      { ID: 1, Name_Kv: '1 квартал' },
      { ID: 2, Name_Kv: '2 квартал' },
      { ID: 3, Name_Kv: '3 квартал' },
      { ID: 4, Name_Kv: '4 квартал' },
    ],
  });

  // ── Main data: Putyovkas ──
  const filtersReady = !!(selectedYear && selectedKvartal && selectedOrganization);

  const {
    data: putyovkasData,
    isLoading: putyovkasLoading,
    refetch: refetchPutyovkas,
  } = useQuery<{ putyovkas: Putyovka[]; zakritiyeRes: ZakritiyeResult }>({
    queryKey: [
      'zakritiye-putevok',
      selectedYear?.Id,
      selectedKvartal?.ID,
      selectedOrganization?.ID,
    ],
    queryFn: () =>
      api
        .get('/api/zakritiyakvartalapoputyovkam', {
          params: {
            yearId: selectedYear?.Id,
            kvartalId: selectedKvartal?.ID,
            organizationId: selectedOrganization?.ID,
          },
        })
        .then((r) => r.data),
    enabled: filtersReady,
  });

  const putyovkas: Putyovka[] = putyovkasData?.putyovkas ?? [];
  const zakritiyeRes: ZakritiyeResult = putyovkasData?.zakritiyeRes ?? {
    PutyovkaCount: 0,
    SummaNakladnix: 0,
  };

  // ── Mutation: Close selected vouchers ──
  const saveZakritieMutation = useMutation({
    mutationFn: (ids: number[]) =>
      api
        .post('/api/zakritiyakvartalapoputyovkam/close', { ids })
        .then((r) => r.data),
    onSuccess: () => {
      message.success('Выделенные путевки закрыты');
      setSelectedIds([]);
      queryClient.invalidateQueries({
        queryKey: [
          'zakritiye-putevok',
          selectedYear?.Id,
          selectedKvartal?.ID,
          selectedOrganization?.ID,
        ],
      });
      refetchPutyovkas();
    },
    onError: () => message.error('Ошибка при закрытии путевок'),
  });

  // ── Handlers ──
  const handleFiltersChanged = () => {
    setSelectedIds([]);
  };

  const handleOrgSelect = (org: Organization) => {
    setSelectedOrganization(org);
    setSelectedIds([]);
  };

  const toggleVoucherSelection = (id: number) => {
    setSelectedIds((prev) =>
      prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
    );
  };

  const handleSaveZakritie = () => {
    if (selectedIds.length === 0) {
      message.warning('Выберите хотя бы одну путевку');
      return;
    }
    modal.confirm({
      title: 'Закрыть выделенные путевки?',
      content: `Будет закрыто путевок: ${selectedIds.length}`,
      okText: 'Закрыть',
      okButtonProps: { danger: true },
      cancelText: 'Отмена',
      onOk: () => saveZakritieMutation.mutate(selectedIds),
    });
  };

  // ── Table columns ──
  const columns: ColumnType<Putyovka>[] = [
    {
      title: 'Номер п/п',
      key: 'index',
      width: 100,
      render: (_: unknown, record: Putyovka) => (
        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <Checkbox
            checked={selectedIds.includes(record.id)}
            onChange={() => toggleVoucherSelection(record.id)}
          />
          <span>{record.id}</span>
        </div>
      ),
    },
    {
      title: 'Название',
      dataIndex: 'sanatName',
      key: 'sanatName',
    },
    {
      title: '№ Путевки',
      dataIndex: 'num_put',
      key: 'num_put',
      width: 130,
    },
    {
      title: 'Стоимость',
      dataIndex: 'symma',
      key: 'symma',
      width: 130,
      align: 'right',
      render: (val: number) =>
        val != null
          ? val.toLocaleString('ru-RU', {
              minimumFractionDigits: 2,
              maximumFractionDigits: 2,
            })
          : '',
    },
    {
      title: 'Срок действия',
      dataIndex: 'data_post',
      key: 'data_post',
      width: 140,
      render: (val: string) => (val ? dayjs(val).format('DD.MM.YYYY') : ''),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      {/* Header */}
      <Row justify="space-between" align="middle" style={{ marginBottom: 8 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>
            Закрытие квартала по путевкам
          </Title>
          <nav aria-label="breadcrumb" style={{ marginTop: 4 }}>
            <Text type="secondary">
              <a href="/Home/Putyovki">Путевки</a>
              {' / '}
              <Text>Закрытие квартала по путевкам</Text>
            </Text>
          </nav>
        </Col>
      </Row>

      {/* Filters */}
      <Row gutter={16} style={{ marginBottom: 24, marginTop: 16 }}>
        {/* Year */}
        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <Text strong>Год</Text>
          </div>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите год"
            value={selectedYear?.Id ?? undefined}
            onChange={(val) => {
              const found = years.find((y) => y.Id === val) ?? null;
              setSelectedYear(found);
              handleFiltersChanged();
            }}
          >
            {years.map((y) => (
              <Option key={y.Id} value={y.Id}>
                {y.Year}
              </Option>
            ))}
          </Select>
        </Col>

        {/* Kvartal */}
        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <Text strong>Квартал</Text>
          </div>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите квартал"
            value={selectedKvartal?.ID ?? undefined}
            onChange={(val) => {
              const found = kvartals.find((k) => k.ID === val) ?? null;
              setSelectedKvartal(found);
              handleFiltersChanged();
            }}
          >
            {kvartals.map((k) => (
              <Option key={k.ID} value={k.ID}>
                {k.Name_Kv}
              </Option>
            ))}
          </Select>
        </Col>

        {/* Organization */}
        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <Text strong>ИНН Организации</Text>
          </div>
          <Input
            value={selectedOrganization?.Name_Org ?? ''}
            readOnly
            placeholder="Организация не выбрана"
            style={{ marginBottom: 6 }}
          />
          <div style={{ textAlign: 'right' }}>
            <Button onClick={() => setOrgPickerOpen(true)}>Выбрать</Button>
          </div>
        </Col>

        {/* Selected organization display */}
        <Col xs={24} sm={12} md={6}>
          <div style={{ marginTop: 38 }}>
            <Text>Выбранная организация:</Text>
            <br />
            <Text strong>{selectedOrganization?.Name_Org ?? '—'}</Text>
          </div>
        </Col>
      </Row>

      {/* Summary */}
      <Row justify="space-between" align="middle" style={{ marginBottom: 16 }}>
        <Col>
          <Title level={5} style={{ margin: 0 }}>
            Список оставшихся путевок
          </Title>
        </Col>
        <Col>
          <Space direction="vertical" size={0} style={{ textAlign: 'right' }}>
            <Text>
              Сумма: <Text strong>{zakritiyeRes.PutyovkaCount}</Text>
            </Text>
            <Text>
              Количество: <Text strong>{zakritiyeRes.SummaNakladnix}</Text>
            </Text>
          </Space>
        </Col>
      </Row>

      {/* Table */}
      <Table<Putyovka>
        rowKey="id"
        columns={columns}
        dataSource={putyovkas}
        loading={putyovkasLoading}
        size="small"
        bordered
        scroll={{ x: 'max-content' }}
        pagination={{
          pageSize: 20,
          showSizeChanger: true,
          showTotal: (total) => `Всего: ${total}`,
        }}
        locale={{
          emptyText: filtersReady
            ? 'Нет путевок для закрытия'
            : 'Выберите год, квартал и организацию',
        }}
        style={{ marginBottom: 16 }}
      />

      {/* Action button */}
      <div style={{ marginTop: 16 }}>
        <Button
          danger
          type="primary"
          onClick={handleSaveZakritie}
          loading={saveZakritieMutation.isPending}
          disabled={selectedIds.length === 0}
        >
          Закрыть выделенные путевки
        </Button>
        {selectedIds.length > 0 && (
          <Text type="secondary" style={{ marginLeft: 12 }}>
            Выбрано: {selectedIds.length}
          </Text>
        )}
      </div>

      {/* Organization Picker Modal */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={handleOrgSelect}
      />
    </div>
  );
}
