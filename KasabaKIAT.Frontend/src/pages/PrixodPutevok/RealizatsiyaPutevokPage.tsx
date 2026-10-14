import { useState } from 'react';
import { Button, Table, Space, Modal, Form, Input, InputNumber, Collapse, Checkbox, Typography, App, Row, Col, Tag } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../api/axios';

const { Title, Text } = Typography;
const { Panel } = Collapse;

interface Voucher {
  id: number;
  num: string;
  status: string;
}

interface Period {
  period: string;
  vouchers: Voucher[];
}

interface Sanatorium {
  sanatName: string;
  periods: Period[];
}

interface Organization {
  id: number;
  name: string;
  inn: string;
  industry?: string;
  region?: string;
}

export function RealizatsiyaPutevokPage() {
  const { message } = App.useApp();
  const queryClient = useQueryClient();

  const [selectedVoucherIds, setSelectedVoucherIds] = useState<number[]>([]);
  const [inn, setInn] = useState('');
  const [foundOrg, setFoundOrg] = useState<Organization | null>(null);

  const [isSearchModalOpen, setIsSearchModalOpen] = useState(false);
  const [searchInn, setSearchInn] = useState('');
  const [searchName, setSearchName] = useState('');
  const [searchResults, setSearchResults] = useState<Organization[]>([]);

  const [isPaymentModalOpen, setIsPaymentModalOpen] = useState(false);
  const [paymentData, setPaymentData] = useState<{ id: number; num: string; paymentPercent: number }[]>([]);

  const { data: sanatoriums = [], isLoading: vouchersLoading } = useQuery<Sanatorium[]>({
    queryKey: ['realizatsiya-vouchers'],
    queryFn: async () => {
      const res = await api.get('/api/realizatsiya/vouchers');
      return res.data;
    },
  });

  const searchOrgMutation = useMutation({
    mutationFn: async (innVal: string) => {
      const res = await api.get(`/api/organizations/by-inn?inn=${innVal}`);
      return res.data as Organization;
    },
    onSuccess: (data) => {
      setFoundOrg(data);
    },
    onError: () => {
      setFoundOrg(null);
      message.error('Организация не найдена');
    },
  });

  const realizovatMutation = useMutation({
    mutationFn: async () => {
      if (!foundOrg) throw new Error('Организация не выбрана');
      await api.post('/api/realizatsiya/realizovat', { voucherIds: selectedVoucherIds, orgId: foundOrg.id });
    },
    onSuccess: () => {
      message.success('Путевки реализованы');
      queryClient.invalidateQueries({ queryKey: ['realizatsiya-vouchers'] });
      setSelectedVoucherIds([]);
    },
    onError: () => {
      message.error('Ошибка при реализации');
    },
  });

  const zabronirovaMutation = useMutation({
    mutationFn: async () => {
      if (!foundOrg) throw new Error('Организация не выбрана');
      await api.post('/api/realizatsiya/zabronirovat', { voucherIds: selectedVoucherIds, orgId: foundOrg.id });
    },
    onSuccess: () => {
      message.success('Путевки забронированы');
      queryClient.invalidateQueries({ queryKey: ['realizatsiya-vouchers'] });
      setSelectedVoucherIds([]);
    },
    onError: () => {
      message.error('Ошибка при бронировании');
    },
  });

  const otmenitBronMutation = useMutation({
    mutationFn: async () => {
      await api.post('/api/realizatsiya/otmenit-bron', { voucherIds: selectedVoucherIds });
    },
    onSuccess: () => {
      message.success('Бронь отменена');
      queryClient.invalidateQueries({ queryKey: ['realizatsiya-vouchers'] });
      setSelectedVoucherIds([]);
    },
    onError: () => {
      message.error('Ошибка при отмене брони');
    },
  });

  const searchOrgsMutation = useMutation({
    mutationFn: async ({ inn, name }: { inn: string; name: string }) => {
      const params = new URLSearchParams();
      if (inn) params.append('inn', inn);
      if (name) params.append('name', name);
      const res = await api.get(`/api/organizations?${params.toString()}`);
      return res.data as Organization[];
    },
    onSuccess: (data) => {
      setSearchResults(data);
    },
    onError: () => {
      message.error('Ошибка при поиске организаций');
    },
  });

  const paymentMutation = useMutation({
    mutationFn: async ({ id, paymentPercent }: { id: number; paymentPercent: number }) => {
      await api.put(`/api/realizatsiya/vouchers/${id}/payment`, { paymentPercent });
    },
    onSuccess: () => {
      message.success('Оплата обновлена');
      queryClient.invalidateQueries({ queryKey: ['realizatsiya-vouchers'] });
    },
    onError: () => {
      message.error('Ошибка при сохранении оплаты');
    },
  });

  const handleVoucherCheck = (voucherId: number, checked: boolean) => {
    setSelectedVoucherIds((prev) =>
      checked ? [...prev, voucherId] : prev.filter((id) => id !== voucherId)
    );
  };

  const handleSearchOrg = () => {
    if (!inn.trim()) {
      message.warning('Введите ИНН');
      return;
    }
    searchOrgMutation.mutate(inn.trim());
  };

  const handleRealizovat = () => {
    if (selectedVoucherIds.length === 0) {
      message.warning('Выберите путевки');
      return;
    }
    if (!foundOrg) {
      message.warning('Найдите организацию');
      return;
    }
    realizovatMutation.mutate();
  };

  const handleZabronirovat = () => {
    if (selectedVoucherIds.length === 0) {
      message.warning('Выберите путевки');
      return;
    }
    if (!foundOrg) {
      message.warning('Найдите организацию');
      return;
    }
    zabronirovaMutation.mutate();
  };

  const handleOtmenitBron = () => {
    if (selectedVoucherIds.length === 0) {
      message.warning('Выберите путевки');
      return;
    }
    otmenitBronMutation.mutate();
  };

  const handleOpenSearchModal = () => {
    setSearchInn('');
    setSearchName('');
    setSearchResults([]);
    setIsSearchModalOpen(true);
  };

  const handleSearchOrgs = () => {
    searchOrgsMutation.mutate({ inn: searchInn, name: searchName });
  };

  const handleSelectOrg = (org: Organization) => {
    setFoundOrg(org);
    setInn(org.inn);
    setIsSearchModalOpen(false);
  };

  const handleOpenPaymentModal = () => {
    if (selectedVoucherIds.length === 0) {
      message.warning('Выберите путевки');
      return;
    }
    const allVouchers: { id: number; num: string }[] = [];
    sanatoriums.forEach((san) => {
      san.periods.forEach((p) => {
        p.vouchers.forEach((v) => {
          if (selectedVoucherIds.includes(v.id)) {
            allVouchers.push({ id: v.id, num: v.num });
          }
        });
      });
    });
    setPaymentData(allVouchers.map((v) => ({ ...v, paymentPercent: 100 })));
    setIsPaymentModalOpen(true);
  };

  const getStatusTag = (status: string) => {
    const colorMap: Record<string, string> = {
      available: 'green',
      booked: 'orange',
      realized: 'blue',
    };
    return <Tag color={colorMap[status] ?? 'default'}>{status}</Tag>;
  };

  const searchOrgColumns = [
    { title: 'Название', dataIndex: 'name', key: 'name' },
    { title: 'ИНН', dataIndex: 'inn', key: 'inn' },
    { title: 'Отрасль', dataIndex: 'industry', key: 'industry' },
    { title: 'Регион', dataIndex: 'region', key: 'region' },
    {
      title: '',
      key: 'action',
      render: (_: unknown, record: Organization) => (
        <Button type="primary" size="small" onClick={() => handleSelectOrg(record)}>
          Tanlash
        </Button>
      ),
    },
  ];

  const paymentColumns = [
    { title: 'Номер путевки', dataIndex: 'num', key: 'num' },
    {
      title: 'Оплата (%)',
      dataIndex: 'paymentPercent',
      key: 'paymentPercent',
      render: (_: unknown, record: { id: number; num: string; paymentPercent: number }, index: number) => (
        <InputNumber
          min={0}
          max={100}
          value={record.paymentPercent}
          onChange={(val) => {
            setPaymentData((prev) =>
              prev.map((item, i) => (i === index ? { ...item, paymentPercent: val ?? 0 } : item))
            );
          }}
        />
      ),
    },
    {
      title: '',
      key: 'save',
      render: (_: unknown, record: { id: number; num: string; paymentPercent: number }) => (
        <Button
          type="primary"
          size="small"
          loading={paymentMutation.isPending}
          onClick={() => paymentMutation.mutate({ id: record.id, paymentPercent: record.paymentPercent })}
        >
          Сохранить
        </Button>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <Title level={3}>Реализация путевок</Title>
      <Row gutter={24}>
        {/* LEFT: Accordion of sanatoriums */}
        <Col xs={24} md={12}>
          <Title level={5}>Санатории и путевки</Title>
          {vouchersLoading ? (
            <Text>Загрузка...</Text>
          ) : (
            <Collapse accordion>
              {sanatoriums.map((san, sanIdx) => (
                <Panel header={san.sanatName} key={sanIdx}>
                  <Collapse>
                    {san.periods.map((p, pIdx) => (
                      <Panel header={p.period} key={pIdx}>
                        <Space direction="vertical" style={{ width: '100%' }}>
                          {p.vouchers.map((voucher) => (
                            <Row key={voucher.id} align="middle" gutter={8}>
                              <Col>
                                <Checkbox
                                  checked={selectedVoucherIds.includes(voucher.id)}
                                  onChange={(e) => handleVoucherCheck(voucher.id, e.target.checked)}
                                  disabled={voucher.status === 'realized'}
                                >
                                  {voucher.num}
                                </Checkbox>
                              </Col>
                              <Col>{getStatusTag(voucher.status)}</Col>
                            </Row>
                          ))}
                        </Space>
                      </Panel>
                    ))}
                  </Collapse>
                </Panel>
              ))}
            </Collapse>
          )}
          {selectedVoucherIds.length > 0 && (
            <div style={{ marginTop: 12 }}>
              <Text type="secondary">Выбрано путевок: {selectedVoucherIds.length}</Text>
            </div>
          )}
        </Col>

        {/* RIGHT: INN search + action buttons */}
        <Col xs={24} md={12}>
          <Title level={5}>Организация</Title>
          <Space direction="vertical" style={{ width: '100%' }}>
            <Space.Compact style={{ width: '100%' }}>
              <Input
                placeholder="Введите ИНН"
                value={inn}
                onChange={(e) => setInn(e.target.value)}
                onPressEnter={handleSearchOrg}
              />
              <Button
                icon={<SearchOutlined />}
                onClick={handleSearchOrg}
                loading={searchOrgMutation.isPending}
              >
                Поиск
              </Button>
            </Space.Compact>

            <Button type="link" onClick={handleOpenSearchModal} style={{ padding: 0 }}>
              Расширенный поиск организации
            </Button>

            {foundOrg && (
              <div
                style={{
                  padding: 12,
                  border: '1px solid #d9d9d9',
                  borderRadius: 6,
                  background: '#fafafa',
                }}
              >
                <Text strong>{foundOrg.name}</Text>
                <br />
                <Text type="secondary">ИНН: {foundOrg.inn}</Text>
              </div>
            )}

            <Space wrap>
              <Button
                type="primary"
                onClick={handleRealizovat}
                loading={realizovatMutation.isPending}
                disabled={selectedVoucherIds.length === 0 || !foundOrg}
              >
                Реализовать
              </Button>
              <Button
                onClick={handleZabronirovat}
                loading={zabronirovaMutation.isPending}
                disabled={selectedVoucherIds.length === 0 || !foundOrg}
              >
                Забронировать
              </Button>
              <Button
                danger
                onClick={handleOtmenitBron}
                loading={otmenitBronMutation.isPending}
                disabled={selectedVoucherIds.length === 0}
              >
                Отменить бронь
              </Button>
            </Space>

            <Button
              onClick={handleOpenPaymentModal}
              disabled={selectedVoucherIds.length === 0}
            >
              Установить оплату
            </Button>
          </Space>
        </Col>
      </Row>

      {/* Modal: Search organization (dobavit) */}
      <Modal
        title="Поиск организации"
        open={isSearchModalOpen}
        onCancel={() => setIsSearchModalOpen(false)}
        footer={null}
        width={700}
      >
        <Form layout="inline" style={{ marginBottom: 16 }}>
          <Form.Item label="ИНН">
            <Input
              value={searchInn}
              onChange={(e) => setSearchInn(e.target.value)}
              placeholder="ИНН"
            />
          </Form.Item>
          <Form.Item label="Название">
            <Input
              value={searchName}
              onChange={(e) => setSearchName(e.target.value)}
              placeholder="Название"
            />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              icon={<SearchOutlined />}
              onClick={handleSearchOrgs}
              loading={searchOrgsMutation.isPending}
            >
              Поиск
            </Button>
          </Form.Item>
        </Form>
        <Table
          dataSource={searchResults}
          columns={searchOrgColumns}
          rowKey="id"
          size="small"
          pagination={{ pageSize: 8 }}
        />
      </Modal>

      {/* Modal: Payment per voucher (dobavit2) */}
      <Modal
        title="Оплата по путевкам"
        open={isPaymentModalOpen}
        onCancel={() => setIsPaymentModalOpen(false)}
        footer={[
          <Button key="close" onClick={() => setIsPaymentModalOpen(false)}>
            Закрыть
          </Button>,
        ]}
        width={500}
      >
        <Table
          dataSource={paymentData}
          columns={paymentColumns}
          rowKey="id"
          size="small"
          pagination={false}
        />
      </Modal>
    </div>
  );
}
