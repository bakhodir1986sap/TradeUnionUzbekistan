import { Row, Col, Card, Typography } from 'antd';
import {
  FileTextOutlined,
  CalendarOutlined,
  BarChartOutlined,
  PlusCircleOutlined,
  TeamOutlined,
  BankOutlined,
  DatabaseOutlined,
  SettingOutlined,
  UserOutlined,
  SearchOutlined,
  SolutionOutlined,
  HomeOutlined,
  PrinterOutlined,
  FundOutlined,
} from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

const { Text } = Typography;

interface Tile {
  icon: React.ReactNode;
  label: string;
  path: string;
  color: string;
}

export function DashboardPage() {
  const navigate = useNavigate();
  const { t } = useTranslation();

  const tiles: Tile[] = [
    { icon: <CalendarOutlined />, label: t('nav.grafikZaezdov'), path: '/grafik-zaezdov', color: '#1890ff' },
    { icon: <BarChartOutlined />, label: t('nav.godovoyPlan'), path: '/godovoy-plan', color: '#52c41a' },
    { icon: <PlusCircleOutlined />, label: t('nav.prixodPutevok'), path: '/prixod-putevok', color: '#13c2c2' },
    { icon: <FileTextOutlined />, label: t('nav.putyovki'), path: '/prixod-putevok', color: '#722ed1' },
    { icon: <SolutionOutlined />, label: t('nav.campZayavki'), path: '/camp-zayavki', color: '#fa8c16' },
    { icon: <BankOutlined />, label: t('nav.buxgalteriya'), path: '/buxgalter', color: '#eb2f96' },
    { icon: <FundOutlined />, label: t('nav.platejnoe'), path: '/platejnoe-poruchenie', color: '#f5222d' },
    { icon: <PrinterOutlined />, label: t('nav.otchety'), path: '/otchoty', color: '#fa541c' },
    { icon: <DatabaseOutlined />, label: t('nav.spravochniki'), path: '/admin/spravochniki', color: '#a0d911' },
    { icon: <HomeOutlined />, label: 'Санатории', path: '/reference/sanatoriums', color: '#36cfc9' },
    { icon: <BankOutlined />, label: t('nav.organizacii'), path: '/reference/organizations', color: '#4096ff' },
    { icon: <UserOutlined />, label: t('nav.users'), path: '/admin/users', color: '#7cb305' },
    { icon: <TeamOutlined />, label: t('nav.roles'), path: '/admin/roles', color: '#d4380d' },
    { icon: <SearchOutlined />, label: t('nav.userActions'), path: '/admin/user-actions', color: '#d48806' },
    { icon: <SettingOutlined />, label: t('nav.admin'), path: '/admin/users', color: '#08979c' },
    { icon: <DatabaseOutlined />, label: t('nav.nsi'), path: '/admin/spravochniki', color: '#531dab' },
  ];

  return (
    <div>
      <Typography.Title level={4} style={{ marginBottom: 24 }}>
        {t('nav.dashboard')}
      </Typography.Title>
      <Row gutter={[16, 16]}>
        {tiles.map((tile, i) => (
          <Col key={i} xs={12} sm={8} md={6} lg={4}>
            <Card
              hoverable
              onClick={() => navigate(tile.path)}
              style={{ textAlign: 'center', cursor: 'pointer' }}
              styles={{ body: { padding: '20px 12px' } }}
            >
              <div
                style={{
                  fontSize: 36,
                  color: tile.color,
                  marginBottom: 8,
                }}
              >
                {tile.icon}
              </div>
              <Text style={{ fontSize: 12 }}>{tile.label}</Text>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
}
