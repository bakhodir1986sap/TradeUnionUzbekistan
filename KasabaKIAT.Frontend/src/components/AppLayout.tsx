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
      content: 'Вы действительно хотите выйти из системы?',
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
            { key: '/prixod-putevok/index', label: 'Приход путевок КУ' },
            { key: '/prixod-putevok/ruchnoy', label: 'Приход путевок ручной' },
            { key: '/prixod-putevok/realizatsiya', label: 'Реализация путевок' },
            { key: '/prixod-putevok/rasxod', label: 'Расход путевок (орг)' },
            { key: '/prixod-putevok/rasxod-fiz', label: 'Расход путевок (физ.лицо)' },
            { key: '/prixod-putevok/ostatki', label: 'Просмотр остатков' },
            { key: '/prixod-putevok/realizovannye', label: 'Реализованные путевки' },
            { key: '/prixod-putevok/oplata', label: 'Оплата и выплаты' },
            { key: '/prixod-putevok/akt-vozvrata', label: 'Акт возврата путевок' },
            { key: '/prixod-putevok/dannye-vladeltsev', label: 'Данные владельцев' },
            { key: '/prixod-putevok/fizlica', label: 'Физ. лица (путевки)' },
            { key: '/prixod-putevok/fizlica-v2', label: 'Физ. лица v2' },
            { key: '/prixod-putevok/organizasii', label: 'Организации (путевки)' },
            { key: '/prixod-putevok/organizasii-v2', label: 'Организации v2' },
            { key: '/prixod-putevok/start-process', label: 'Старт процесса' },
            { key: '/prixod-putevok/sverka', label: 'Сверка данных' },
            { key: '/prixod-putevok/sverka-v2', label: 'Сверка данных v2' },
            { key: '/prixod-putevok/zakritiye-kvartala', label: 'Закрытие квартала' },
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
      key: '/otchoty',
      icon: <BarChartOutlined />,
      label: t('nav.otchety'),
    },
    {
      key: 'nsi',
      icon: <DatabaseOutlined />,
      label: t('nav.nsi'),
      children: [
        { key: '/reference/organizations', icon: <BankOutlined />, label: t('nav.organizacii') },
        { key: '/reference/sanatoriums', icon: <HomeOutlined />, label: 'Санатории' },
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
          defaultOpenKeys={['putyovki', 'prixod-putevok-submenu', 'zayavki', 'buxgalteriya', 'nsi', 'admin']}
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
