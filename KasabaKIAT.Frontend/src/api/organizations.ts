import api from './axios';

export interface OtraslOption { id: number; name_Ot: string }
export interface RegionOption { id: number; name_Reg: string; code?: string }

export interface OrgListParams {
  name?: string;
  inn?: string;
  otraslId?: number | null;
  page?: number;
  pageSize?: number;
}

export interface OrgListResult {
  items: OrgItem[];
  count: number;
  totalCountNeot: number;
  totalCountOpr: number;
}

export interface OrgItem {
  id: number;
  name_Org?: string;
  inn?: string;
  id_Ot?: number | null;
  departmentBase?: string;
  adress?: string;
  rs?: string;
  mfo?: string;
  bank?: string;
  okonx?: string;
  telefon?: string;
  region?: number | null;
  e_Mail?: string;
  owner_CODE?: string;
  director?: string;
  buxgalter?: string;
  doverennoe_Liso?: string;
  orig_INN?: string;
  to_DepCode?: string;
  face_Type?: number | null;
  pas_ser?: string;
  pas_Num?: string;
  pas_KD?: string;
  pas_Data?: string | null;
}

export const organizationsApi = {
  getAll: (params?: { search?: string; otrasId?: number }) =>
    api.get('/api/organizations', { params }).then((r) => r.data),
  getList: (params?: OrgListParams): Promise<OrgListResult> =>
    api.get('/api/organizations', { params }).then((r) => r.data),
  getById: (id: number) => api.get(`/api/organizations/${id}`).then((r) => r.data),
  getByInn: (inn: string) => api.get(`/api/organizations/by-inn/${inn}`).then((r) => r.data),
  getOtrasls: (): Promise<OtraslOption[]> =>
    api.get('/api/organizations/otrasl').then((r) => r.data),
  getRegions: (): Promise<RegionOption[]> =>
    api.get('/api/organizations/regions').then((r) => r.data),
  create: (data: unknown) => api.post('/api/organizations', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/organizations/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/organizations/${id}`),
  changeOtraslMassive: (ids: number[], otraslId: number) =>
    api.post('/api/organizations/change-otrasl-massive', { ids, otraslId }).then((r) => r.data),
  export: (params?: { name?: string; inn?: string; otraslId?: number | null }) =>
    api.get('/api/organizations/export', { params, responseType: 'blob' }).then((r) => r.data),
  uploadOtrasl: (file: File) => {
    const fd = new FormData();
    fd.append('OtraslFile', file);
    return api.post('/api/organizations/upload-otrasl', fd, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },
};

