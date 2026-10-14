import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, InputNumber, Select,
  DatePicker, Row, Col, Typography, App, Upload, Checkbox, Statistic, Card,
} from 'antd';
import {
  PlusOutlined, EditOutlined, DeleteOutlined,
  DownloadOutlined, UploadOutlined, SearchOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { UploadFile } from 'antd/es/upload/interface';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { Title, Text } = Typography;

// ─────────────────────── interfaces ───────────────────────

interface OplataMain {
  id: number;
  data_Post: string;
  num_Doc: string;
  orgName: string;
  symma_DT: number;
  symma_KT: number;
}

interface OplataTotals {
  summaDebit: number;
  summaCredit: number;
  saldo: number;
}

interface PutyovkaDtl {
  id: number;
  sanatName: string;
  num_Put: string;
  name_Proc: string;
  symma: number;
}

interface TipOplati {
  id: number;
  name: string;
}

interface VidOplati {
  id: number;
  name: string;
}

interface ProcentZaPutyovki {
  id: number;
  name: string;
  procent: number;
}

interface NotCoveredPutyovka {
  id: number;
  num_Put: string;
  sanatName: string;
  symma: number;
  covered: number;
  remaining: number;
}

interface SummaModalData {
  debtByOrg: number;
  notCoveredPutyovkas: NotCoveredPutyovka[];
}

interface Organization {
  id: number;
  name: string;
  inn: string;
}

// ─────────────────────── OrganizationPickerModal ───────────────────────

interface OrgPickerProps {
  open: boolean;
  onCancel: () => void;
  onSelect: (org: Organization) => void;
}

function OrganizationPickerModal({ open, onCancel, onSelect }: OrgPickerProps) {
  const { message } = App.useApp();
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<Organization[]>([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    try {
      setLoading(true);
      const res = await api.get<Organization[]>(`/api/organizations?search=${encodeURIComponent(query)}`);
      setResults(res.data);
    } catch {
      message.error('Ошибка поиска организаций');
    } finally {
      setLoading(false);
    }
  };

  const columns = [
    { title: 'Название', dataIndex: 'name', key: 'name' },
    { title: 'ИНН', dataIndex: 'inn', key: 'inn', width: 130 },
    {
      title: '',
      key: 'action',
      width: 90,
      render: (_: unknown, record: Organization) => (
        <Button size="small" type="primary" onClick={() => onSelect(record)}>
          Выбрать
        </Button>
      ),
    },
  ];

  return (
    <Modal title="Поиск организации" open={open} onCancel={onCancel} footer={null} width={640}>
      <Space style={{ marginBottom: 12 }}>
        <Input
          placeholder="Название или ИНН"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          onPressEnter={handleSearch}
          style={{ width: 320 }}
        />
        <Button icon={<SearchOutlined />} type="primary" onClick={handleSearch} loading={loading}>
          Поиск
        </Button>
      </Space>
      <Table
        dataSource={results}
        columns={columns}
        rowKey="id"
        size="small"
        pagination={{ pageSize: 8 }}
        loading={loading}
      />
    </Modal>
  );
}

// ─────────────────────── main page ───────────────────────

export function OplataViplatiZaPutyovkiPage() {
  const { message, modal } = App.useApp();
  const queryClient = useQueryClient();
  const [form] = Form.useForm();

  // filter state
  const [year, setYear] = useState<number>(dayjs().year());
  const [quarter, setQuarter] = useState<number>(1);
  const [tipId, setTipId] = useState<number | undefined>();
  const [vidId, setVidId] = useState<number | undefined>();

  // selection
  const [selectedMain, setSelectedMain] = useState<OplataMain | null>(null);

  // modals
  const [addEditOpen, setAddEditOpen] = useState(false);
  const [editing, setEditing] = useState<OplataMain | null>(null);
  const [summaOpen, setSummaOpen] = useState(false);
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);

  // add/edit form supplementary state
  const [pickedOrg, setPickedOrg] = useState<Organization | null>(null);

  // summa modal state
  const [procentId, setProcentId] = useState<number | undefined>();
  const [selectedPutyovkaIds, setSelectedPutyovkaIds] = useState<number[]>([]);

  // excel state
  const [excelFile, setExcelFile] = useState<UploadFile | null>(null);

  // ── reference data ──
  const { data: tipList = [] } = useQuery<TipOplati[]>({
    queryKey: ['tip-oplati'],
    queryFn: () => api.get('/api/tip-oplati').then((r) => r.data),
  });

  const { data: vidList = [] } = useQuery<VidOplati[]>({
    queryKey: ['vid-oplati'],
    queryFn: () => api.get('/api/vid-oplati').then((r) => r.data),
  });

  const { data: procentList = [] } = useQuery<ProcentZaPutyovki[]>({
    queryKey: ['procent-za-putyovki'],
    queryFn: () => api.get('/api/procent-za-putyovki').then((r) => r.data),
  });

  // ── main list ──
  const { data: mainData, isLoading: mainLoading } = useQuery<{
    oplataMains: OplataMain[];
    totals: OplataTotals;
  }>({
    queryKey: ['oplata-viplati', year, quarter, tipId, vidId],
    queryFn: () => {
      const params = new URLSearchParams({
        year: String(year),
        quarter: String(quarter),
      });
      if (tipId !== undefined) params.append('tipId', String(tipId));
      if (vidId !== undefined) params.append('vidId', String(vidId));
      return api.get(`/api/oplata-viplati?${params}`).then((r) => r.data);
    },
  });

  const oplataMains = mainData?.oplataMains ?? [];
  const totals: OplataTotals = mainData?.totals ?? { summaDebit: 0, summaCredit: 0, saldo: 0 };

  // ── details ──
  const { data: details = [], isLoading: detailsLoading } = useQuery<PutyovkaDtl[]>({
    queryKey: ['oplata-viplati-details', selectedMain?.id],
    queryFn: () =>
      selectedMain
        ? api.get(`/api/oplata-viplati/${selectedMain.id}/details`).then((r) => r.data)
        : Promise.resolve([]),
    enabled: !!selectedMain,
  });

  // ── summa modal data ──
  const { data: summaData, isLoading: summaLoading, refetch: refetchSumma } = useQuery<SummaModalData>({
    queryKey: ['oplata-viplati-summa', selectedMain?.id, procentId],
    queryFn: () =>
      api
        .get(`/api/oplata-viplati/summa?orgId=${selectedMain?.id ?? ''}&procentId=${procentId ?? ''}`)
        .then((r) => r.data),
    enabled: false,
  });

  // ── mutations ──
  const saveMutation = useMutation({
    mutationFn: (values: Record<string, unknown>) =>
      editing
        ? api.put(`/api/oplata-viplati/${editing.id}`, values).then((r) => r.data)
        : api.post('/api/oplata-viplati', values).then((r) => r.data),
    onSuccess: () => {
      message.success('Сохранено');
      queryClient.invalidateQueries({ queryKey: ['oplata-viplati'] });
      setAddEditOpen(false);
    },
    onError: () => message.error('Ошибка сохранения'),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => api.delete(`/api/oplata-viplati/${id}`),
    onSuccess: () => {
      message.success('Удалено');
      queryClient.invalidateQueries({ queryKey: ['oplata-viplati'] });
      if (selectedMain && selectedMain.id === deleteMutation.variables) {
        setSelectedMain(null);
      }
    },
    onError: () => message.error('Ошибка удаления'),
  });

  const parseExcelMutation = useMutation({
    mutationFn: async (file: File) => {
      const fd = new FormData();
      fd.append('file', file);
      return api.post('/api/oplata-viplati/parse-excel', fd, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
    },
    onSuccess: () => {
      message.success('Excel обработан');
      queryClient.invalidateQueries({ queryKey: ['oplata-viplati'] });
      setExcelFile(null);
    },
    onError: () => message.error('Ошибка при разборе Excel'),
  });

  // ── open add/edit ──
  const openAdd = () => {
    setEditing(null);
    setPickedOrg(null);
    form.resetFields();
    setAddEditOpen(true);
  };

  const openEdit = (rec: OplataMain) => {
    setEditing(rec);
    setPickedOrg({ id: 0, name: rec.orgName, inn: '' });
    form.setFieldsValue({
      ...rec,
      data_Post: rec.data_Post ? dayjs(rec.data_Post) : undefined,
      orgId: undefined,
      name_org: rec.orgName,
    });
    setAddEditOpen(true);
  };

  const confirmDelete = (id: number) =>
    modal.confirm({
      title: 'Удалить запись?',
      onOk: () => deleteMutation.mutate(id),
    });

  const handleFormFinish = (values: Record<string, unknown>) => {
    saveMutation.mutate({
      ...values,
      data_Post: values.data_Post ? (values.data_Post as ReturnType<typeof dayjs>).toISOString() : undefined,
      orgId: pickedOrg?.id,
    });
  };

  const handleDownloadTemplate = async () => {
    try {
      const res = await api.get('/api/oplata-viplati/template', { responseType: 'blob' });
      const url = URL.createObjectURL(res.data as Blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'template.xlsx';
      a.click();
      URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка скачивания шаблона');
    }
  };

  const handleParseExcel = () => {
    if (!excelFile?.originFileObj) {
      message.warning('Выберите файл');
      return;
    }
    parseExcelMutation.mutate(excelFile.originFileObj as File);
  };

  const handleSummaOpen = () => {
    if (!selectedMain) {
      message.warning('Выберите запись в таблице');
      return;
    }
    setProcentId(undefined);
    setSelectedPutyovkaIds([]);
    setSummaOpen(true);
  };

  const handleProcentChange = (val: number) => {
    setProcentId(val);
    setTimeout(() => refetchSumma(), 0);
  };

  // ── columns ──
  const mainColumns = [
    { title: 'ID', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Дата', dataIndex: 'data_Post', key: 'data_Post', width: 110,
      render: (v: string) => v ? dayjs(v).format('DD.MM.YYYY') : '' },
    { title: 'Номер', dataIndex: 'num_Doc', key: 'num_Doc', width: 100 },
    { title: 'Организация', dataIndex: 'orgName', key: 'orgName' },
    { title: 'Сумма ДТ', dataIndex: 'symma_DT', key: 'symma_DT', width: 110,
      render: (v: number) => v?.toLocaleString() },
    { title: 'Сумма КТ', dataIndex: 'symma_KT', key: 'symma_KT', width: 110,
      render: (v: number) => v?.toLocaleString() },
    {
      title: '',
      key: 'actions',
      width: 90,
      render: (_: unknown, rec: OplataMain) => (
        <Space>
          <Button size="small" icon={<EditOutlined />} onClick={(e) => { e.stopPropagation(); openEdit(rec); }} />
          <Button size="small" danger icon={<DeleteOutlined />}
            onClick={(e) => { e.stopPropagation(); confirmDelete(rec.id); }} />
        </Space>
      ),
    },
  ];

  const detailColumns = [
    { title: 'ID', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Санаторий', dataIndex: 'sanatName', key: 'sanatName' },
    { title: 'Номер путевки', dataIndex: 'num_Put', key: 'num_Put', width: 120 },
    { title: 'Назначение', dataIndex: 'name_Proc', key: 'name_Proc' },
    { title: 'Сумма', dataIndex: 'symma', key: 'symma', width: 110,
      render: (v: number) => v?.toLocaleString() },
  ];

  const notCoveredColumns = [
    {
      title: '',
      key: 'check',
      width: 40,
      render: (_: unknown, rec: NotCoveredPutyovka) => (
        <Checkbox
          checked={selectedPutyovkaIds.includes(rec.id)}
          onChange={(e) =>
            setSelectedPutyovkaIds((prev) =>
              e.target.checked ? [...prev, rec.id] : prev.filter((x) => x !== rec.id)
            )
          }
        />
      ),
    },
    { title: 'Путевка', dataIndex: 'num_Put', key: 'num_Put' },
    { title: 'Санаторий', dataIndex: 'sanatName', key: 'sanatName' },
    { title: 'Сумма', dataIndex: 'symma', key: 'symma', width: 100,
      render: (v: number) => v?.toLocaleString() },
    { title: 'Покрыто', dataIndex: 'covered', key: 'covered', width: 100,
      render: (v: number) => v?.toLocaleString() },
    { title: 'Остаток', dataIndex: 'remaining', key: 'remaining', width: 100,
      render: (v: number) => v?.toLocaleString() },
  ];

  const notCovered = summaData?.notCoveredPutyovkas ?? [];
  const selectedNotCovered = notCovered.filter((p) => selectedPutyovkaIds.includes(p.id));
  const totalCount = selectedNotCovered.length;
  const totalSum = selectedNotCovered.reduce((acc, p) => acc + p.symma, 0);
  const totalCovered = selectedNotCovered.reduce((acc, p) => acc + p.covered, 0);
  const totalRemaining = selectedNotCovered.reduce((acc, p) => acc + p.remaining, 0);

  // ── year options ──
  const currentYear = dayjs().year();
  const yearOptions = Array.from({ length: 10 }, (_, i) => ({
    value: currentYear - i,
    label: String(currentYear - i),
  }));

  return (
    <div style={{ padding: 24 }}>
      <Title level={3} style={{ marginBottom: 16 }}>Оплата и выплаты за путевки</Title>

      {/* ── Filters ── */}
      <Card size="small" style={{ marginBottom: 16 }}>
        <Space wrap>
          <Space>
            <Text>Год:</Text>
            <Select
              value={year}
              onChange={setYear}
              options={yearOptions}
              style={{ width: 100 }}
            />
          </Space>
          <Space>
            <Text>Квартал:</Text>
            <Select
              value={quarter}
              onChange={setQuarter}
              style={{ width: 100 }}
              options={[
                { value: 1, label: 'I' },
                { value: 2, label: 'II' },
                { value: 3, label: 'III' },
                { value: 4, label: 'IV' },
              ]}
            />
          </Space>
          <Space>
            <Text>Тип оплаты:</Text>
            <Select
              allowClear
              value={tipId}
              onChange={setTipId}
              style={{ width: 180 }}
              placeholder="Все"
              options={tipList.map((t) => ({ value: t.id, label: t.name }))}
            />
          </Space>
          <Space>
            <Text>Вид оплаты:</Text>
            <Select
              allowClear
              value={vidId}
              onChange={setVidId}
              style={{ width: 180 }}
              placeholder="Все"
              options={vidList.map((v) => ({ value: v.id, label: v.name }))}
            />
          </Space>
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>
            Добавить
          </Button>
          <Button onClick={handleSummaOpen}>Сумма</Button>
        </Space>
      </Card>

      {/* ── Main content: LEFT table + RIGHT detail ── */}
      <Row gutter={16}>
        {/* LEFT: OplataMains */}
        <Col xs={24} xl={14}>
          <Title level={5}>Оплаты</Title>
          <Table
            dataSource={oplataMains}
            columns={mainColumns}
            rowKey="id"
            size="small"
            loading={mainLoading}
            pagination={{ pageSize: 15 }}
            rowClassName={(rec) => rec.id === selectedMain?.id ? 'ant-table-row-selected' : ''}
            onRow={(rec) => ({
              onClick: () => setSelectedMain(rec),
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 'max-content' }}
          />
          {/* Footer totals */}
          <Row gutter={16} style={{ marginTop: 8 }}>
            <Col>
              <Statistic title="Сумма ДТ" value={totals.summaDebit} precision={2} />
            </Col>
            <Col>
              <Statistic title="Сумма КТ" value={totals.summaCredit} precision={2} />
            </Col>
            <Col>
              <Statistic title="Сальдо" value={totals.saldo} precision={2}
                valueStyle={{ color: totals.saldo < 0 ? '#cf1322' : '#3f8600' }} />
            </Col>
          </Row>
        </Col>

        {/* RIGHT: PutyovkaDtls */}
        <Col xs={24} xl={10}>
          <Title level={5}>
            Детали путевок
            {selectedMain && (
              <Text type="secondary" style={{ fontSize: 13, fontWeight: 400, marginLeft: 8 }}>
                ({selectedMain.orgName}, №{selectedMain.num_Doc})
              </Text>
            )}
          </Title>
          {!selectedMain ? (
            <Text type="secondary">Выберите запись слева для просмотра деталей</Text>
          ) : (
            <Table
              dataSource={details}
              columns={detailColumns}
              rowKey="id"
              size="small"
              loading={detailsLoading}
              pagination={{ pageSize: 15 }}
              scroll={{ x: 'max-content' }}
            />
          )}
        </Col>
      </Row>

      {/* ── Excel section ── */}
      <Card size="small" title="Импорт из Excel" style={{ marginTop: 24 }}>
        <Space wrap>
          <Button icon={<DownloadOutlined />} onClick={handleDownloadTemplate}>
            Скачать шаблон
          </Button>
          <Upload
            beforeUpload={() => false}
            maxCount={1}
            accept=".xlsx,.xls"
            fileList={excelFile ? [excelFile] : []}
            onChange={({ fileList }) => setExcelFile(fileList[0] ?? null)}
          >
            <Button icon={<UploadOutlined />}>Выбрать файл</Button>
          </Upload>
          <Button
            type="primary"
            onClick={handleParseExcel}
            loading={parseExcelMutation.isPending}
            disabled={!excelFile}
          >
            Разобрать Excel
          </Button>
        </Space>
      </Card>

      {/* ── Add / Edit Modal ── */}
      <Modal
        title={editing ? 'Редактировать запись' : 'Добавить запись'}
        open={addEditOpen}
        onOk={() => form.submit()}
        onCancel={() => setAddEditOpen(false)}
        confirmLoading={saveMutation.isPending}
        width={600}
        destroyOnClose
      >
        <Form form={form} layout="vertical" onFinish={handleFormFinish}>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="tipOplatiId" label="Тип оплаты" rules={[{ required: true }]}>
                <Select
                  options={tipList.map((t) => ({ value: t.id, label: t.name }))}
                  placeholder="Выберите тип"
                />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="vidOplatiId" label="Вид оплаты" rules={[{ required: true }]}>
                <Select
                  options={vidList.map((v) => ({ value: v.id, label: v.name }))}
                  placeholder="Выберите вид"
                />
              </Form.Item>
            </Col>
          </Row>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="data_Post" label="Дата" rules={[{ required: true }]}>
                <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="num_Doc" label="Номер документа">
                <Input />
              </Form.Item>
            </Col>
          </Row>
          <Form.Item label="Организация">
            <Space.Compact style={{ width: '100%' }}>
              <Input
                readOnly
                value={pickedOrg?.name ?? ''}
                placeholder="Не выбрана"
                style={{ flex: 1 }}
              />
              <Button onClick={() => setOrgPickerOpen(true)}>Выбрать</Button>
            </Space.Compact>
          </Form.Item>
          <Form.Item name="name_org" label="Наименование (свободно)">
            <Input />
          </Form.Item>
          <Form.Item name="osnov" label="Основание">
            <Input />
          </Form.Item>
          <Row gutter={16}>
            <Col span={12}>
              <Form.Item name="symma_KT" label="Сумма КТ">
                <InputNumber style={{ width: '100%' }} min={0} />
              </Form.Item>
            </Col>
            <Col span={12}>
              <Form.Item name="symma_DT" label="Сумма ДТ">
                <InputNumber style={{ width: '100%' }} min={0} />
              </Form.Item>
            </Col>
          </Row>
        </Form>
      </Modal>

      {/* ── Organization picker (inside add/edit flow) ── */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onCancel={() => setOrgPickerOpen(false)}
        onSelect={(org) => {
          setPickedOrg(org);
          form.setFieldsValue({ name_org: org.name });
          setOrgPickerOpen(false);
        }}
      />

      {/* ── Summa Modal ── */}
      <Modal
        title="Сумма по организации"
        open={summaOpen}
        onCancel={() => setSummaOpen(false)}
        footer={[
          <Button key="close" onClick={() => setSummaOpen(false)}>
            Закрыть
          </Button>,
        ]}
        width={760}
        destroyOnClose
      >
        <Space direction="vertical" style={{ width: '100%' }}>
          <Row gutter={16} align="middle">
            <Col>
              <Text>Процент за путевки:</Text>
            </Col>
            <Col>
              <Select
                value={procentId}
                onChange={handleProcentChange}
                style={{ width: 240 }}
                placeholder="Выберите процент"
                options={procentList.map((p) => ({ value: p.id, label: `${p.name} (${p.procent}%)` }))}
              />
            </Col>
          </Row>

          {summaLoading && <Text>Загрузка...</Text>}

          {summaData && (
            <>
              <Statistic
                title="Долг по организации"
                value={summaData.debtByOrg}
                precision={2}
                valueStyle={{ color: summaData.debtByOrg > 0 ? '#cf1322' : '#3f8600' }}
              />

              <Title level={5} style={{ marginTop: 16 }}>Непокрытые путевки</Title>
              <Table
                dataSource={notCovered}
                columns={notCoveredColumns}
                rowKey="id"
                size="small"
                pagination={{ pageSize: 10 }}
                scroll={{ x: 'max-content' }}
              />

              {selectedPutyovkaIds.length > 0 && (
                <Row gutter={16} style={{ marginTop: 8 }}>
                  <Col>
                    <Statistic title="Кол-во" value={totalCount} />
                  </Col>
                  <Col>
                    <Statistic title="Итого сумма" value={totalSum} precision={2} />
                  </Col>
                  <Col>
                    <Statistic title="Покрыто" value={totalCovered} precision={2} />
                  </Col>
                  <Col>
                    <Statistic title="Остаток" value={totalRemaining} precision={2}
                      valueStyle={{ color: totalRemaining > 0 ? '#cf1322' : '#3f8600' }} />
                  </Col>
                </Row>
              )}
            </>
          )}
        </Space>
      </Modal>
    </div>
  );
}
