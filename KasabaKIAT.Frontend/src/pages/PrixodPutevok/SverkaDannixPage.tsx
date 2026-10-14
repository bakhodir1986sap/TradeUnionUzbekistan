import { useState, useRef } from 'react';
import {
  Button,
  Table,
  Select,
  Typography,
  App,
  Row,
  Col,
  Space,
  Breadcrumb,
  Divider,
  Tag,
} from 'antd';
import { DownloadOutlined, UploadOutlined, ClearOutlined, SearchOutlined, ExportOutlined } from '@ant-design/icons';
import { useQuery, useMutation } from '@tanstack/react-query';
import type { ColumnsType } from 'antd/es/table';
import api from '../../api/axios';

const { Title } = Typography;
const { Option } = Select;

// ── Interfaces ──────────────────────────────────────────────────────────────

interface Year {
  Id: number;
  Year: number;
}

interface Side {
  Code: string;
  Decode: string;
}

interface Sanatory {
  ID: number;
  Name_San: string;
}

interface Profsouz {
  MATCH_DEPARTMENTBASE: string;
  ProfsoyzNaimenovanie: string;
}

interface DifferRecord {
  SanatName: string;
  NumPut: string;
  KurortFio: string;
  KurortAdress: string;
  OblastFio: string;
  OblastAdress: string;
}

interface MainTableRecord {
  id: number | string;
  numPut: string;
  sanatName: string;
  dataSanat: string;
  infoOtdixayushego: string;
  infoOtdixayushegoProfsouz: string;
  adresSanatory: string;
  adresProfsouz: string;
  tipDokumenta: string;
  tipDokumentaProfsouz: string;
  seriaNomer: string;
  seriaNomerProfsouz: string;
  dataVidachi: string;
  dataVidachiProfsouz: string;
  kimTomonidan: string;
  kimTomonidanProfsouz: string;
}

// ── Page component ───────────────────────────────────────────────────────────

