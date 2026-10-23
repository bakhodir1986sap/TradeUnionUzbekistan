import { useState, useEffect } from 'react';
import {
  Button, Table, Space, DatePicker, Row, Col, Typography, App,
  Form, Input, Select, Card, Divider, Spin,
} from 'antd';
import { SearchOutlined, SaveOutlined, UserOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
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
  inn?: string;
}

interface NakladnoyRow {
  idSan?: number;
  name_San?: string;
  kolPut: number;
  totalSymma?: number;
  dateFirst?: string;
  dateLast?: string;
}

interface ViplPutRow {
  id: number;
  num_Put?: string;
  idSan?: number;
  name_San?: string;
  idOrg?: number;
  name_Org?: string;
  dataRealizacii?: string;
  symma?: number;
  num_Doc?: string;
  dataNakladnoy?: string;
  isFaceExists: number;
}

interface NakladnoyResponse {
  rows: NakladnoyRow[];
  total: { kolPut: number; totalSymma: number };
}

interface ReferenceItem {
  id: number;
  name: string;
}

interface OwnerData {
  faceMain: {
    id: number;
    lastName?: string;
    name?: string;
    firstName?: string;
    pol?: string;
    adress?: string;
    rabota?: string;
    dolj?: string;
    id_Osnov?: number;
    stav_Vid?: number;
    gost_Pr?: number;
    pas_Ser?: string;
    pas_Num?: string;
    pas_KD?: string;
    pas_Data?: string;
    inn?: string;
    pnfl?: string;
  } | null;
  faceMed: {
    id: number;
    med_Doc?: string;
    med_Data?: string;
    med_San?: number;
    opl_Doc?: string;
    opl_Data?: string;
    opl_Symma?: number;
    tal_Doc?: string;
    tal_Data?: string;
  } | null;
}

// ─── Main Page ────────────────────────────────────────────────────────────────

