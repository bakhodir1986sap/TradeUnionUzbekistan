import { lazy, Suspense } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { Spin } from 'antd';
import { PrivateRoute } from './auth/PrivateRoute';
import { AppLayout } from './components/AppLayout';
import { LoginPage } from './pages/Login/LoginPage';

const DashboardPage = lazy(() => import('./pages/Dashboard/DashboardPage').then(m => ({ default: m.DashboardPage })));
const GodovoyPlanPage = lazy(() => import('./pages/GodovoyPlan/GodovoyPlanPage').then(m => ({ default: m.GodovoyPlanPage })));
const GrafikZaezdovPage = lazy(() => import('./pages/GrafikZaezdov/GrafikZaezdovPage').then(m => ({ default: m.GrafikZaezdovPage })));
const IndexPage = lazy(() => import('./pages/PrixodPutevok/IndexPage').then(m => ({ default: m.IndexPage })));
const PrixodPutyovokRuchnoyPage = lazy(() => import('./pages/PrixodPutevok/PrixodPutyovokRuchnoyPage').then(m => ({ default: m.PrixodPutyovokRuchnoyPage })));
const RealizatsiyaPutevokPage = lazy(() => import('./pages/PrixodPutevok/RealizatsiyaPutevokPage').then(m => ({ default: m.RealizatsiyaPutevokPage })));
const RasxodPutyovokPage = lazy(() => import('./pages/PrixodPutevok/RasxodPutyovokPage').then(m => ({ default: m.RasxodPutyovokPage })));
const RasxodPutyovokFizLisoPage = lazy(() => import('./pages/PrixodPutevok/RasxodPutyovokFizLisoPage').then(m => ({ default: m.RasxodPutyovokFizLisoPage })));
const ProsmotrOstatkovPage = lazy(() => import('./pages/PrixodPutevok/ProsmotrOstatkovPage').then(m => ({ default: m.ProsmotrOstatkovPage })));
const ProsmotrRealzPutevokPage = lazy(() => import('./pages/PrixodPutevok/ProsmotrRealzPutevokPage').then(m => ({ default: m.ProsmotrRealzPutevokPage })));
const OplataViplatiZaPutyovkiPage = lazy(() => import('./pages/PrixodPutevok/OplataViplatiZaPutyovkiPage').then(m => ({ default: m.OplataViplatiZaPutyovkiPage })));
const AktVozvrataPutyovokPage = lazy(() => import('./pages/PrixodPutevok/AktVozvrataPutyovokPage').then(m => ({ default: m.AktVozvrataPutyovokPage })));
const DanniyeVladelsaxPutyovokPage = lazy(() => import('./pages/PrixodPutevok/DanniyeVladelsaxPutyovokPage').then(m => ({ default: m.DanniyeVladelsaxPutyovokPage })));
const FizLisoPage = lazy(() => import('./pages/PrixodPutevok/FizLisoPage').then(m => ({ default: m.FizLisoPage })));
const FizLisoV2Page = lazy(() => import('./pages/PrixodPutevok/FizLisoV2Page').then(m => ({ default: m.FizLisoV2Page })));
const OrganizasiiPage = lazy(() => import('./pages/PrixodPutevok/OrganizasiiPage').then(m => ({ default: m.OrganizasiiPage })));
const StartProcessPage = lazy(() => import('./pages/PrixodPutevok/StartProcessPage').then(m => ({ default: m.StartProcessPage })));
const SverkaDannixPage = lazy(() => import('./pages/PrixodPutevok/SverkaDannixPage').then(m => ({ default: m.SverkaDannixPage })));
const SverkaDannixV2Page = lazy(() => import('./pages/PrixodPutevok/SverkaDannixV2Page').then(m => ({ default: m.SverkaDannixV2Page })));
const ZakritiyaKvartalaPoPutyovkamPage = lazy(() => import('./pages/PrixodPutevok/ZakritiyaKvartalaPoPutyovkamPage').then(m => ({ default: m.ZakritiyaKvartalaPoPutyovkamPage })));
const CampZayavkiPage = lazy(() => import('./pages/CampZayavki/CampZayavkiPage').then(m => ({ default: m.CampZayavkiPage })));
const BuxgalterPage = lazy(() => import('./pages/Buxgalter/BuxgalterPage').then(m => ({ default: m.BuxgalterPage })));
const PlatejnoyePorucheniyePage = lazy(() => import('./pages/Buxgalter/PlatejnoyePorucheniyePage').then(m => ({ default: m.PlatejnoyePorucheniyePage })));
const OtchotCoreTabsPage = lazy(() => import('./pages/Otchot/OtchotCoreTabsPage').then(m => ({ default: m.OtchotCoreTabsPage })));
const OtchotPutyovkiTabsPage = lazy(() => import('./pages/Otchot/OtchotPutyovkiTabsPage').then(m => ({ default: m.OtchotPutyovkiTabsPage })));
const OtchotFinansyTabsPage = lazy(() => import('./pages/Otchot/OtchotFinansyTabsPage').then(m => ({ default: m.OtchotFinansyTabsPage })));
const OtchotJurOrderTabsPage = lazy(() => import('./pages/Otchot/OtchotJurOrderTabsPage').then(m => ({ default: m.OtchotJurOrderTabsPage })));
const OtchotMonitoringTabsPage = lazy(() => import('./pages/Otchot/OtchotMonitoringTabsPage').then(m => ({ default: m.OtchotMonitoringTabsPage })));
const OtchotKartochkaTabsPage = lazy(() => import('./pages/Otchot/OtchotKartochkaTabsPage').then(m => ({ default: m.OtchotKartochkaTabsPage })));
const OtchotFinOtchetTabsPage = lazy(() => import('./pages/Otchot/OtchotFinOtchetTabsPage').then(m => ({ default: m.OtchotFinOtchetTabsPage })));
const UsersPage = lazy(() => import('./pages/Admin/UsersPage').then(m => ({ default: m.UsersPage })));
const RolesPage = lazy(() => import('./pages/Admin/RolesPage').then(m => ({ default: m.RolesPage })));
const UserActionsPage = lazy(() => import('./pages/Admin/UserActionsPage').then(m => ({ default: m.UserActionsPage })));
const SpravichnikiPage = lazy(() => import('./pages/Admin/SpravichnikiPage').then(m => ({ default: m.SpravichnikiPage })));
const OrganizationsPage = lazy(() => import('./pages/Reference/OrganizationsPage').then(m => ({ default: m.OrganizationsPage })));
const SanatoriumsPage = lazy(() => import('./pages/Reference/SanatoriumsPage').then(m => ({ default: m.SanatoriumsPage })));
const NotFoundPage = lazy(() => import('./pages/NotFound/NotFoundPage').then(m => ({ default: m.NotFoundPage })));

