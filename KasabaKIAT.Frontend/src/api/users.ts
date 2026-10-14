import api from './axios';

export const usersApi = {
  getAll: (params?: { page?: number; pageSize?: number; search?: string }) =>
    api.get('/api/users', { params }).then((r) => r.data),

  getMe: () => api.get('/api/users/me').then((r) => r.data),

  getById: (id: number) => api.get(`/api/users/${id}`).then((r) => r.data),

  create: (data: unknown) => api.post('/api/users', data).then((r) => r.data),

  update: (id: number, data: unknown) => api.put(`/api/users/${id}`, data).then((r) => r.data),

  delete: (id: number) => api.delete(`/api/users/${id}`),
};
