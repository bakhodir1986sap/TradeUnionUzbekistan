import { Routes, Route, Navigate } from 'react-router-dom';
import { PrivateRoute } from './auth/PrivateRoute';
import { AppLayout } from './components/AppLayout';
import { LoginPage } from './pages/Login/LoginPage';
import { DashboardPage } from './pages/Dashboard/DashboardPage';
import { GodovoyPlanPage } from './pages/GodovoyPlan/GodovoyPlanPage';
import { GrafikZaezdovPage } from './pages/GrafikZaezdov/GrafikZaezdovPage';
import { IndexPage } from './pages/PrixodPutevok/IndexPage';
import { PrixodPutyovokRuchnoyPage } from './pages/PrixodPutevok/PrixodPutyovokRuchnoyPage';
import { RealizatsiyaPutevokPage } from './pages/PrixodPutevok/RealizatsiyaPutevokPage';
import { RasxodPutyovokPage } from './pages/PrixodPutevok/RasxodPutyovokPage';
import { RasxodPutyovokFizLisoPage } from './pages/PrixodPutevok/RasxodPutyovokFizLisoPage';
import { ProsmotrOstatkovPage } from './pages/PrixodPutevok/ProsmotrOstatkovPage';
import { ProsmotrRealzPutevokPage } from './pages/PrixodPutevok/ProsmotrRealzPutevokPage';
import { OplataViplatiZaPutyovkiPage } from './pages/PrixodPutevok/OplataViplatiZaPutyovkiPage';
import { AktVozvrataPutyovokPage } from './pages/PrixodPutevok/AktVozvrataPutyovokPage';
import { DanniyeVladelsaxPutyovokPage } from './pages/PrixodPutevok/DanniyeVladelsaxPutyovokPage';
import { FizLisoPage } from './pages/PrixodPutevok/FizLisoPage';
import { FizLisoV2Page } from './pages/PrixodPutevok/FizLisoV2Page';
import { OrganizasiiPage } from './pages/PrixodPutevok/OrganizasiiPage';
import { OrganizasiiV2Page } from './pages/PrixodPutevok/OrganizasiiV2Page';
import { StartProcessPage } from './pages/PrixodPutevok/StartProcessPage';
import { SverkaDannixPage } from './pages/PrixodPutevok/SverkaDannixPage';
import { SverkaDannixV2Page } from './pages/PrixodPutevok/SverkaDannixV2Page';
import { ZakritiyaKvartalaPoPutyovkamPage } from './pages/PrixodPutevok/ZakritiyaKvartalaPoPutyovkamPage';
import { CampZayavkiPage } from './pages/CampZayavki/CampZayavkiPage';
import { BuxgalterPage } from './pages/Buxgalter/BuxgalterPage';
import { PlatejnoyePorucheniyePage } from './pages/Buxgalter/PlatejnoyePorucheniyePage';
import { OtchotPage } from './pages/Otchot/OtchotPage';
import { UsersPage } from './pages/Admin/UsersPage';
import { RolesPage } from './pages/Admin/RolesPage';
import { UserActionsPage } from './pages/Admin/UserActionsPage';
import { SpravichnikiPage } from './pages/Admin/SpravichnikiPage';
import { OrganizationsPage } from './pages/Reference/OrganizationsPage';
import { SanatoriumsPage } from './pages/Reference/SanatoriumsPage';
import { NotFoundPage } from './pages/NotFound/NotFoundPage';

export function AppRouter() {
  return (
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
        <Route path="prixod-putevok/organizasii-v2" element={<OrganizasiiV2Page />} />
        <Route path="prixod-putevok/start-process" element={<StartProcessPage />} />
        <Route path="prixod-putevok/sverka" element={<SverkaDannixPage />} />
        <Route path="prixod-putevok/sverka-v2" element={<SverkaDannixV2Page />} />
        <Route path="prixod-putevok/zakritiye-kvartala" element={<ZakritiyaKvartalaPoPutyovkamPage />} />
        <Route path="camp-zayavki" element={<CampZayavkiPage />} />
        <Route path="buxgalter" element={<BuxgalterPage />} />
        <Route path="platejnoe-poruchenie" element={<PlatejnoyePorucheniyePage />} />
        <Route path="otchoty" element={<OtchotPage />} />
        <Route path="admin/users" element={<UsersPage />} />
        <Route path="admin/roles" element={<RolesPage />} />
        <Route path="admin/user-actions" element={<UserActionsPage />} />
        <Route path="admin/spravochniki" element={<SpravichnikiPage />} />
        <Route path="reference/organizations" element={<OrganizationsPage />} />
        <Route path="reference/sanatoriums" element={<SanatoriumsPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  );
}
