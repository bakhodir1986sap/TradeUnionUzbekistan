import api from './axios';

export const godovoyPlanApi = {
  getAll: (params?: Record<string, unknown>) =>
    api.get('/api/godovoy-plan', { params }).then((r) => r.data),
  getById: (id: number) => api.get(`/api/godovoy-plan/${id}`).then((r) => r.data),
  create: (data: unknown) => api.post('/api/godovoy-plan', data).then((r) => r.data),
  update: (id: number, data: unknown) => api.put(`/api/godovoy-plan/${id}`, data).then((r) => r.data),
  delete: (id: number) => api.delete(`/api/godovoy-plan/${id}`),
};
