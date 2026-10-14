import api from './axios';

export const putyovkiApi = {
  getAll: (params?: Record<string, unknown>) =>
    api.get('/api/putyovki', { params }).then((r) => r.data),
  getById: (id: number) => api.get(`/api/putyovki/${id}`).then((r) => r.data),
  create: (data: unknown) => api.post('/api/putyovki', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/putyovki/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/putyovki/${id}`),
};
