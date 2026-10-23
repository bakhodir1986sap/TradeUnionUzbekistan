import { useState, useEffect } from 'react';
import {
  Table,
  Button,
  Select,
  Space,
  App,
  Typography,
  Breadcrumb,
  Card,
  Row,
  Col,
  Form,
  Upload,
  Flex,
} from 'antd';
import {
  FileExcelOutlined,
  UploadOutlined,
  ClearOutlined,
  SearchOutlined,
  DownloadOutlined,
} from '@ant-design/icons';
import { useQuery, useMutation } from '@tanstack/react-query';
import type { ColumnType } from 'antd/es/table';
import type { UploadFile } from 'antd/es/upload';
import api from '../../api/axios';

const { Title } = Typography;

// ---- Interfaces ----

interface YearItem {
  Id: number;
  Year: string;
}

interface SideItem {
  Code: string;
  Decode: string;
}

interface SanatoryItem {
  ID: number;
  Name_San: string;
}

interface ProfItem {
  MATCH_DEPARTMENTBASE: string;
  ProfsoyzNaimenovanie: string;
}

interface SverkaResultItem {
  _key?: string;
  sanatName: string;
  numPut: string;
  kurortFio: string;
  kurortAdress: string;
  oblastFio: string;
  oblastAdress: string;
}

interface LoadedDataItem {
  _key?: string;
  num_put: string;
  sanatName: string;
  date_san: string;
  fio_otdix: string;
  price: string;
  work_place: string;
}

interface YearComboItem {
  year_id: string;
  year_str: string;
}

interface CodeDecodeItem {
  text: string;
  value: string;
}

// ---- Component ----

