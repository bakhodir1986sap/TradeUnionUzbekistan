import { useRef, useState } from 'react';
import { Button, Space, Typography, App, Divider } from 'antd';
import { useMutation } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import api from '../../api/axios';

const { Title } = Typography;

export function StartProcessPage() {
  const { message } = App.useApp();

  // Путевки
  const [selectedFilePutyovka, setSelectedFilePutyovka] = useState<File | null>(null);
  const putyovkaInputRef = useRef<HTMLInputElement>(null);

  // Кенгаш Путевки
  const [selectedFileKengashPutyovka, setSelectedFileKengashPutyovka] = useState<File | null>(null);
  const putyovkaKengashInputRef = useRef<HTMLInputElement>(null);

  // Баланс
  const [selectedFileBalance, setSelectedFileBalance] = useState<File | null>(null);
  const balanceInputRef = useRef<HTMLInputElement>(null);

  // Дебиторка / Организации
  const [selectedFileOrganization, setSelectedFileOrganization] = useState<File | null>(null);
  const debitCreditInputRef = useRef<HTMLInputElement>(null);

  // --- Download template helpers ---
  const downloadFile = async (url: string, filename: string) => {
    try {
      const res = await api.get(url, { responseType: 'blob' });
      const blobUrl = window.URL.createObjectURL(new Blob([res.data]));
      const link = document.createElement('a');
      link.href = blobUrl;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(blobUrl);
    } catch {
      message.error('Ошибка при загрузке шаблона');
    }
  };

  // --- Mutations ---
  const parsePutyovkaMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append('PutyovkaFile', file);
      const res = await api.post('/api/prixod-putevok/parse-excel-putyovka', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return res.data;
    },
    onSuccess: () => {
      message.success('Путевки успешно загружены');
      setSelectedFilePutyovka(null);
      if (putyovkaInputRef.current) putyovkaInputRef.current.value = '';
    },
    onError: () => {
      message.error('Ошибка при загрузке путевок');
    },
  });

  const parsePutyovkaKengashMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append('PutyovkaFile', file);
      const res = await api.post('/api/prixod-putevok/parse-excel-putyovka-kengash', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return res.data;
    },
    onSuccess: () => {
      message.success('Кенгаш путевки успешно загружены');
      setSelectedFileKengashPutyovka(null);
      if (putyovkaKengashInputRef.current) putyovkaKengashInputRef.current.value = '';
    },
    onError: () => {
      message.error('Ошибка при загрузке кенгаш путевок');
    },
  });

  const parseBalanceMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append('BalanceFile', file);
      const res = await api.post('/api/prixod-putevok/parse-excel-balance', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return res.data;
    },
    onSuccess: () => {
      message.success('Баланс успешно загружен');
      setSelectedFileBalance(null);
      if (balanceInputRef.current) balanceInputRef.current.value = '';
    },
    onError: () => {
      message.error('Ошибка при загрузке баланса');
    },
  });

  const parseDebitCreditMutation = useMutation({
    mutationFn: async (file: File) => {
      const formData = new FormData();
      formData.append('DebitCreditFile', file);
      const res = await api.post('/api/prixod-putevok/parse-excel-debit-credit', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      return res.data;
    },
    onSuccess: () => {
      message.success('Дебиторка успешно загружена');
      setSelectedFileOrganization(null);
      if (debitCreditInputRef.current) debitCreditInputRef.current.value = '';
    },
    onError: () => {
      message.error('Ошибка при загрузке дебиторки');
    },
  });

  const excelAccept =
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.ms-excel';

  return (
    <div style={{ padding: 24 }}>
      <Title level={4}>Загрузка первичных остатков</Title>

      <nav aria-label="breadcrumb" style={{ marginBottom: 24 }}>
        <ol style={{ display: 'flex', listStyle: 'none', padding: 0, gap: 8, margin: 0 }}>
          <li>
            <Link to="/buxgalter">Бухгалтерия</Link>
          </li>
          <li style={{ color: '#999' }}>/</li>
          <li style={{ color: '#333' }}>Загрузка первичных остатков</li>
        </ol>
      </nav>

      {/* Путевки */}
      <div style={{ marginBottom: 16 }}>
        <Space wrap align="center">
          <Button
            type="link"
            style={{ paddingLeft: 0 }}
            onClick={() =>
              downloadFile(
                '/api/prixod-putevok/download-putyovki-template',
                'PutyovkiTemplate.xlsx'
              )
            }
          >
            Загрузить Путевки Шаблон
          </Button>

          <input
            ref={putyovkaInputRef}
            type="file"
            accept={excelAccept}
            onChange={(e) => setSelectedFilePutyovka(e.target.files?.[0] ?? null)}
          />

          <Button
            type="primary"
            disabled={!selectedFilePutyovka}
            loading={parsePutyovkaMutation.isPending}
            onClick={() => {
              if (selectedFilePutyovka) parsePutyovkaMutation.mutate(selectedFilePutyovka);
            }}
          >
            Загрузить Путевки
          </Button>

          <Link to="/prixod-putevok/prixod-putyovok-ruchnoy">Приход путевок ручной</Link>
        </Space>
      </div>

      <Divider />

      {/* Кенгаш Путевки */}
      <div style={{ marginBottom: 16 }}>
        <Space wrap align="center">
          <Button
            type="link"
            style={{ paddingLeft: 0 }}
            onClick={() =>
              downloadFile(
                '/api/prixod-putevok/download-putyovki-kengash-template',
                'PutyovkiKengashTemplate.xlsx'
              )
            }
          >
            Загрузить Кенгаш Путевки Шаблон
          </Button>

          <input
            ref={putyovkaKengashInputRef}
            type="file"
            accept={excelAccept}
            onChange={(e) => setSelectedFileKengashPutyovka(e.target.files?.[0] ?? null)}
          />

          <Button
            type="primary"
            disabled={!selectedFileKengashPutyovka}
            loading={parsePutyovkaKengashMutation.isPending}
            onClick={() => {
              if (selectedFileKengashPutyovka)
                parsePutyovkaKengashMutation.mutate(selectedFileKengashPutyovka);
            }}
          >
            Загрузить Путевки
          </Button>
        </Space>
      </div>

      <Divider />

      {/* Баланс */}
      <div style={{ marginBottom: 16 }}>
        <Space wrap align="center">
          <Button
            type="link"
            style={{ paddingLeft: 0 }}
            onClick={() =>
              downloadFile(
                '/api/prixod-putevok/download-balance-template',
                'BalanceTemplate.xlsx'
              )
            }
          >
            Загрузить Баланс Шаблон
          </Button>

          <input
            ref={balanceInputRef}
            type="file"
            accept={excelAccept}
            onChange={(e) => setSelectedFileBalance(e.target.files?.[0] ?? null)}
          />

          <Button
            type="primary"
            disabled={!selectedFileBalance}
            loading={parseBalanceMutation.isPending}
            onClick={() => {
              if (selectedFileBalance) parseBalanceMutation.mutate(selectedFileBalance);
            }}
          >
            Загрузить Баланс
          </Button>

          <Link to="/otchot/glavnaya-kniga">Главная Книга</Link>
        </Space>
      </div>

      <Divider />

      {/* Дебиторка / Организации */}
      <div style={{ marginBottom: 16 }}>
        <Space wrap align="center">
          <Button
            type="link"
            style={{ paddingLeft: 0 }}
            onClick={() =>
              downloadFile(
                '/api/prixod-putevok/download-debitor-template',
                'DebitorTemplate.xlsx'
              )
            }
          >
            Загрузить Организ. Шаблон
          </Button>

          <input
            ref={debitCreditInputRef}
            type="file"
            accept={excelAccept}
            onChange={(e) => setSelectedFileOrganization(e.target.files?.[0] ?? null)}
          />

          <Button
            type="primary"
            disabled={!selectedFileOrganization}
            loading={parseDebitCreditMutation.isPending}
            onClick={() => {
              if (selectedFileOrganization)
                parseDebitCreditMutation.mutate(selectedFileOrganization);
            }}
          >
            Загрузить Дебиторку
          </Button>

          <Link to="/otchot/otchet-titul">Отчеты</Link>
        </Space>
      </div>
    </div>
  );
}
