import api from './axios';

export const campZayavkiApi = {
  getAll: (params?: Record<string, unknown>) =>
    api.get('/api/camp-zayavki', { params }).then((r) => r.data),
  getById: (id: number) => api.get(`/api/camp-zayavki/${id}`).then((r) => r.data),
  create: (data: unknown) => api.post('/api/camp-zayavki', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/camp-zayavki/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/camp-zayavki/${id}`),
  updateStatus: (id: number, status: string) =>
    api.patch(`/api/camp-zayavki/${id}/status`, { status }).then((r) => r.data),
  getCamps: () => api.get('/api/reference/camps').then((r) => r.data),
};