const pageFallback = (
  <div style={{ display: 'flex', justifyContent: 'center', padding: 48 }}>
    <Spin size="large" />
  </div>
);

export function AppRouter() {
  return (
    <Suspense fallback={pageFallback}>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <AppLayout />
            </PrivateRoute>
          }
        >
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="dashboard" element={<DashboardPage />} />
          <Route path="godovoy-plan" element={<GodovoyPlanPage />} />
          <Route path="grafik-zaezdov" element={<GrafikZaezdovPage />} />
          <Route path="prixod-putevok" element={<Navigate to="/prixod-putevok/index" replace />} />
          <Route path="prixod-putevok/index" element={<IndexPage />} />
          <Route path="prixod-putevok/ruchnoy" element={<PrixodPutyovokRuchnoyPage />} />
          <Route path="prixod-putevok/realizatsiya" element={<RealizatsiyaPutevokPage />} />
          <Route path="prixod-putevok/rasxod" element={<RasxodPutyovokPage />} />
          <Route path="prixod-putevok/rasxod-fiz" element={<RasxodPutyovokFizLisoPage />} />
          <Route path="prixod-putevok/ostatki" element={<ProsmotrOstatkovPage />} />
          <Route path="prixod-putevok/realizovannye" element={<ProsmotrRealzPutevokPage />} />
          <Route path="prixod-putevok/oplata" element={<OplataViplatiZaPutyovkiPage />} />
          <Route path="prixod-putevok/akt-vozvrata" element={<AktVozvrataPutyovokPage />} />
          <Route path="prixod-putevok/dannye-vladeltsev" element={<DanniyeVladelsaxPutyovokPage />} />
          <Route path="prixod-putevok/fizlica" element={<FizLisoPage />} />
          <Route path="prixod-putevok/fizlica-v2" element={<FizLisoV2Page />} />
          <Route path="prixod-putevok/organizasii" element={<OrganizasiiPage />} />
          <Route path="prixod-putevok/start-process" element={<StartProcessPage />} />
          <Route path="prixod-putevok/sverka" element={<SverkaDannixPage />} />
          <Route path="prixod-putevok/sverka-v2" element={<SverkaDannixV2Page />} />
          <Route path="prixod-putevok/zakritiye-kvartala" element={<ZakritiyaKvartalaPoPutyovkamPage />} />
          <Route path="camp-zayavki" element={<CampZayavkiPage />} />
          <Route path="buxgalter" element={<BuxgalterPage />} />
          <Route path="platejnoe-poruchenie" element={<PlatejnoyePorucheniyePage />} />
          <Route path="otchoty" element={<Navigate to="/otchoty/osnovnye" replace />} />
          <Route path="otchoty/osnovnye" element={<OtchotCoreTabsPage />} />
          <Route path="otchoty/putyovki" element={<OtchotPutyovkiTabsPage />} />
          <Route path="otchoty/finansy" element={<OtchotFinansyTabsPage />} />
          <Route path="otchoty/jur-order" element={<OtchotJurOrderTabsPage />} />
          <Route path="otchoty/monitoring" element={<OtchotMonitoringTabsPage />} />
          <Route path="otchoty/kartochka" element={<OtchotKartochkaTabsPage />} />
          <Route path="otchoty/fin-otchet" element={<OtchotFinOtchetTabsPage />} />
          <Route path="admin/users" element={<UsersPage />} />
          <Route path="admin/roles" element={<RolesPage />} />
          <Route path="admin/user-actions" element={<UserActionsPage />} />
          <Route path="admin/spravochniki" element={<SpravichnikiPage />} />
          <Route path="reference/organizations" element={<OrganizationsPage />} />
          <Route path="reference/sanatoriums" element={<SanatoriumsPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Route>
      </Routes>
    </Suspense>
  );
}
