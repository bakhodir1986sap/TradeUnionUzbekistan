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
}

export interface OrgItem {
  id: number;
  name_Org?: string;
  inn?: string;
  id_Ot?: number | null;
  departmentBase?: string;
  address?: string;
  name_Short?: string;
  name_Full?: string;
  soato_ID?: string;
  owner_CODE?: string;
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
};
