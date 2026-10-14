import { useState } from 'react';
import {
  Button, Table, Space, Modal, Form, Input, Tabs, Typography, App, Upload, Descriptions, DatePicker,
} from 'antd';
import type { UploadFile } from 'antd';
import { UploadOutlined, RollbackOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { QueryErrorAlert } from '../../components/QueryErrorAlert';
import api from '../../api/axios';
import dayjs from 'dayjs';

const { TabPane } = Tabs;
const { TextArea } = Input;
const { Title } = Typography;

// ─── Types ────────────────────────────────────────────────────────────────────

interface RealzPutevokRow {
  id: number;
  numPut: string;
  sanat: string;
  dataNach: string;
  dataKon: string;
  price: number;
  kolvo: number;
  obshayaStoimost: number;
}

interface RealzPutevokDetail {
  id: number;
  // Voucher info
  numPut: string;
  sanat: string;
  dataNach: string;
  dataKon: string;
  dataRealizacii: string;
  poluchenoOt: string;
  price: number;
  oplachenoProcent: number;
  nakladnaya: string;
  summaOplaty: number;
  // Org + employee
  kodOrg: string;
  innOrg: string;
  otrasl: string;
  nazvOrg: string;
  fio: string;
  dolzhnost: string;
  innSotr: string;
  primechanie: string;
  talon: string;
  // Doverenost
  poluchatel: string;
  nomDov: string;
  dateDov: string;
  srokIst: string;
}

// ─── Component ────────────────────────────────────────────────────────────────

export function ProsmotrRealzPutevokPage() {
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  // Detail modal state
  const [detailOpen, setDetailOpen] = useState(false);
  const [selectedId, setSelectedId] = useState<number | null>(null);

  // Vozvrat modal state
  const [vozvratOpen, setVozvratOpen] = useState(false);
  const [vozvratFileList, setVozvratFileList] = useState<UploadFile[]>([]);
  const [prichina, setPrichina] = useState('');

  // Doverenost file upload state
  const [dovFileList, setDovFileList] = useState<UploadFile[]>([]);

  // Forms
  const [talonForm] = Form.useForm();
  const [dovForm] = Form.useForm();

  // ─── Queries ──────────────────────────────────────────────────────────────

  const { data: rows = [], isLoading, isError, refetch } = useQuery<RealzPutevokRow[]>({
    queryKey: ['prosmotr-realz-putevok'],
    queryFn: () => api.get('/api/prosmotr-realz-putevok').then(r => r.data),
  });

  const { data: detail, isLoading: detailLoading } = useQuery<RealzPutevokDetail>({
    queryKey: ['prosmotr-realz-putevok', selectedId],
    queryFn: () => api.get(`/api/prosmotr-realz-putevok/${selectedId}`).then(r => r.data),
    enabled: selectedId !== null && detailOpen,
  });

  // ─── Mutations ────────────────────────────────────────────────────────────

  const talonMutation = useMutation({
    mutationFn: (values: { talon: string }) =>
      api.put(`/api/prosmotr-realz-putevok/${selectedId}/talon`, values).then(r => r.data),
    onSuccess: () => {
      message.success('Талон сохранён');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok', selectedId] });
    },
    onError: () => message.error('Ошибка при сохранении талона'),
  });

  const dovMutation = useMutation({
    mutationFn: (values: { poluchatel: string; nomDov: string; dateDov: string; srokIst: string }) =>
      api.put(`/api/prosmotr-realz-putevok/${selectedId}/doverenost`, values).then(r => r.data),
    onSuccess: () => {
      message.success('Доверенность сохранена');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok', selectedId] });
    },
    onError: () => message.error('Ошибка при сохранении доверенности'),
  });

  const vozvratMutation = useMutation({
    mutationFn: ({ file, prichina }: { file: File; prichina: string }) => {
      const fd = new FormData();
      fd.append('file', file);
      fd.append('prichina', prichina);
      return api.post(`/api/prosmotr-realz-putevok/${selectedId}/vozvrat`, fd, {
        headers: { 'Content-Type': 'multipart/form-data' },
      }).then(r => r.data);
    },
    onSuccess: () => {
      message.success('Путевка возвращена');
      setVozvratOpen(false);
      setVozvratFileList([]);
      setPrichina('');
      queryClient.invalidateQueries({ queryKey: ['prosmotr-realz-putevok'] });
      setDetailOpen(false);
    },
    onError: () => message.error('Ошибка при возврате путевки'),
  });

  // ─── Handlers ─────────────────────────────────────────────────────────────

  const openDetail = (rec: RealzPutevokRow) => {
    setSelectedId(rec.id);
    talonForm.resetFields();
    dovForm.resetFields();
    setDovFileList([]);
    setDetailOpen(true);
  };

  const closeDetail = () => {
    setDetailOpen(false);
    setSelectedId(null);
  };

  const handleTalonSave = (values: { talon: string }) => {
    talonMutation.mutate(values);
  };

  const handleDovSave = (values: { poluchatel: string; nomDov: string; dateDov: dayjs.Dayjs; srokIst: dayjs.Dayjs }) => {
    dovMutation.mutate({
      poluchatel: values.poluchatel,
      nomDov: values.nomDov,
      dateDov: values.dateDov?.toISOString() ?? '',
      srokIst: values.srokIst?.toISOString() ?? '',
    });
  };

  const handleVozvratSubmit = () => {
    if (vozvratFileList.length === 0) {
      message.warning('Прикрепите файл');
      return;
    }
    if (!prichina.trim()) {
      message.warning('Укажите причину возврата');
      return;
    }
    const file = vozvratFileList[0].originFileObj as File;
    vozvratMutation.mutate({ file, prichina });
  };

  // Populate forms when detail loads
  const handleDetailAfterOpen = () => {
    if (detail) {
      talonForm.setFieldsValue({ talon: detail.talon });
      dovForm.setFieldsValue({
        poluchatel: detail.poluchatel,
        nomDov: detail.nomDov,
        dateDov: detail.dateDov ? dayjs(detail.dateDov) : undefined,
        srokIst: detail.srokIst ? dayjs(detail.srokIst) : undefined,
      });
    }
  };

  // ─── Table columns ────────────────────────────────────────────────────────

  const columns = [
    {
      title: '№',
      key: 'index',
      width: 55,
      render: (_: unknown, __: RealzPutevokRow, idx: number) => idx + 1,
    },
    {
      title: 'Номер путевок',
      dataIndex: 'numPut',
      key: 'numPut',
    },
    {
      title: 'Санаторий',
      dataIndex: 'sanat',
      key: 'sanat',
    },
    {
      title: 'Период',
      key: 'period',
      render: (_: unknown, rec: RealzPutevokRow) => {
        const from = rec.dataNach ? dayjs(rec.dataNach).format('DD.MM.YYYY') : '';
        const to = rec.dataKon ? dayjs(rec.dataKon).format('DD.MM.YYYY') : '';
        return from && to ? `${from} — ${to}` : from || to;
      },
    },
    {
      title: 'Цена',
      dataIndex: 'price',
      key: 'price',
      align: 'right' as const,
      render: (v: number) =>
        v != null ? v.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
    {
      title: 'Кол-во',
      dataIndex: 'kolvo',
      key: 'kolvo',
      align: 'right' as const,
    },
    {
      title: 'Общая стоимость',
      dataIndex: 'obshayaStoimost',
      key: 'obshayaStoimost',
      align: 'right' as const,
      render: (v: number) =>
        v != null ? v.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
  ];

  // ─── Detail modal tabs ────────────────────────────────────────────────────

  const tab1 = (
    <Descriptions bordered column={1} size="small">
      <Descriptions.Item label="Номер путевки">{detail?.numPut}</Descriptions.Item>
      <Descriptions.Item label="Санаторий">{detail?.sanat}</Descriptions.Item>
      <Descriptions.Item label="Дата начала">
        {detail?.dataNach ? dayjs(detail.dataNach).format('DD.MM.YYYY') : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Дата конца">
        {detail?.dataKon ? dayjs(detail.dataKon).format('DD.MM.YYYY') : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Дата реализации">
        {detail?.dataRealizacii ? dayjs(detail.dataRealizacii).format('DD.MM.YYYY') : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Получено от">{detail?.poluchenoOt}</Descriptions.Item>
      <Descriptions.Item label="Цена">
        {detail?.price != null
          ? detail.price.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : ''}
      </Descriptions.Item>
      <Descriptions.Item label="Оплачено, %">{detail?.oplachenoProcent}</Descriptions.Item>
      <Descriptions.Item label="Накладная">{detail?.nakladnaya}</Descriptions.Item>
      <Descriptions.Item label="Сумма оплаты">
        {detail?.summaOplaty != null
          ? detail.summaOplaty.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
          : ''}
      </Descriptions.Item>
    </Descriptions>
  );

  const tab2 = (
    <div>
      <Descriptions bordered column={1} size="small" style={{ marginBottom: 16 }}>
        <Descriptions.Item label="Код организации">{detail?.kodOrg}</Descriptions.Item>
        <Descriptions.Item label="ИНН организации">{detail?.innOrg}</Descriptions.Item>
        <Descriptions.Item label="Отрасль">{detail?.otrasl}</Descriptions.Item>
        <Descriptions.Item label="Название организации">{detail?.nazvOrg}</Descriptions.Item>
        <Descriptions.Item label="ФИО">{detail?.fio}</Descriptions.Item>
        <Descriptions.Item label="Должность">{detail?.dolzhnost}</Descriptions.Item>
        <Descriptions.Item label="ИНН сотрудника">{detail?.innSotr}</Descriptions.Item>
        <Descriptions.Item label="Примечание">{detail?.primechanie}</Descriptions.Item>
      </Descriptions>
      <Form form={talonForm} layout="inline" onFinish={handleTalonSave}>
        <Form.Item name="talon" label="Талон" style={{ flex: 1 }}>
          <Input placeholder="Талон" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={talonMutation.isPending}>
            Сохранить
          </Button>
        </Form.Item>
      </Form>
    </div>
  );

  const tab3 = (
    <Form form={dovForm} layout="vertical" onFinish={handleDovSave}>
      <Form.Item name="poluchatel" label="Получатель">
        <Input placeholder="Получатель" />
      </Form.Item>
      <Form.Item name="nomDov" label="Номер доверенности">
        <Input placeholder="Номер доверенности" />
      </Form.Item>
      <Form.Item name="dateDov" label="Дата">
        <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
      </Form.Item>
      <Form.Item name="srokIst" label="Срок истечения">
        <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
      </Form.Item>
      <Form.Item label="Файл (PDF)">
        <Upload
          fileList={dovFileList}
          accept=".pdf"
          beforeUpload={() => false}
          onChange={({ fileList }) => setDovFileList(fileList)}
          maxCount={1}
        >
          <Button icon={<UploadOutlined />}>Загрузить PDF</Button>
        </Upload>
      </Form.Item>
      <Form.Item>
        <Button type="primary" htmlType="submit" loading={dovMutation.isPending}>
          Сохранить
        </Button>
      </Form.Item>
    </Form>
  );

  // ─── Render ───────────────────────────────────────────────────────────────

  return (
    <div style={{ padding: 24 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>
          Просмотр реализованных путевок
        </Title>
      </div>

      <QueryErrorAlert isError={isError} onRetry={refetch} />
      <Table<RealzPutevokRow>
        rowKey="id"
        columns={columns}
        dataSource={rows}
        loading={isLoading}
        size="small"
        bordered
        pagination={{ pageSize: 20, showSizeChanger: true, showTotal: total => `Всего: ${total}` }}
        onRow={rec => ({ onClick: () => openDetail(rec), style: { cursor: 'pointer' } })}
        scroll={{ x: 'max-content' }}
      />

      {/* ── Detail modal ─────────────────────────────────────────────────── */}
      <Modal
        title={`Путевка: ${detail?.numPut ?? '...'}`}
        open={detailOpen}
        onCancel={closeDetail}
        footer={
          <Space>
            <Button
              danger
              icon={<RollbackOutlined />}
              onClick={() => setVozvratOpen(true)}
            >
              Возврат путевки
            </Button>
            <Button onClick={closeDetail}>Закрыть</Button>
          </Space>
        }
        width={720}
        afterOpenChange={open => { if (open) handleDetailAfterOpen(); }}
        destroyOnClose
      >
        {detailLoading ? (
          <div style={{ textAlign: 'center', padding: 32 }}>Загрузка...</div>
        ) : (
          <Tabs defaultActiveKey="1">
            <TabPane tab="Информация о путевке" key="1">
              {tab1}
            </TabPane>
            <TabPane tab="Организация и сотрудник" key="2">
              {tab2}
            </TabPane>
            <TabPane tab="Доверенность" key="3">
              {tab3}
            </TabPane>
          </Tabs>
        )}
      </Modal>

      {/* ── Vozvrat nested modal ─────────────────────────────────────────── */}
      <Modal
        title="Возврат путевки"
        open={vozvratOpen}
        onCancel={() => {
          setVozvratOpen(false);
          setVozvratFileList([]);
          setPrichina('');
        }}
        footer={
          <Space>
            <Button
              onClick={() => {
                setVozvratOpen(false);
                setVozvratFileList([]);
                setPrichina('');
              }}
            >
              Отмена
            </Button>
            <Button
              type="primary"
              danger
              loading={vozvratMutation.isPending}
              onClick={handleVozvratSubmit}
            >
              Подтвердить возврат
            </Button>
          </Space>
        }
        width={480}
        destroyOnClose
      >
        <div style={{ marginBottom: 16 }}>
          <div style={{ marginBottom: 8, fontWeight: 500 }}>Причина возврата</div>
          <TextArea
            rows={4}
            value={prichina}
            onChange={e => setPrichina(e.target.value)}
            placeholder="Укажите причину возврата..."
          />
        </div>
        <div>
          <div style={{ marginBottom: 8, fontWeight: 500 }}>Прикрепить документ</div>
          <Upload
            fileList={vozvratFileList}
            beforeUpload={() => false}
            onChange={({ fileList }) => setVozvratFileList(fileList)}
            maxCount={1}
          >
            <Button icon={<UploadOutlined />}>Загрузить файл</Button>
          </Upload>
        </div>
      </Modal>
    </div>
  );
}
