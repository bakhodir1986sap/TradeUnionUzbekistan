import api from './axios';

export const organizationsApi = {
  getAll: (params?: { search?: string; otrasId?: number }) =>
    api.get('/api/organizations', { params }).then((r) => r.data),
  getById: (id: number) => api.get(`/api/organizations/${id}`).then((r) => r.data),
  getByInn: (inn: string) => api.get(`/api/organizations/by-inn/${inn}`).then((r) => r.data),
  create: (data: unknown) => api.post('/api/organizations', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/organizations/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/organizations/${id}`),
};
