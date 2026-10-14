import { BrowserRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ConfigProvider, App as AntApp } from 'antd';
import ruRU from 'antd/locale/ru_RU';
import uzUZ from 'antd/locale/uz_UZ';
import { useTranslation } from 'react-i18next';
import { AuthProvider } from './auth/AuthContext';
import { AppRouter } from './router';
import { ErrorBoundary } from './components/ErrorBoundary';
import './i18n';
import 'antd/dist/reset.css';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: { retry: 1, staleTime: 30_000 },
  },
});

const antdLocales: Record<string, typeof ruRU> = { ru: ruRU, uz: uzUZ };

export default function App() {
  const { i18n } = useTranslation();
  const locale = antdLocales[i18n.language] ?? ruRU;

  return (
    <QueryClientProvider client={queryClient}>
      <ConfigProvider locale={locale}>
        <AntApp>
          <ErrorBoundary>
            <BrowserRouter>
              <AuthProvider>
                <AppRouter />
              </AuthProvider>
            </BrowserRouter>
          </ErrorBoundary>
        </AntApp>
      </ConfigProvider>
    </QueryClientProvider>
  );
}
