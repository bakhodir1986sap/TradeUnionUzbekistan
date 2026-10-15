import { test, expect } from '@playwright/test';

// storageState from playwright.config.ts (admin) is applied by default.

async function performLogout(page: import('@playwright/test').Page) {
  // Click the user avatar + name area in the header to open dropdown
  await page.locator('header').getByText('admin').click();
  // Click "Выход" in the dropdown menu
  await page.getByRole('menuitem', { name: /Выход/i }).click();
  // Confirm the modal dialog
  await page.getByRole('button', { name: /^ОК$|^OK$/i }).click();
}

test('logout redirects to login page', async ({ page }) => {
  await page.goto('/dashboard');
  await page.waitForSelector('header');
  await performLogout(page);
  await expect(page).toHaveURL(/login/);
});

test('after logout, navigating to protected page redirects to login', async ({ page }) => {
  await page.goto('/dashboard');
  await page.waitForSelector('header');
  await performLogout(page);
  await expect(page).toHaveURL(/login/);

  // Try to navigate directly
  await page.goto('/dashboard');
  await expect(page).toHaveURL(/login/);
});

test('logout clears session storage', async ({ page }) => {
  await page.goto('/dashboard');
  await page.waitForSelector('header');

  // Trigger logout and capture storage state before navigation away
  await page.locator('header').getByText('admin').click();
  await page.getByRole('menuitem', { name: /Выход/i }).click();
  await page.getByRole('button', { name: /^ОК$|^OK$/i }).click();

  // Wait for redirect to login
  await expect(page).toHaveURL(/login/);

  // After logout the app calls clearSession() which removes tokens
  // The login page itself doesn't restore storage, so tokens should be gone
  const token = await page.evaluate(() => sessionStorage.getItem('access_token'));
  expect(token).toBeNull();

  const user = await page.evaluate(() => localStorage.getItem('current_user'));
  expect(user).toBeNull();
});
