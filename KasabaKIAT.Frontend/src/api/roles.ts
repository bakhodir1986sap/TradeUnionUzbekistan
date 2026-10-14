import api from './axios';

export const rolesApi = {
  getAll: () => api.get('/api/roles').then((r) => r.data),
  getById: (id: number) => api.get(`/api/roles/${id}`).then((r) => r.data),
  create: (data: unknown) => api.post('/api/roles', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/roles/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/roles/${id}`),
};
