import api from './axios';

export const otchotApi = {
  // --- Core ---
  getSvodniyOtchot: (p?: Record<string, unknown>) =>
    api.get('/api/reports/svodniy-otchot', { params: p }).then(r => r.data),
  getPostupleniya: (p?: Record<string, unknown>) =>
    api.get('/api/reports/postupleniya', { params: p }).then(r => r.data),
  getPostupleniyaN: (p?: Record<string, unknown>) =>
    api.get('/api/reports/postupleniya-n', { params: p }).then(r => r.data),
  getProvodkiDetails: (p?: Record<string, unknown>) =>
    api.get('/api/reports/provodki-details', { params: p }).then(r => r.data),
  getProvodkiSummary: (p?: Record<string, unknown>) =>
    api.get('/api/reports/provodki-summary', { params: p }).then(r => r.data),
  getTalabnomalarKorxona: (p?: Record<string, unknown>) =>
    api.get('/api/reports/talabnomalar-korxona', { params: p }).then(r => r.data),
  getTalabnomalarTarmoq: (p?: Record<string, unknown>) =>
    api.get('/api/reports/talabnomalar-tarmoq', { params: p }).then(r => r.data),
  getForma13: (p?: Record<string, unknown>) =>
    api.get('/api/reports/forma13', { params: p }).then(r => r.data),

  // --- Group A: Putyovki ---
  getOperativkaPutyovki: (p?: Record<string, unknown>) =>
    api.get('/api/reports/operativka-putyovki', { params: p }).then(r => r.data),
  getOperativkaOstatok: (p?: Record<string, unknown>) =>
    api.get('/api/reports/operativka-ostatok', { params: p }).then(r => r.data),
  getOperativkaPrixod: (p?: Record<string, unknown>) =>
    api.get('/api/reports/operativka-prixod', { params: p }).then(r => r.data),
  getOperativkaRasxod: (p?: Record<string, unknown>) =>
    api.get('/api/reports/operativka-rasxod', { params: p }).then(r => r.data),
  getOperativkaVozvrat: (p?: Record<string, unknown>) =>
    api.get('/api/reports/operativka-vozvrat', { params: p }).then(r => r.data),
  getSvodPoPutyovkam: (p?: Record<string, unknown>) =>
    api.get('/api/reports/svod-po-putyovkam', { params: p }).then(r => r.data),
  getSvodniyOtchetPutyovki: (p?: Record<string, unknown>) =>
    api.get('/api/reports/svodny-otchet-putyovki', { params: p }).then(r => r.data),
  getZaprosPutyovki: (p?: Record<string, unknown>) =>
    api.get('/api/reports/zapros-putyovki', { params: p }).then(r => r.data),
  getKnigaVladelsev: (p?: Record<string, unknown>) =>
    api.get('/api/reports/kniga-vladelsev', { params: p }).then(r => r.data),
  getForma14: (p?: Record<string, unknown>) =>
    api.get('/api/reports/forma14', { params: p }).then(r => r.data),
  getTekushiyeOstatok: () =>
    api.get('/api/reports/tekushiye-ostatok').then(r => r.data),

  // --- Group B: Financial ---
  getPostupleniyaEspOtrasli: (p?: Record<string, unknown>) =>
    api.get('/api/reports/postupleniya-esp-otrasli', { params: p }).then(r => r.data),
  getPostupleniyaSvodnoye: (p?: Record<string, unknown>) =>
    api.get('/api/reports/postupleniya-svodnoye', { params: p }).then(r => r.data),
  getOborotkaRegionam: (p?: Record<string, unknown>) =>
    api.get('/api/reports/oborotka-regionam', { params: p }).then(r => r.data),
  getBalansPredpriyatiy: (p?: Record<string, unknown>) =>
    api.get('/api/reports/balans-predpriyatiy', { params: p }).then(r => r.data),
  getDebitorskaya: () =>
    api.get('/api/reports/debitorskaya').then(r => r.data),
  getOrganizasiiOtrasli: () =>
    api.get('/api/reports/organizasii-otrasli').then(r => r.data),
  getOrganizasiiRayony: () =>
    api.get('/api/reports/organizasii-rayony').then(r => r.data),
  getSpisokOrg: () =>
    api.get('/api/reports/spisok-org').then(r => r.data),

  // --- Group C: Journal Orders ---
  getJurOrder4: (p?: Record<string, unknown>) =>
    api.get('/api/reports/jurorder/4', { params: p }).then(r => r.data),
  getJurOrder5: (p?: Record<string, unknown>) =>
    api.get('/api/reports/jurorder/5', { params: p }).then(r => r.data),
  getJurOrder6: (p?: Record<string, unknown>) =>
    api.get('/api/reports/jurorder/6', { params: p }).then(r => r.data),
  getJurOrder: (p?: Record<string, unknown>) =>
    api.get('/api/reports/jurorder', { params: p }).then(r => r.data),

  // --- Group D: Monitoring ---
  getMonitoringOxvat: (p?: Record<string, unknown>) =>
    api.get('/api/reports/monitoring/oxvat', { params: p }).then(r => r.data),
  getMonitoringNarusheniya: (p?: Record<string, unknown>) =>
    api.get('/api/reports/monitoring/narusheniya', { params: p }).then(r => r.data),
  getMonitoringPererasxodPlan: (p?: Record<string, unknown>) =>
    api.get('/api/reports/monitoring/pererasxod-plan', { params: p }).then(r => r.data),
  getMonitoringPererasxodPutyovki: (p?: Record<string, unknown>) =>
    api.get('/api/reports/monitoring/pererasxod-putyovki', { params: p }).then(r => r.data),
  getMonitoringKaznacheystvo: (p?: Record<string, unknown>) =>
    api.get('/api/reports/monitoring/kaznacheystvo', { params: p }).then(r => r.data),

  // --- Group E: Org Cards ---
  getKartochkaOrg: (p?: Record<string, unknown>) =>
    api.get('/api/reports/kartochka-org', { params: p }).then(r => r.data),
  getKartochkaHistory: (p?: Record<string, unknown>) =>
    api.get('/api/reports/kartochka-history', { params: p }).then(r => r.data),
  getKartochkaOplata: (p?: Record<string, unknown>) =>
    api.get('/api/reports/kartochka-oplata', { params: p }).then(r => r.data),

  // --- Group F: Medical / Other ---
  getMedRegistraciya: (p?: Record<string, unknown>) =>
    api.get('/api/reports/med-registraciya', { params: p }).then(r => r.data),
  getUchetStoimosti: (p?: Record<string, unknown>) =>
    api.get('/api/reports/uchet-stoimosti', { params: p }).then(r => r.data),
  getPoiskPutyovki: (p?: Record<string, unknown>) =>
    api.get('/api/reports/poisk-putyovki', { params: p }).then(r => r.data),
  getOstatokNizhestvuyushix: () =>
    api.get('/api/reports/ostatok-nizhestvuyushix').then(r => r.data),
  getTalabnomalarViloyat: (p?: Record<string, unknown>) =>
    api.get('/api/reports/talabnomalar-viloyat', { params: p }).then(r => r.data),

  // --- Group G: FinOtchet / GlavnayaKniga ---
  getFinOtchet: (p?: Record<string, unknown>) =>
    api.get('/api/reports/fin-otchet', { params: p }).then(r => r.data),
  getGlavnayaKniga: (p?: Record<string, unknown>) =>
    api.get('/api/reports/glavnaya-kniga', { params: p }).then(r => r.data),
};