export function SverkaDannixPage() {
  const { message } = App.useApp();
  const fileInputRef = useRef<HTMLInputElement>(null);

  // ── Filter state ─────────────────────────────────────────────────────────
  const [selectedYearId, setSelectedYearId] = useState<number | undefined>(undefined);
  const [selectedSideCode, setSelectedSideCode] = useState<string | undefined>(undefined);
  const [selectedSanatoryId, setSelectedSanatoryId] = useState<number | undefined>(undefined);
  const [selectedPravsoyuzId, setSelectedPravsoyuzId] = useState<string | undefined>(undefined);
  const [selectedFileForUpload, setSelectedFileForUpload] = useState<File | null>(null);

  // Year2 for the differ section
  const [selectedYear2Id, setSelectedYear2Id] = useState<number | undefined>(undefined);
  const [differData, setDifferData] = useState<DifferRecord[]>([]);

  // Main table data from the uploaded/processed file
  const [mainTableData, setMainTableData] = useState<MainTableRecord[]>([]);

  // ── Reference data queries ────────────────────────────────────────────────
  const { data: years = [] } = useQuery<Year[]>({
    queryKey: ['sverkadannix-years'],
    queryFn: () => api.get('/api/sverkadannix/years').then((r) => r.data),
  });

  const { data: sides = [] } = useQuery<Side[]>({
    queryKey: ['sverkadannix-sides'],
    queryFn: () => api.get('/api/sverkadannix/sides').then((r) => r.data),
  });

  const { data: sanatories = [] } = useQuery<Sanatory[]>({
    queryKey: ['sverkadannix-sanatories'],
    queryFn: () => api.get('/api/sverkadannix/sanatories').then((r) => r.data),
  });

  const { data: pravs = [] } = useQuery<Profsouz[]>({
    queryKey: ['sverkadannix-pravs', selectedSideCode],
    queryFn: () =>
      api
        .get('/api/sverkadannix/profsouzy', { params: { sideCode: selectedSideCode } })
        .then((r) => r.data),
    enabled: !!selectedSideCode,
  });

  // ── Upload mutation ───────────────────────────────────────────────────────
  const uploadMutation = useMutation({
    mutationFn: async () => {
      if (!selectedFileForUpload) throw new Error('Файл не выбран');
      const formData = new FormData();
      formData.append('UploadedFile', selectedFileForUpload);
      if (selectedYearId !== undefined) formData.append('yearId', String(selectedYearId));
      if (selectedSideCode) formData.append('sideCode', selectedSideCode);
      if (selectedSanatoryId !== undefined) formData.append('sanatoryId', String(selectedSanatoryId));
      if (selectedPravsoyuzId) formData.append('pravsoyuzId', selectedPravsoyuzId);
      const res = await api.post('/api/sverkadannix/upload', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return res.data as MainTableRecord[];
    },
    onSuccess: (data) => {
      setMainTableData(data);
      message.success('Данные успешно загружены');
    },
    onError: () => {
      message.error('Ошибка при загрузке данных');
    },
  });

  // ── Clear mutation ────────────────────────────────────────────────────────
  const clearMutation = useMutation({
    mutationFn: () => api.post('/api/sverkadannix/clear').then((r) => r.data),
    onSuccess: () => {
      setMainTableData([]);
      setSelectedFileForUpload(null);
      if (fileInputRef.current) fileInputRef.current.value = '';
      message.success('Данные очищены');
    },
    onError: () => {
      message.error('Ошибка при очистке данных');
    },
  });

  // ── Differ query ─────────────────────────────────────────────────────────
  const differMutation = useMutation({
    mutationFn: () =>
      api
        .get('/api/sverkadannix/differ', { params: { yearId: selectedYear2Id } })
        .then((r) => r.data as DifferRecord[]),
    onSuccess: (data) => {
      setDifferData(data);
    },
    onError: () => {
      message.error('Ошибка при получении несоответствий');
    },
  });

  // ── Export mutation ───────────────────────────────────────────────────────
  const exportMutation = useMutation({
    mutationFn: () =>
      api
        .get('/api/sverkadannix/export', { responseType: 'blob' })
        .then((r) => r.data as Blob),
    onSuccess: (blob) => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'sverkadannix_export.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    },
    onError: () => {
      message.error('Ошибка при экспорте данных');
    },
  });

  // ── Template download ─────────────────────────────────────────────────────
  const handleDownloadTemplate = async () => {
    try {
      const res = await api.get('/api/prixod-putevok/download-template', { responseType: 'blob' });
      const url = window.URL.createObjectURL(res.data);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'template.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка при скачивании шаблона');
    }
  };

  // ── Handlers ──────────────────────────────────────────────────────────────
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] ?? null;
    setSelectedFileForUpload(file);
  };

  const handleSaveData = () => {
    if (!selectedFileForUpload) {
      message.warning('Выберите файл для загрузки');
      return;
    }
    uploadMutation.mutate();
  };

  const handleClear = () => {
    clearMutation.mutate();
  };

  const handleShowDiffer = () => {
    if (!selectedYear2Id) {
      message.warning('Выберите год');
      return;
    }
    differMutation.mutate();
  };

  // ── Highlight helper ──────────────────────────────────────────────────────
  const highlightText = (text: string, compare: string) => {
    if (!text || !compare || text.trim().toLowerCase() === compare.trim().toLowerCase()) {
      return <span>{text}</span>;
    }
    return <span style={{ background: '#ff0000', color: '#fff' }}>{text}</span>;
  };

  // ── Differ table columns ──────────────────────────────────────────────────
  const differColumns: ColumnsType<DifferRecord> = [
    {
      title: 'Санаторий',
      dataIndex: 'SanatName',
      key: 'SanatName',
      width: 130,
    },
    {
      title: 'Номера путевок',
      dataIndex: 'NumPut',
      key: 'NumPut',
      width: 120,
    },
    {
      title: 'Санаторий ФИО',
      dataIndex: 'KurortFio',
      key: 'KurortFio',
      width: 160,
    },
    {
      title: 'Санаторий Адрес',
      dataIndex: 'KurortAdress',
      key: 'KurortAdress',
      width: 160,
    },
    {
      title: 'Профсоюз ФИО',
      dataIndex: 'OblastFio',
      key: 'OblastFio',
      width: 160,
      render: (val: string, record: DifferRecord) => highlightText(val, record.KurortFio),
    },
    {
      title: 'Профсоюз Адрес',
      dataIndex: 'OblastAdress',
      key: 'OblastAdress',
      width: 160,
    },
  ];

  // ── Main table columns ────────────────────────────────────────────────────
  const mainColumns: ColumnsType<MainTableRecord> = [
    {
      title: '№ путевки',
      dataIndex: 'numPut',
      key: 'numPut',
      width: 100,
    },
    {
      title: 'Санаторий',
      dataIndex: 'sanatName',
      key: 'sanatName',
      width: 130,
    },
    {
      title: 'Дата Санатория',
      dataIndex: 'dataSanat',
      key: 'dataSanat',
      width: 120,
    },
    {
      title: 'Информация об отдыхающем',
      dataIndex: 'infoOtdixayushego',
      key: 'infoOtdixayushego',
      width: 200,
      render: (val: string, record: MainTableRecord) => (
        <span>
          <div>{val}</div>
          {record.infoOtdixayushegoProfsouz && record.infoOtdixayushegoProfsouz !== val && (
            <Tag color="red" style={{ marginTop: 2 }}>
              Профсоюз: {record.infoOtdixayushegoProfsouz}
            </Tag>
          )}
        </span>
      ),
    },
    {
      title: 'Адрес (Санаторий)',
      dataIndex: 'adresSanatory',
      key: 'adresSanatory',
      width: 160,
    },
    {
      title: 'Адрес (Профсоюз)',
      dataIndex: 'adresProfsouz',
      key: 'adresProfsouz',
      width: 160,
      render: (val: string, record: MainTableRecord) =>
        highlightText(val, record.adresSanatory),
    },
    {
      title: 'Тип документа (Сан.)',
      dataIndex: 'tipDokumenta',
      key: 'tipDokumenta',
      width: 140,
    },
    {
      title: 'Тип документа (Проф.)',
      dataIndex: 'tipDokumentaProfsouz',
      key: 'tipDokumentaProfsouz',
      width: 150,
      render: (val: string, record: MainTableRecord) =>
        highlightText(val, record.tipDokumenta),
    },
    {
      title: 'Серия/Номер (Сан.)',
      dataIndex: 'seriaNomer',
      key: 'seriaNomer',
      width: 140,
    },
    {
      title: 'Серия/Номер (Проф.)',
      dataIndex: 'seriaNomerProfsouz',
      key: 'seriaNomerProfsouz',
      width: 150,
      render: (val: string, record: MainTableRecord) =>
        highlightText(val, record.seriaNomer),
    },
    {
      title: 'Дата выдачи (Сан.)',
      dataIndex: 'dataVidachi',
      key: 'dataVidachi',
      width: 140,
    },
    {
      title: 'Дата выдачи (Проф.)',
      dataIndex: 'dataVidachiProfsouz',
      key: 'dataVidachiProfsouz',
      width: 150,
      render: (val: string, record: MainTableRecord) =>
        highlightText(val, record.dataVidachi),
    },
    {
      title: 'Кем выдан (Сан.)',
      dataIndex: 'kimTomonidan',
      key: 'kimTomonidan',
      width: 160,
    },
    {
      title: 'Кем выдан (Проф.)',
      dataIndex: 'kimTomonidanProfsouz',
      key: 'kimTomonidanProfsouz',
      width: 160,
      render: (val: string, record: MainTableRecord) =>
        highlightText(val, record.kimTomonidan),
    },
  ];

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <div>
      {/* Header */}
      <Row className="mb-4" align="middle">
        <Col flex="auto">
          <Title level={4} style={{ margin: 0 }}>
            Сверка данных по отдыхающим
          </Title>
          <Breadcrumb
            style={{ marginTop: 4 }}
            items={[
              { title: <a href="/PrixodPutevok">Путевки</a> },
              { title: 'Сверка данных по отдыхающим' },
            ]}
          />
        </Col>
      </Row>

      {/* Filters row */}
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <label>Год</label>
          </div>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите год"
            value={selectedYearId}
            onChange={(val) => setSelectedYearId(val)}
            allowClear
          >
            {years.map((y) => (
              <Option key={y.Id} value={y.Id}>
                {y.Year}
              </Option>
            ))}
          </Select>
        </Col>

        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <label>Сторона загрузки</label>
          </div>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите сторону"
            value={selectedSideCode}
            onChange={(val) => {
              setSelectedSideCode(val);
              setSelectedPravsoyuzId(undefined);
            }}
            allowClear
          >
            {sides.map((s) => (
              <Option key={s.Code} value={s.Code}>
                {s.Decode}
              </Option>
            ))}
          </Select>
        </Col>

        <Col xs={24} sm={12} md={6}>
          <div style={{ marginBottom: 4 }}>
            <label>Санаторий</label>
          </div>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите санаторий"
            value={selectedSanatoryId}
            onChange={(val) => setSelectedSanatoryId(val)}
            allowClear
          >
            {sanatories.map((s) => (
              <Option key={s.ID} value={s.ID}>
                {s.Name_San}
              </Option>
            ))}
          </Select>
        </Col>

        {selectedSideCode && (
          <Col xs={24} sm={12} md={6}>
            <div style={{ marginBottom: 4 }}>
              <label>Профсоюз</label>
            </div>
            <Select
              style={{ width: '100%' }}
              placeholder="Выберите профсоюз"
              value={selectedPravsoyuzId}
              onChange={(val) => setSelectedPravsoyuzId(val)}
              allowClear
            >
              {pravs.map((p) => (
                <Option key={p.MATCH_DEPARTMENTBASE} value={p.MATCH_DEPARTMENTBASE}>
                  {p.ProfsoyzNaimenovanie}
                </Option>
              ))}
            </Select>
          </Col>
        )}
      </Row>

      {/* Template download + File upload */}
      <Row style={{ marginBottom: 16 }} align="middle" gutter={16}>
        <Col>
          <Button
            icon={<DownloadOutlined />}
            onClick={handleDownloadTemplate}
          >
            Загрузить Шаблон
          </Button>
        </Col>
      </Row>

      <Row style={{ marginBottom: 24 }} align="middle" gutter={12}>
        <Col>
          <input
            ref={fileInputRef}
            type="file"
            id="input08"
            name="UploadedFile"
            accept=".xlsx,.xls,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel"
            onChange={handleFileChange}
            style={{ maxWidth: 300 }}
          />
        </Col>
        <Col>
          <Space>
            <Button
              type="primary"
              icon={<UploadOutlined />}
              disabled={!selectedFileForUpload}
              loading={uploadMutation.isPending}
              onClick={handleSaveData}
            >
              Загрузить
            </Button>
            <Button
              icon={<ClearOutlined />}
              loading={clearMutation.isPending}
              onClick={handleClear}
            >
              Очистить
            </Button>
          </Space>
        </Col>
      </Row>

      <Divider />

      {/* Differ section */}
      <Row style={{ marginBottom: 24 }} gutter={16} align="middle">
        <Col xs={24} sm={8} md={6}>
          <Select
            style={{ width: '100%' }}
            placeholder="Выберите год"
            value={selectedYear2Id}
            onChange={(val) => setSelectedYear2Id(val)}
            allowClear
          >
            {years.map((y) => (
              <Option key={y.Id} value={y.Id}>
                {y.Year}
              </Option>
            ))}
          </Select>
        </Col>
        <Col>
          <Button
            type="primary"
            icon={<SearchOutlined />}
            loading={differMutation.isPending}
            onClick={handleShowDiffer}
          >
            Показать несоответствия
          </Button>
        </Col>
      </Row>

      {/* Differ results table */}
      {differData.length > 0 && (
        <div style={{ marginBottom: 32 }}>
          <Row style={{ marginBottom: 8 }} align="middle">
            <Col flex="auto">
              <Typography.Text strong>Несоответствия</Typography.Text>
            </Col>
            <Col>
              <Button
                icon={<ExportOutlined />}
                loading={exportMutation.isPending}
                onClick={() => exportMutation.mutate()}
              >
                Экспорт
              </Button>
            </Col>
          </Row>
          <div style={{ overflowX: 'auto', boxShadow: '0 1px 4px rgba(0,0,0,0.12)', marginBottom: 16 }}>
            <Table<DifferRecord>
              columns={differColumns}
              dataSource={differData}
              rowKey={(r) => `${r.NumPut}-${r.SanatName}`}
              size="small"
              pagination={{ pageSize: 20 }}
              scroll={{ x: 'max-content' }}
            />
          </div>
        </div>
      )}

      {/* Main data table */}
      {mainTableData.length > 0 && (
        <div style={{ marginBottom: 32 }}>
          <Typography.Text strong style={{ display: 'block', marginBottom: 8 }}>
            Данные по отдыхающим
          </Typography.Text>
          <div style={{ overflowX: 'auto', boxShadow: '0 1px 4px rgba(0,0,0,0.12)' }}>
            <Table<MainTableRecord>
              columns={mainColumns}
              dataSource={mainTableData}
              rowKey="id"
              size="small"
              pagination={{ pageSize: 20 }}
              scroll={{ x: 'max-content' }}
            />
          </div>
        </div>
      )}
    </div>
  );
}