export function DanniyeVladelsaxPutyovokPage() {
  const { message } = App.useApp();
  const queryClient = useQueryClient();
  const [ownerForm] = Form.useForm();

  // Filter state
  const [beginDate, setBeginDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [selectedOrg, setSelectedOrg] = useState<Organization | null>(null);
  const [orgPickerOpen, setOrgPickerOpen] = useState(false);

  // Applied filters
  const [appliedBeginDate, setAppliedBeginDate] = useState<string>('');
  const [appliedEndDate, setAppliedEndDate] = useState<string>('');
  const [appliedOrgId, setAppliedOrgId] = useState<number | null>(null);
  const [queryEnabled, setQueryEnabled] = useState(false);

  // Table selection
  const [selectedSan, setSelectedSan] = useState<NakladnoyRow | null>(null);
  const [selectedVoucher, setSelectedVoucher] = useState<ViplPutRow | null>(null);

  // ── Reference queries (for dropdowns)
  const { data: refFace = [] } = useQuery<ReferenceItem[]>({
    queryKey: ['ref-face'],
    queryFn: () => api.get('/api/tbl-sprav-face').then(r =>
      (r.data as { id: number; name_Face?: string }[]).map(x => ({ id: x.id, name: x.name_Face ?? '' }))
    ),
  });

  const { data: refOs = [] } = useQuery<ReferenceItem[]>({
    queryKey: ['ref-os'],
    queryFn: () => api.get('/api/tbl-sprav-os').then(r =>
      (r.data as { id: number; name_Os?: string }[]).map(x => ({ id: x.id, name: x.name_Os ?? '' }))
    ),
  });

  const { data: refPr = [] } = useQuery<ReferenceItem[]>({
    queryKey: ['ref-pr'],
    queryFn: () => api.get('/api/tbl-sprav-pr').then(r =>
      (r.data as { id: number; name_Pr?: string }[]).map(x => ({ id: x.id, name: x.name_Pr ?? '' }))
    ),
  });

  // ── Nakladnoy query (left table)
  const {
    data: nakladnoyData,
    isLoading: nakladnoyLoading,
    isError: nakladnoyError,
    refetch: refetchNakladnoy,
  } = useQuery<NakladnoyResponse>({
    queryKey: ['danniye-vladelsax-nakladnoy', appliedBeginDate, appliedEndDate, appliedOrgId],
    queryFn: () =>
      api.get('/api/dannye-vladeltsev/nakladnoy', {
        params: { beginDate: appliedBeginDate, endDate: appliedEndDate, orgId: appliedOrgId },
      }).then(r => r.data),
    enabled: queryEnabled && !!appliedOrgId,
    staleTime: 0,
  });

  // ── Detail query (right table)
  const {
    data: allDetailRows = [],
    isLoading: detailLoading,
    isError: detailError,
    refetch: refetchDetail,
  } = useQuery<ViplPutRow[]>({
    queryKey: ['danniye-vladelsax-detail', appliedBeginDate, appliedEndDate, appliedOrgId],
    queryFn: () =>
      api.get('/api/dannye-vladeltsev', {
        params: { beginDate: appliedBeginDate, endDate: appliedEndDate, orgId: appliedOrgId },
      }).then(r => r.data),
    enabled: queryEnabled && !!appliedOrgId,
    staleTime: 0,
  });

  // ── Owner data query (loads when a voucher row is clicked)
  const {
    data: ownerData,
    isLoading: ownerLoading,
    isError: ownerError,
  } = useQuery<OwnerData>({
    queryKey: ['danniye-vladelsax-owner', selectedVoucher?.id],
    queryFn: () =>
      api.get(`/api/dannye-vladeltsev/${selectedVoucher!.id}/owner`).then(r => r.data),
    enabled: !!selectedVoucher,
    staleTime: 0,
  });

  // Populate form when owner data loads
  useEffect(() => {
    if (!ownerData) return;
    const f = ownerData.faceMain;
    const m = ownerData.faceMed;
    ownerForm.setFieldsValue({
      lastName: f?.lastName ?? '',
      name: f?.name ?? '',
      firstName: f?.firstName ?? '',
      pol: f?.pol ?? undefined,
      adress: f?.adress ?? '',
      rabota: f?.rabota ?? '',
      dolj: f?.dolj ?? '',
      idOsnov: f?.id_Osnov ?? undefined,
      stavVid: f?.stav_Vid ?? undefined,
      gostPr: f?.gost_Pr ?? undefined,
      pasSer: f?.pas_Ser ?? '',
      pasNum: f?.pas_Num ?? '',
      pasKD: f?.pas_KD ?? '',
      pasData: f?.pas_Data ? dayjs(f.pas_Data) : undefined,
      inn: f?.inn ?? '',
      pnfl: f?.pnfl ?? '',
      medDoc: m?.med_Doc ?? '',
      medData: m?.med_Data ? dayjs(m.med_Data) : undefined,
      medSan: m?.med_San ?? undefined,
      oplDoc: m?.opl_Doc ?? '',
      oplData: m?.opl_Data ? dayjs(m.opl_Data) : undefined,
      oplSymma: m?.opl_Symma ?? undefined,
      talDoc: m?.tal_Doc ?? '',
      talData: m?.tal_Data ? dayjs(m.tal_Data) : undefined,
    });
  }, [ownerData, ownerForm]);

  // ── Save owner mutation
  const saveMutation = useMutation({
    mutationFn: (payload: object) =>
      api.post(`/api/dannye-vladeltsev/${selectedVoucher!.id}/save`, payload).then(r => r.data),
    onSuccess: () => {
      message.success('Данные владельца сохранены');
      queryClient.invalidateQueries({ queryKey: ['danniye-vladelsax-detail', appliedBeginDate, appliedEndDate, appliedOrgId] });
      queryClient.invalidateQueries({ queryKey: ['danniye-vladelsax-owner', selectedVoucher?.id] });
    },
    onError: () => message.error('Ошибка при сохранении'),
  });

  const detailRows = selectedSan?.idSan != null
    ? allDetailRows.filter(r => r.idSan === selectedSan.idSan)
    : allDetailRows;

  const nakladnoyRows: NakladnoyRow[] = nakladnoyData?.rows ?? [];
  const total = nakladnoyData?.total ?? { kolPut: 0, totalSymma: 0 };

  const handleShow = () => {
    if (!selectedOrg) { message.warning('Выберите организацию'); return; }
    if (!beginDate || !endDate) { message.warning('Укажите период (с и по)'); return; }
    setAppliedBeginDate(beginDate);
    setAppliedEndDate(endDate);
    setAppliedOrgId(selectedOrg.id);
    setQueryEnabled(true);
    setSelectedSan(null);
    setSelectedVoucher(null);
    ownerForm.resetFields();
  };

  const handleVoucherClick = (record: ViplPutRow) => {
    if (selectedVoucher?.id === record.id) {
      setSelectedVoucher(null);
      ownerForm.resetFields();
    } else {
      setSelectedVoucher(record);
      ownerForm.resetFields();
    }
  };

  const handleSave = () => {
    if (!selectedVoucher) return;
    ownerForm.validateFields().then(values => {
      saveMutation.mutate({
        ...values,
        pasData: values.pasData?.toISOString() ?? null,
        medData: values.medData?.toISOString() ?? null,
        oplData: values.oplData?.toISOString() ?? null,
        talData: values.talData?.toISOString() ?? null,
        oplSymma: values.oplSymma === '' || values.oplSymma == null ? null : Number(values.oplSymma),
        medSan: values.medSan === '' || values.medSan == null ? null : Number(values.medSan),
      });
    });
  };

  // ── Left table columns
  const nakladnoyColumns: ColumnType<NakladnoyRow>[] = [
    { title: 'Санаторий', dataIndex: 'name_San', key: 'name_San', ellipsis: true },
    { title: 'Кол-во', dataIndex: 'kolPut', key: 'kolPut', width: 70, align: 'right' as const },
    {
      title: 'Сумма', dataIndex: 'totalSymma', key: 'totalSymma', width: 120, align: 'right' as const,
      render: (val: number) => val != null
        ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
  ];

  // ── Right table columns
  const detailColumns: ColumnType<ViplPutRow>[] = [
    { title: '№', key: 'index', width: 45, render: (_: unknown, __: ViplPutRow, i: number) => i + 1 },
    { title: '№ путевки', dataIndex: 'num_Put', key: 'num_Put', width: 110 },
    { title: 'Санаторий', dataIndex: 'name_San', key: 'name_San' },
    {
      title: 'Дата выдачи', dataIndex: 'dataRealizacii', key: 'dataRealizacii', width: 120,
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Сумма', dataIndex: 'symma', key: 'symma', width: 110, align: 'right' as const,
      render: (val: number) => val != null
        ? val.toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '',
    },
    { title: '№ накладной', dataIndex: 'num_Doc', key: 'num_Doc', width: 130 },
    {
      title: 'Дата накладной', dataIndex: 'dataNakladnoy', key: 'dataNakladnoy', width: 130,
      render: (val: string) => val ? dayjs(val).format('DD.MM.YYYY') : '',
    },
    {
      title: 'Владелец', dataIndex: 'isFaceExists', key: 'isFaceExists', width: 90,
      align: 'center' as const,
      render: (val: number) => val === 1
        ? <Text style={{ color: '#52c41a', fontWeight: 600 }}>✓ Есть</Text>
        : <Text type="secondary">—</Text>,
    },
  ];

  const isQueryReady = queryEnabled && !!appliedOrgId;

  return (
    <div style={{ padding: 16 }}>
      {/* Header */}
      <Row justify="space-between" align="middle" style={{ marginBottom: 12 }}>
        <Col>
          <Title level={4} style={{ margin: 0 }}>Данные о владельцах путевок</Title>
          <Text type="secondary" style={{ fontSize: 12 }}>Путевки / Данные о владельцах путевок</Text>
        </Col>
        {isQueryReady && (
          <Col>
            <Space size="large" style={{ background: '#f5f5f5', padding: '6px 16px', borderRadius: 8 }}>
              <span>
                <Text type="secondary">Количество: </Text>
                <Text strong>{total.kolPut}</Text>
              </span>
              <span>
                <Text type="secondary">Сумма: </Text>
                <Text strong>
                  {(total.totalSymma ?? 0).toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                </Text>
              </span>
            </Space>
          </Col>
        )}
      </Row>

      {/* Filters */}
      <Row gutter={[12, 8]} style={{ marginBottom: 12 }} align="bottom">
        <Col xs={24} sm={12} md={5}>
          <Text style={{ display: 'block', marginBottom: 4, fontSize: 12 }}>Период с</Text>
          <DatePicker
            style={{ width: '100%' }} format="DD.MM.YYYY" placeholder="с"
            value={beginDate ? dayjs(beginDate) : null}
            onChange={d => setBeginDate(d ? d.format('YYYY-MM-DD') : '')}
          />
        </Col>
        <Col xs={24} sm={12} md={5}>
          <Text style={{ display: 'block', marginBottom: 4, fontSize: 12 }}>Период по</Text>
          <DatePicker
            style={{ width: '100%' }} format="DD.MM.YYYY" placeholder="по"
            value={endDate ? dayjs(endDate) : null}
            onChange={d => setEndDate(d ? d.format('YYYY-MM-DD') : '')}
          />
        </Col>
        <Col xs={24} md={10}>
          <Text style={{ display: 'block', marginBottom: 4, fontSize: 12 }}>Организация</Text>
          <div style={{ display: 'flex', gap: 8, alignItems: 'center' }}>
            <Button onClick={() => setOrgPickerOpen(true)}>Выбрать организацию</Button>
            {selectedOrg ? (
              <Text ellipsis>
                {selectedOrg.inn && <Text type="secondary" style={{ fontSize: 11 }}>ИНН {selectedOrg.inn}: </Text>}
                <Text strong style={{ fontSize: 12 }}>{selectedOrg.name}</Text>
              </Text>
            ) : (
              <Text type="secondary" style={{ fontSize: 12 }}>Организация не выбрана</Text>
            )}
          </div>
        </Col>
        <Col xs={24} md={4}>
          <Button
            type="primary" icon={<SearchOutlined />} onClick={handleShow}
            loading={nakladnoyLoading} style={{ width: '100%' }}
          >
            Показать
          </Button>
        </Col>
      </Row>

      {/* Tables */}
      <Row gutter={12}>
        {/* LEFT: Grouped by sanatorium */}
        <Col xs={24} lg={8}>
          <Text strong style={{ display: 'block', marginBottom: 6, fontSize: 12, color: '#555' }}>
            По санаториям
          </Text>
          <QueryErrorAlert isError={nakladnoyError} onRetry={refetchNakladnoy} />
          <Table<NakladnoyRow>
            columns={nakladnoyColumns}
            dataSource={nakladnoyRows}
            rowKey={r => String(r.idSan ?? r.name_San ?? 'unknown')}
            loading={nakladnoyLoading}
            size="small"
            pagination={false}
            rowClassName={rec => selectedSan?.idSan === rec.idSan ? 'ant-table-row-selected' : ''}
            onRow={rec => ({
              onClick: () => {
                setSelectedSan(prev => prev?.idSan === rec.idSan ? null : rec);
                setSelectedVoucher(null);
                ownerForm.resetFields();
              },
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 300 }}
            locale={{ emptyText: isQueryReady ? 'Нет данных' : 'Задайте фильтры и нажмите «Показать»' }}
            summary={() =>
              nakladnoyRows.length > 0 ? (
                <Table.Summary.Row>
                  <Table.Summary.Cell index={0}><Text strong>Итого</Text></Table.Summary.Cell>
                  <Table.Summary.Cell index={1} align="right"><Text strong>{total.kolPut}</Text></Table.Summary.Cell>
                  <Table.Summary.Cell index={2} align="right">
                    <Text strong>
                      {(total.totalSymma ?? 0).toLocaleString('ru-RU', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                    </Text>
                  </Table.Summary.Cell>
                </Table.Summary.Row>
              ) : null
            }
          />
        </Col>

        {/* RIGHT: Individual vouchers */}
        <Col xs={24} lg={16}>
          <Text strong style={{ display: 'block', marginBottom: 6, fontSize: 12, color: '#555' }}>
            {selectedSan
              ? `Путевки — ${selectedSan.name_San ?? 'Санаторий'}`
              : 'Все путевки (выберите санаторий слева для фильтрации)'}
          </Text>
          <QueryErrorAlert isError={detailError} onRetry={refetchDetail} />
          <Table<ViplPutRow>
            columns={detailColumns}
            dataSource={detailRows}
            rowKey="id"
            loading={detailLoading}
            size="small"
            pagination={{ pageSize: 15, showSizeChanger: false, showTotal: t => `Всего: ${t}` }}
            rowClassName={rec => {
              if (selectedVoucher?.id === rec.id) return 'ant-table-row-selected';
              if (rec.isFaceExists === 1) return 'row-has-owner';
              return '';
            }}
            onRow={rec => ({
              onClick: () => handleVoucherClick(rec),
              style: { cursor: 'pointer' },
            })}
            scroll={{ x: 900 }}
            locale={{ emptyText: isQueryReady ? 'Нет данных' : 'Задайте фильтры и нажмите «Показать»' }}
          />
        </Col>
      </Row>

      {/* Owner form — shown below when a voucher is selected */}
      {selectedVoucher && (
        <div style={{ marginTop: 16, borderTop: '1px solid #e8e8e8', paddingTop: 16 }}>
          <Row justify="space-between" align="middle" style={{ marginBottom: 12 }}>
            <Col>
              <Space>
                <UserOutlined style={{ color: '#1677ff' }} />
                <Text strong style={{ fontSize: 14 }}>
                  Данные владельца путевки №{selectedVoucher.num_Put}
                </Text>
                {ownerData?.faceMain
                  ? <Text style={{ color: '#52c41a', fontSize: 12 }}>— Редактирование</Text>
                  : <Text style={{ color: '#fa8c16', fontSize: 12 }}>— Новая запись</Text>
                }
              </Space>
            </Col>
            <Col>
              <Button
                type="primary" icon={<SaveOutlined />}
                loading={saveMutation.isPending} onClick={handleSave}
              >
                Сохранить
              </Button>
            </Col>
          </Row>

          {ownerLoading ? (
            <div style={{ textAlign: 'center', padding: 32 }}><Spin /></div>
          ) : ownerError ? (
            <Text type="danger">Ошибка загрузки данных владельца</Text>
          ) : (
            <Form form={ownerForm} layout="vertical" size="small">
              <Row gutter={16}>
                {/* Card 1: Personal data */}
                <Col xs={24} lg={8}>
                  <Card size="small" title="Личные данные владельца" style={{ marginBottom: 12 }}>
                    <Form.Item name="lastName" label="Фамилия">
                      <Input />
                    </Form.Item>
                    <Form.Item name="name" label="Имя">
                      <Input />
                    </Form.Item>
                    <Form.Item name="firstName" label="Отчество">
                      <Input />
                    </Form.Item>
                    <Form.Item name="pol" label="Пол">
                      <Select allowClear placeholder="Выберите">
                        <Select.Option value="М">М (мужской)</Select.Option>
                        <Select.Option value="Ж">Ж (женский)</Select.Option>
                      </Select>
                    </Form.Item>
                    <Form.Item name="adress" label="Место жительства">
                      <Input />
                    </Form.Item>
                    <Form.Item name="rabota" label="Место работы">
                      <Input />
                    </Form.Item>
                    <Form.Item name="dolj" label="Должность">
                      <Input />
                    </Form.Item>
                    <Form.Item name="idOsnov" label="Основания выдачи">
                      <Select allowClear placeholder="Выберите" options={refFace.map(r => ({ value: r.id, label: r.name }))} />
                    </Form.Item>
                    <Form.Item name="stavVid" label="Тип выдачи">
                      <Select allowClear placeholder="Выберите" options={refOs.map(r => ({ value: r.id, label: r.name }))} />
                    </Form.Item>
                    <Form.Item name="gostPr" label="Ставка">
                      <Select allowClear placeholder="Выберите" options={refPr.map(r => ({ value: r.id, label: r.name }))} />
                    </Form.Item>
                  </Card>
                </Col>

                {/* Card 2: Passport data */}
                <Col xs={24} lg={8}>
                  <Card size="small" title="Паспортные данные" style={{ marginBottom: 12 }}>
                    <Form.Item name="pasSer" label="Серия">
                      <Input />
                    </Form.Item>
                    <Form.Item name="pasNum" label="Номер">
                      <Input />
                    </Form.Item>
                    <Form.Item name="pasKD" label="Кем выдан">
                      <Input />
                    </Form.Item>
                    <Form.Item name="pasData" label="Дата выдачи">
                      <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
                    </Form.Item>
                    <Form.Item name="inn" label="ИНН">
                      <Input maxLength={9} />
                    </Form.Item>
                    <Form.Item name="pnfl" label="ПНФЛ">
                      <Input maxLength={20} />
                    </Form.Item>
                  </Card>
                </Col>

                {/* Card 3: Medical, payment, talon */}
                <Col xs={24} lg={8}>
                  <Card size="small" title="Медицинское заключение" style={{ marginBottom: 12 }}>
                    <Form.Item name="medDoc" label="№ Документа">
                      <Input />
                    </Form.Item>
                    <Form.Item name="medData" label="Дата выдачи">
                      <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
                    </Form.Item>
                    <Form.Item name="medSan" label="Рекомендуемый санаторий">
                      <Input type="number" placeholder="ID санатория" />
                    </Form.Item>
                  </Card>
                  <Card size="small" title="Оплата за путевки" style={{ marginBottom: 12 }}>
                    <Form.Item name="oplDoc" label="№ Документа">
                      <Input />
                    </Form.Item>
                    <Form.Item name="oplData" label="Дата оплаты">
                      <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
                    </Form.Item>
                    <Form.Item name="oplSymma" label="Сумма оплаты">
                      <Input type="number" />
                    </Form.Item>
                  </Card>
                  <Card size="small" title="Информация о талоне возврата">
                    <Form.Item name="talDoc" label="№ Документа">
                      <Input />
                    </Form.Item>
                    <Form.Item name="talData" label="Дата приезда">
                      <DatePicker style={{ width: '100%' }} format="DD.MM.YYYY" />
                    </Form.Item>
                  </Card>
                </Col>
              </Row>

              <Divider style={{ margin: '8px 0' }} />
              <Row justify="end">
                <Col>
                  <Button
                    type="primary" icon={<SaveOutlined />}
                    loading={saveMutation.isPending} onClick={handleSave}
                  >
                    Сохранить данные владельца
                  </Button>
                </Col>
              </Row>
            </Form>
          )}
        </div>
      )}

      {/* Organization Picker Modal */}
      <OrganizationPickerModal
        open={orgPickerOpen}
        onClose={() => setOrgPickerOpen(false)}
        onSelect={org => {
          setSelectedOrg(org);
          setOrgPickerOpen(false);
        }}
      />
    </div>
  );
}
