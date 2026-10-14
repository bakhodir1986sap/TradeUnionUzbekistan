import api from './axios';

export interface LoginRequest {
  login: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: {
    id: number;
    email: string;
    userName: string;
    role: string;
    bkutId: string;
    isSuperAdmin: boolean;
  };
}

export const authApi = {
  login: (data: LoginRequest) =>
    api.post<LoginResponse>('/api/auth/login', data).then((r) => r.data),

  refresh: (refreshToken: string) =>
    api.post<LoginResponse>('/api/auth/refresh', { refreshToken }).then((r) => r.data),

  logout: () => api.post('/api/auth/logout'),
};
