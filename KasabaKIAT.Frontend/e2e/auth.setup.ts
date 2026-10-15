import { test as setup } from '@playwright/test';
import path from 'path';
import fs from 'fs/promises';

const adminFile = path.join('e2e', '.auth', 'admin.json');

setup('authenticate as admin', async ({ page }) => {
  await page.goto('/login');

  await page.getByPlaceholder(/Введите логин|Loginni kiriting/i).fill(process.env.E2E_ADMIN_USER ?? 'admin');
  await page.getByPlaceholder(/Введите пароль|Parolni kiriting/i).fill(process.env.E2E_ADMIN_PASS ?? 'admin');
  await page.getByRole('button', { name: /Войти|Kirish/i }).click();

  await page.waitForURL('**/dashboard');

  // Get access_token from sessionStorage — Playwright storageState doesn't always capture it
  const accessToken = await page.evaluate(() => sessionStorage.getItem('access_token'));

  // Save storageState (captures localStorage including refresh_token and current_user)
  await page.context().storageState({ path: adminFile });

  // Patch the saved state to include sessionStorage with access_token
  if (accessToken) {
    const state = JSON.parse(await fs.readFile(adminFile, 'utf8'));
    const origin = state.origins.find((o: { origin: string }) => o.origin === 'http://localhost:5173');
    if (origin) {
      origin.sessionStorage = [{ name: 'access_token', value: accessToken }];
    }
    await fs.writeFile(adminFile, JSON.stringify(state, null, 2));
  }
});
