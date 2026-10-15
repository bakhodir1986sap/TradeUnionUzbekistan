import axios from 'axios';

const BACKEND_URL = process.env.BACKEND_URL ?? 'http://localhost:5166';

let cachedToken: string | null = null;
let tokenExpiry = 0;

async function getAdminToken(): Promise<string> {
  const now = Date.now();
  if (cachedToken && now < tokenExpiry) return cachedToken;

  const res = await axios.post(`${BACKEND_URL}/api/auth/login`, {
    login: process.env.E2E_ADMIN_USER ?? 'admin',
    password: process.env.E2E_ADMIN_PASS ?? 'admin',
  });
  cachedToken = res.data.accessToken as string;
  // Access token expires in 10 minutes — cache for 8 minutes
  tokenExpiry = now + 8 * 60 * 1000;
  return cachedToken;
}

function makeClient(token: string) {
  return axios.create({
    baseURL: BACKEND_URL,
    headers: { Authorization: `Bearer ${token}` },
  });
}

export async function createAdminClient() {
  const token = await getAdminToken();
  return makeClient(token);
}

export async function createCampZayavka(data: {
  campId?: number;
  serialBegin?: string;
  serialEnd?: string;
  restBeginDate?: string;
  restEndDate?: string;
  price?: number;
  quantity?: number;
}) {
  const client = await createAdminClient();
  const payload = {
    campId: data.campId ?? 1,
    serialBegin: data.serialBegin ?? 'E2E001',
    serialEnd: data.serialEnd ?? 'E2E010',
    restBeginDate: data.restBeginDate ?? '2026-06-01T00:00:00',
    restEndDate: data.restEndDate ?? '2026-06-14T00:00:00',
    price: data.price ?? 1000,
    quantity: data.quantity ?? 5,
  };
  const res = await client.post('/api/camp-zayavki', payload);
  return res.data as { id: number };
}

export async function deleteCampZayavka(id: number) {
  const client = await createAdminClient();
  await client.delete(`/api/camp-zayavki/${id}`);
}

export async function updateCampZayavkaStatus(id: number, status: string) {
  const client = await createAdminClient();
  await client.patch(`/api/camp-zayavki/${id}/status`, { status });
}

export async function cleanupE2ECampZayavki() {
  // Always get a fresh token for cleanup to avoid cached/revoked token issues
  const res = await axios.post(`${BACKEND_URL}/api/auth/login`, {
    login: process.env.E2E_ADMIN_USER ?? 'admin',
    password: process.env.E2E_ADMIN_PASS ?? 'admin',
  });
  const token = res.data.accessToken as string;
  const client = axios.create({
    baseURL: BACKEND_URL,
    headers: { Authorization: `Bearer ${token}` },
  });
  // Fetch all records (no pagination — the API returns all by default)
  const listRes = await client.get('/api/camp-zayavki');
  const records = listRes.data as { id: number; serialBegin: string }[];
  const testPrefixes = ['E2ETEST', 'E2E001', 'DEL001', 'WF001', 'WF002', 'WF003', 'E2E-', 'DEL-', 'WF1-', 'WF2-', 'WF3-', 'E2E0'];
  for (const rec of records) {
    if (testPrefixes.some(p => rec.serialBegin?.startsWith(p))) {
      await client.delete(`/api/camp-zayavki/${rec.id}`);
    }
  }
  // Update module cache with new token
  cachedToken = token;
  tokenExpiry = Date.now() + 8 * 60 * 1000;
}
