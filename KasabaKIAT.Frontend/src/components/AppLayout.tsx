import { useState } from 'react';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import {
  Layout,
  Menu,
  Avatar,
  Dropdown,
  Space,
  Typography,
  Button,
  App,
} from 'antd';
import type { MenuProps } from 'antd';
import {
  HomeOutlined,
  FileTextOutlined,
  TeamOutlined,
  BankOutlined,
  BarChartOutlined,
  DatabaseOutlined,
  SettingOutlined,
  UserOutlined,
  LogoutOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  GlobalOutlined,
  CalendarOutlined,
  PlusCircleOutlined,
  SearchOutlined,
  SolutionOutlined,
} from '@ant-design/icons';
import { useAuth } from '../auth/AuthContext';
import { useTranslation } from 'react-i18next';

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

export function AppLayout() {
  const [collapsed, setCollapsed] = useState(false);
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const { modal } = App.useApp();
  const { t, i18n } = useTranslation();

  const handleLogout = () => {
    modal.confirm({
      title: t('nav.logout'),
      content: t('nav.logoutConfirm'),
      onOk: async () => {
        await logout();
        navigate('/login');
      },
    });
  };

  const menuItems: MenuProps['items'] = [
    {
      key: '/dashboard',
      icon: <HomeOutlined />,
      label: t('nav.dashboard'),
    },
    {
      key: 'putyovki',
      icon: <FileTextOutlined />,
      label: t('nav.putyovki'),
      children: [
        { key: '/grafik-zaezdov', icon: <CalendarOutlined />, label: t('nav.grafikZaezdov') },
        { key: '/godovoy-plan', icon: <BarChartOutlined />, label: t('nav.godovoyPlan') },
        {
          key: 'prixod-putevok-submenu',
          icon: <PlusCircleOutlined />,
          label: t('nav.prixodPutevok'),
          children: [
            { key: '/prixod-putevok/index', label: t('nav.prixodKu') },
            { key: '/prixod-putevok/ruchnoy', label: t('nav.prixodRuchnoy') },
            { key: '/prixod-putevok/realizatsiya', label: t('nav.realizatsiya') },
            { key: '/prixod-putevok/rasxod', label: t('nav.rasxodOrg') },
            { key: '/prixod-putevok/rasxod-fiz', label: t('nav.rasxodFiz') },
            { key: '/prixod-putevok/ostatki', label: t('nav.ostatki') },
            { key: '/prixod-putevok/realizovannye', label: t('nav.realizovannye') },
            { key: '/prixod-putevok/oplata', label: t('nav.oplataViplatiMenu') },
            { key: '/prixod-putevok/akt-vozvrata', label: t('nav.aktVozvrata') },
            { key: '/prixod-putevok/dannye-vladeltsev', label: t('nav.dannyeVladeltsev') },
            { key: '/prixod-putevok/fizlica', label: t('nav.fizlicaPutyovki') },
            { key: '/prixod-putevok/fizlica-v2', label: t('nav.fizlicaV2') },
            { key: '/prixod-putevok/organizasii', label: t('nav.organizasiiPutyovki') },
            { key: '/prixod-putevok/organizasii-v2', label: t('nav.organizasiiV2') },
            { key: '/prixod-putevok/start-process', label: t('nav.startProcess') },
            { key: '/prixod-putevok/sverka', label: t('nav.sverka') },
            { key: '/prixod-putevok/sverka-v2', label: t('nav.sverkaV2') },
            { key: '/prixod-putevok/zakritiye-kvartala', label: t('nav.zakritiyeKvartala') },
          ],
        },
      ],
    },
    {
      key: 'zayavki',
      icon: <SolutionOutlined />,
      label: t('nav.zayavki'),
      children: [
        { key: '/camp-zayavki', icon: <TeamOutlined />, label: t('nav.campZayavki') },
      ],
    },
    {
      key: 'buxgalteriya',
      icon: <BankOutlined />,
      label: t('nav.buxgalteriya'),
      children: [
        { key: '/buxgalter', icon: <DatabaseOutlined />, label: t('nav.vvodDannix') },
        { key: '/platejnoe-poruchenie', icon: <FileTextOutlined />, label: t('nav.platejnoe') },
      ],
    },
    {
      key: 'otchoty',
      icon: <BarChartOutlined />,
      label: t('nav.otchety'),
      children: [
        { key: '/otchoty/osnovnye',   label: t('nav.otchetyOsnovnye') },
        { key: '/otchoty/putyovki',   label: t('nav.otchetyPutyovki') },
        { key: '/otchoty/finansy',    label: t('nav.otchetyFinansy') },
        { key: '/otchoty/jur-order',  label: t('nav.otchetyJurOrder') },
        { key: '/otchoty/monitoring', label: t('nav.otchetyMonitoring') },
        { key: '/otchoty/kartochka',  label: t('nav.otchetyKartochka') },
        { key: '/otchoty/fin-otchet', label: t('nav.otchetyFinOtchet') },
      ],
    },
    {
      key: 'nsi',
      icon: <DatabaseOutlined />,
      label: t('nav.nsi'),
      children: [
        { key: '/reference/organizations', icon: <BankOutlined />, label: t('nav.organizacii') },
        { key: '/reference/sanatoriums', icon: <HomeOutlined />, label: t('nav.sanatoriums') },
        { key: '/admin/spravochniki', icon: <DatabaseOutlined />, label: t('nav.spravochniki') },
      ],
    },
    {
      key: 'admin',
      icon: <SettingOutlined />,
      label: t('nav.admin'),
      children: [
        { key: '/admin/users', icon: <UserOutlined />, label: t('nav.users') },
        { key: '/admin/roles', icon: <TeamOutlined />, label: t('nav.roles') },
        { key: '/admin/user-actions', icon: <SearchOutlined />, label: t('nav.userActions') },
      ],
    },
  ];

  const userMenuItems: MenuProps['items'] = [
    {
      key: 'lang-ru',
      icon: <GlobalOutlined />,
      label: 'Русский',
      onClick: () => i18n.changeLanguage('ru'),
    },
    {
      key: 'lang-uz',
      icon: <GlobalOutlined />,
      label: 'O\'zbekcha',
      onClick: () => i18n.changeLanguage('uz'),
    },
    { type: 'divider' },
    {
      key: 'logout',
      icon: <LogoutOutlined />,
      label: t('nav.logout'),
      onClick: handleLogout,
      danger: true,
    },
  ];

  const selectedKey = location.pathname;

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider
        collapsible
        collapsed={collapsed}
        trigger={null}
        style={{ background: '#001529' }}
        width={240}
      >
        <div
          style={{
            height: 64,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#fff',
            fontSize: collapsed ? 14 : 16,
            fontWeight: 700,
            padding: '0 16px',
            borderBottom: '1px solid rgba(255,255,255,0.1)',
            overflow: 'hidden',
            whiteSpace: 'nowrap',
          }}
        >
          {collapsed ? 'KK' : 'KasabaKIAT'}
        </div>
        <Menu
          theme="dark"
          mode="inline"
          selectedKeys={[selectedKey]}
          defaultOpenKeys={['putyovki', 'prixod-putevok-submenu', 'zayavki', 'buxgalteriya', 'otchoty', 'nsi', 'admin']}
          items={menuItems}
          onClick={({ key }) => {
            if (key.startsWith('/')) navigate(key);
          }}
          style={{ borderRight: 0 }}
        />
      </Sider>
      <Layout>
        <Header
          style={{
            background: '#fff',
            padding: '0 24px',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            boxShadow: '0 1px 4px rgba(0,21,41,.08)',
            position: 'sticky',
            top: 0,
            zIndex: 100,
          }}
        >
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{ fontSize: 18 }}
          />
          <Space>
            <Dropdown menu={{ items: userMenuItems }} trigger={['click']}>
              <Space style={{ cursor: 'pointer' }}>
                <Avatar icon={<UserOutlined />} size="small" />
                <Text>{user?.userName || user?.email}</Text>
              </Space>
            </Dropdown>
          </Space>
        </Header>
        <Content style={{ margin: '24px', minHeight: 280 }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
