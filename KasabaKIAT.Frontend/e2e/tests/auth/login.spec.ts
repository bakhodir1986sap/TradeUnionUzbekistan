import { test, expect } from '@playwright/test';

// These tests run without storageState — they test the login page itself.
test.use({ storageState: { cookies: [], origins: [] } });

test('valid credentials redirect to dashboard', async ({ page }) => {
  await page.goto('/login');

  await page.getByPlaceholder(/Введите логин|Loginni kiriting/i).fill('admin');
  await page.getByPlaceholder(/Введите пароль|Parolni kiriting/i).fill('admin');
  await page.getByRole('button', { name: /Войти|Kirish/i }).click();

  await expect(page).toHaveURL(/dashboard/);
});

test('invalid credentials show error alert', async ({ page }) => {
  await page.goto('/login');
  await page.waitForSelector('input[placeholder]');

  await page.getByPlaceholder(/Введите логин|Loginni kiriting/i).fill('testadmin');
  await page.getByPlaceholder(/Введите пароль|Parolni kiriting/i).fill('wrongpassword');

  // Intercept the login API call to confirm it returns 401
  const [response] = await Promise.all([
    page.waitForResponse((r) => r.url().includes('/api/auth/login')),
    page.getByRole('button', { name: /Войти|Kirish/i }).click(),
  ]);

  expect(response.status()).toBe(401);
  // User should remain on login page
  await expect(page).toHaveURL(/login/);
});

test('empty form shows required field validation', async ({ page }) => {
  await page.goto('/login');

  await page.getByRole('button', { name: /Войти|Kirish/i }).click();

  // Ant Design shows validation messages next to the fields
  const validationMessages = page.locator('.ant-form-item-explain-error');
  await expect(validationMessages.first()).toBeVisible();
});

test('unauthenticated access to protected page redirects to login', async ({ page }) => {
  await page.goto('/dashboard');
  await expect(page).toHaveURL(/login/);
});

test('redirects back to original page after login', async ({ page }) => {
  // Try to visit a protected page
  await page.goto('/camp-zayavki');
  await expect(page).toHaveURL(/login/);

  // Login
  await page.getByPlaceholder(/Введите логин|Loginni kiriting/i).fill('admin');
  await page.getByPlaceholder(/Введите пароль|Parolni kiriting/i).fill('admin');
  await page.getByRole('button', { name: /Войти|Kirish/i }).click();

  // Should land back on the originally requested page
  await expect(page).toHaveURL(/camp-zayavki/);
});
