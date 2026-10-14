import React, { createContext, useContext, useState, useCallback } from 'react';
import { authApi, type LoginRequest, type LoginResponse } from '../api/auth';

interface CurrentUser {
  id: number;
  email: string;
  userName: string;
  role: string;
  bkutId: string;
  isSuperAdmin: boolean;
}

interface AuthContextValue {
  user: CurrentUser | null;
  isAuthenticated: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

function loadUser(): CurrentUser | null {
  try {
    const raw = localStorage.getItem('current_user');
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<CurrentUser | null>(loadUser);

  const login = useCallback(async (credentials: LoginRequest) => {
    const resp: LoginResponse = await authApi.login(credentials);
    sessionStorage.setItem('access_token', resp.accessToken);
    localStorage.setItem('refresh_token', resp.refreshToken);
    localStorage.setItem('current_user', JSON.stringify(resp.user));
    setUser(resp.user);
  }, []);

  const logout = useCallback(async () => {
    try { await authApi.logout(); } catch { /* ignore */ }
    sessionStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('current_user');
    setUser(null);
  }, []);

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}