export function SverkaDannixV2Page() {
  const { message } = App.useApp();

  // Upload section state
  const [selectedYear, setSelectedYear] = useState<YearItem | null>(null);
  const [selectedSide, setSelectedSide] = useState<SideItem | null>(null);
  const [selectedSanatory, setSelectedSanatory] = useState<SanatoryItem | null>(null);
  const [selectedProfsoyz, setSelectedProfsoyz] = useState<ProfItem | null>(null);
  const [fileList, setFileList] = useState<UploadFile[]>([]);

  // Sverka filter state
  const [sverkaYear, setSverkaYear] = useState<string | undefined>(undefined);
  const [sverskaSanatory, setsverskaSanatory] = useState<string | undefined>(undefined);
  const [sverkaProfsoyuz, setSverkaProfsoyuz] = useState<string | undefined>(undefined);
  const [sverkaResults, setSverkaResults] = useState<SverkaResultItem[]>([]);
  const [sverkaTriggered, setSverkaTriggered] = useState(false);

  // Loaded data filter state
  const [loadedYear, setLoadedYear] = useState<string | undefined>(undefined);
  const [loadedData, setLoadedData] = useState<LoadedDataItem[]>([]);
  const [loadedTriggered, setLoadedTriggered] = useState(false);


  const { data: yearsData } = useQuery<YearItem[]>({
    queryKey: ['sverkaDannixYears'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/years');
      return response.data;
    },
  });

  const { data: sidesData } = useQuery<SideItem[]>({
    queryKey: ['sverkaDannixSides'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/sides');
      return response.data;
    },
  });

  const { data: sanatoriesData } = useQuery<SanatoryItem[]>({
    queryKey: ['sverkaDannixSanatories'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/sanatories');
      return response.data;
    },
  });

  const { data: profsoyuzyData } = useQuery<ProfItem[]>({
    queryKey: ['sverkaDannixPravs', selectedSide?.Code],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/profsoyuzy', {
        params: { sideCode: selectedSide?.Code },
      });
      return response.data;
    },
    enabled: !!selectedSide?.Code,
  });

  const { data: yearComboData } = useQuery<YearComboItem[]>({
    queryKey: ['sverkaDannixYearsCombo'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/years-list');
      return response.data;
    },
  });

  const { data: sanatoryComboData } = useQuery<CodeDecodeItem[]>({
    queryKey: ['sverkaDannixSanatoryCombo'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/sanatory-list');
      return response.data;
    },
  });

  const { data: profComboData } = useQuery<CodeDecodeItem[]>({
    queryKey: ['sverkaDannixProfCombo'],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/prof-list');
      return response.data;
    },
  });

  // ---- Sverka results query (triggered) ----

  const { isFetching: isSverkaLoading, error: sverkaError } = useQuery<SverkaResultItem[]>({
    queryKey: ['sverkaDannixResults', sverkaYear, sverskaSanatory, sverkaProfsoyuz],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/GetDifferByYearFilesV2', {
        params: {
          year: sverkaYear,
          sanatory: sverskaSanatory,
          profsoyuz: sverkaProfsoyuz,
        },
      });
      const data: SverkaResultItem[] = response.data.map(
        (r: SverkaResultItem, i: number) => ({ ...r, _key: `${r.numPut || i}-${i}` })
      );
      setSverkaResults(data);
      return data;
    },
    enabled: sverkaTriggered && !!sverkaYear,
  });

  useEffect(() => {
    if (sverkaError) message.error('Ошибка при загрузке сверки данных');
  }, [sverkaError, message]);

  // ---- Loaded data query (triggered) ----

  const { isFetching: isLoadedDataLoading, error: loadedError } = useQuery<LoadedDataItem[]>({
    queryKey: ['sverkaDannixLoadedData', loadedYear],
    queryFn: async () => {
      const response = await api.get('/api/sverkadannixv2/GetLoadedRowsV2', {
        params: { year: loadedYear },
      });
      const data: LoadedDataItem[] = response.data.map(
        (r: LoadedDataItem, i: number) => ({ ...r, _key: `${r.num_put || i}-${i}` })
      );
      setLoadedData(data);
      return data;
    },
    enabled: loadedTriggered && !!loadedYear,
  });

  useEffect(() => {
    if (loadedError) message.error('Ошибка при загрузке загруженных данных');
  }, [loadedError, message]);

  // ---- Mutations ----

  const uploadMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append('UploadedFile', file);
      if (selectedYear) formData.append('yearId', String(selectedYear.Id));
      if (selectedSide) formData.append('sideCode', selectedSide.Code);
      if (selectedSanatory) formData.append('sanatoryId', String(selectedSanatory.ID));
      if (selectedProfsoyz)
        formData.append('profsoyzId', selectedProfsoyz.MATCH_DEPARTMENTBASE);
      const response = await api.post('/api/sverkadannixv2/upload', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return response.data;
    },
    onSuccess: () => {
      message.success('Файл успешно загружен');
      setFileList([]);
    },
    onError: () => {
      message.error('Ошибка при загрузке файла');
    },
  });

  // ---- Handlers ----

  const handleSaveData = () => {
    if (fileList.length === 0 || !fileList[0].originFileObj) {
      message.warning('Пожалуйста, выберите файл для загрузки');
      return;
    }
    uploadMutation.mutate(fileList[0].originFileObj as File);
  };

  const handleClearOblast = () => {
    setSelectedYear(null);
    setSelectedSide(null);
    setSelectedSanatory(null);
    setSelectedProfsoyz(null);
    setFileList([]);
    message.info('Форма очищена');
  };

  const handleDownloadTemplate = async () => {
    try {
      const response = await api.get('/api/sverkadannixv2/download-template', {
        responseType: 'blob',
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'template.xlsx');
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch {
      message.error('Ошибка при скачивании шаблона');
    }
  };

  const handleShowSverka = () => {
    if (!sverkaYear) {
      message.warning('Выберите год для отображения несоответствий');
      return;
    }
    setSverkaTriggered(true);
  };

  const handleShowLoadedData = () => {
    if (!loadedYear) {
      message.warning('Выберите год для отображения загруженных данных');
      return;
    }
    setLoadedTriggered(true);
  };

  const handleExportSverka = async () => {
    try {
      const response = await api.get('/api/sverkadannixv2/export-sverka', {
        responseType: 'blob',
        params: {
          year: sverkaYear,
          sanatory: sverskaSanatory,
          profsoyuz: sverkaProfsoyuz,
        },
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'sverka_result.xlsx');
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
      message.success('Экспорт выполнен успешно');
    } catch {
      message.error('Ошибка при экспорте в Excel');
    }
  };

  const handleExportLoadedData = async () => {
    try {
      const response = await api.get('/api/sverkadannixv2/export-loaded', {
        responseType: 'blob',
        params: { year: loadedYear },
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', 'loaded_data.xlsx');
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
      message.success('Экспорт выполнен успешно');
    } catch {
      message.error('Ошибка при экспорте в Excel');
    }
  };

  // ---- Columns ----

  const sverkaColumns: ColumnType<SverkaResultItem>[] = [
    {
      title: 'Санаторий',
      dataIndex: 'sanatName',
      key: 'sanatName',
      ellipsis: true,
      width: 150,
    },
    {
      title: 'Номера путевок',
      dataIndex: 'numPut',
      key: 'numPut',
      ellipsis: true,
      width: 150,
    },
    {
      title: 'Санаторий ФИО',
      dataIndex: 'kurortFio',
      key: 'kurortFio',
      ellipsis: true,
      width: 200,
    },
    {
      title: 'Санаторий Адрес',
      dataIndex: 'kurortAdress',
      key: 'kurortAdress',
      ellipsis: true,
      width: 200,
    },
    {
      title: 'Профсоюз ФИО',
      dataIndex: 'oblastFio',
      key: 'oblastFio',
      ellipsis: true,
      width: 200,
    },
    {
      title: 'Профсоюз Адрес',
      dataIndex: 'oblastAdress',
      key: 'oblastAdress',
      ellipsis: true,
      width: 200,
    },
  ];

  const loadedDataColumns: ColumnType<LoadedDataItem>[] = [
    {
      title: '№ путевки',
      dataIndex: 'num_put',
      key: 'num_put',
      ellipsis: true,
      width: 100,
    },
    {
      title: 'Санаторий',
      dataIndex: 'sanatName',
      key: 'sanatName',
      ellipsis: true,
      width: 150,
    },
    {
      title: 'Дата Санатория',
      dataIndex: 'date_san',
      key: 'date_san',
      ellipsis: true,
      width: 130,
    },
    {
      title: 'Информация об отдыхающем',
      dataIndex: 'fio_otdix',
      key: 'fio_otdix',
      ellipsis: true,
      width: 250,
    },
    {
      title: 'Стоимость',
      dataIndex: 'price',
      key: 'price',
      ellipsis: true,
      width: 100,
    },
    {
      title: 'Место работы',
      dataIndex: 'work_place',
      key: 'work_place',
      ellipsis: true,
      width: 200,
    },
  ];

  // ---- Render ----

  return (
    <div style={{ padding: 24 }}>
      {/* Page Header */}
      <div style={{ marginBottom: 24 }}>
        <Title level={4} style={{ marginBottom: 4 }}>
          Сверка данных по отдыхающим
        </Title>
        <Breadcrumb
          items={[
            { title: <a href="/Home/Putyovki">Путевки</a> },
            { title: 'Сверка данных по отдыхающим' },
          ]}
        />
      </div>

      {/* Upload Section */}
      <Card style={{ marginBottom: 24 }}>
        <Title level={5} style={{ marginBottom: 16 }}>
          Загрузка данных
        </Title>

        {/* Upload Filters Row */}
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col xs={24} md={6}>
            <Form.Item label="Год" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите год"
                value={selectedYear?.Id}
                onChange={(val) => {
                  const found = yearsData?.find((y) => y.Id === val) ?? null;
                  setSelectedYear(found);
                }}
                options={(yearsData ?? []).map((y) => ({
                  label: y.Year,
                  value: y.Id,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Form.Item label="Сторона загрузки" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите сторону"
                value={selectedSide?.Code}
                onChange={(val) => {
                  const found = sidesData?.find((s) => s.Code === val) ?? null;
                  setSelectedSide(found);
                  setSelectedProfsoyz(null);
                }}
                options={(sidesData ?? []).map((s) => ({
                  label: s.Decode,
                  value: s.Code,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Form.Item label="Санаторий" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите санаторий"
                value={selectedSanatory?.ID}
                onChange={(val) => {
                  const found = sanatoriesData?.find((s) => s.ID === val) ?? null;
                  setSelectedSanatory(found);
                }}
                options={(sanatoriesData ?? []).map((s) => ({
                  label: s.Name_San,
                  value: s.ID,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          {selectedSide?.Code && (
            <Col xs={24} md={6}>
              <Form.Item label="Профсоюз" style={{ marginBottom: 0 }}>
                <Select
                  placeholder="Выберите профсоюз"
                  value={selectedProfsoyz?.MATCH_DEPARTMENTBASE}
                  onChange={(val) => {
                    const found =
                      profsoyuzyData?.find(
                        (p) => p.MATCH_DEPARTMENTBASE === val
                      ) ?? null;
                    setSelectedProfsoyz(found);
                  }}
                  options={(profsoyuzyData ?? []).map((p) => ({
                    label: p.ProfsoyzNaimenovanie,
                    value: p.MATCH_DEPARTMENTBASE,
                  }))}
                  allowClear
                  style={{ width: '100%' }}
                />
              </Form.Item>
            </Col>
          )}
        </Row>

        {/* Download Template & File Upload */}
        <Flex vertical gap={8} style={{ width: '100%' }}>
          <Button
            icon={<DownloadOutlined />}
            onClick={handleDownloadTemplate}
          >
            Загрузить Шаблон
          </Button>

          <Space wrap>
            <Upload
              accept=".xlsx,.xls"
              fileList={fileList}
              beforeUpload={() => false}
              onChange={({ fileList: fl }) => setFileList(fl.slice(-1))}
              maxCount={1}
            >
              <Button icon={<UploadOutlined />}>Выбрать файл</Button>
            </Upload>
            <Button
              type="primary"
              onClick={handleSaveData}
              disabled={fileList.length === 0}
              loading={uploadMutation.isPending}
            >
              Загрузить
            </Button>
            <Button
              icon={<ClearOutlined />}
              onClick={handleClearOblast}
            >
              Очистить
            </Button>
          </Space>
        </Flex>
      </Card>

      {/* Sverka Section */}
      <Card style={{ marginBottom: 24 }}>
        <Title level={5} style={{ marginBottom: 16 }}>
          Показать несоответствия
        </Title>
        <Row gutter={16} style={{ marginBottom: 16 }} align="bottom">
          <Col xs={24} md={6}>
            <Form.Item label="Год" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите год"
                value={sverkaYear}
                onChange={(val) => {
                  setSverkaYear(val);
                  setSverkaTriggered(false);
                }}
                options={(yearComboData ?? []).map((y) => ({
                  label: y.year_id,
                  value: y.year_str,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Form.Item label="Санаторий" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите санаторий"
                value={sverskaSanatory}
                onChange={(val) => {
                  setsverskaSanatory(val);
                  setSverkaTriggered(false);
                }}
                options={(sanatoryComboData ?? []).map((s) => ({
                  label: s.text,
                  value: s.value,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Form.Item label="Профсоюз" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите профсоюз"
                value={sverkaProfsoyuz}
                onChange={(val) => {
                  setSverkaProfsoyuz(val);
                  setSverkaTriggered(false);
                }}
                options={(profComboData ?? []).map((p) => ({
                  label: p.text,
                  value: p.value,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleShowSverka}
              loading={isSverkaLoading}
            >
              Показать несоответствия
            </Button>
          </Col>
        </Row>

        <Space style={{ marginBottom: 12 }}>
          <Button
            icon={<FileExcelOutlined />}
            onClick={handleExportSverka}
            disabled={sverkaResults.length === 0}
            style={
              sverkaResults.length > 0
                ? { backgroundColor: '#217346', borderColor: '#217346', color: '#fff' }
                : {}
            }
          >
            Экспорт в Excel
          </Button>
        </Space>

        <Table<SverkaResultItem>
          columns={sverkaColumns}
          dataSource={sverkaResults}
          rowKey={(record) => record._key ?? record.numPut}
          loading={isSverkaLoading}
          scroll={{ x: 'max-content' }}
          size="small"
          pagination={{ pageSize: 10, showSizeChanger: true }}
          bordered
        />
      </Card>

      {/* Loaded Data Section */}
      <Card>
        <Title level={5} style={{ marginBottom: 16 }}>
          Показать загруженные данные
        </Title>
        <Row gutter={16} style={{ marginBottom: 16 }} align="bottom">
          <Col xs={24} md={6}>
            <Form.Item label="Год" style={{ marginBottom: 0 }}>
              <Select
                placeholder="Выберите год"
                value={loadedYear}
                onChange={(val) => {
                  setLoadedYear(val);
                  setLoadedTriggered(false);
                }}
                options={(yearComboData ?? []).map((y) => ({
                  label: y.year_id,
                  value: y.year_str,
                }))}
                allowClear
                style={{ width: '100%' }}
              />
            </Form.Item>
          </Col>
          <Col xs={24} md={6}>
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleShowLoadedData}
              loading={isLoadedDataLoading}
            >
              Показать Загруженные Данные
            </Button>
          </Col>
        </Row>

        <Space style={{ marginBottom: 12 }}>
          <Button
            icon={<FileExcelOutlined />}
            onClick={handleExportLoadedData}
            disabled={loadedData.length === 0}
            style={
              loadedData.length > 0
                ? { backgroundColor: '#217346', borderColor: '#217346', color: '#fff' }
                : {}
            }
          >
            Экспорт в Excel
          </Button>
        </Space>

        <Table<LoadedDataItem>
          columns={loadedDataColumns}
          dataSource={loadedData}
          rowKey={(record) => record._key ?? record.num_put}
          loading={isLoadedDataLoading}
          scroll={{ x: 'max-content' }}
          size="small"
          pagination={{ pageSize: 10, showSizeChanger: true }}
          bordered
        />
      </Card>
    </div>
  );
}
