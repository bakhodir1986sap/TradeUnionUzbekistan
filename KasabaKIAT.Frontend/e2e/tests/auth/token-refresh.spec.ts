import { test, expect } from '@playwright/test';

// storageState from playwright.config.ts (admin) is applied by default.

test('expired access token is refreshed transparently', async ({ page }) => {
  await page.goto('/dashboard');

  // Force-expire the access token by replacing it with an invalid value.
  // The axios interceptor should detect the 401, call /api/auth/refresh,
  // and retry the original request automatically.
  await page.evaluate(() => {
    sessionStorage.setItem('access_token', 'expired.token.value');
  });

  // Navigate to a page that makes an API call on load
  await page.goto('/camp-zayavki');

  // The page should still render correctly (not redirect to /login)
  await expect(page).toHaveURL(/camp-zayavki/);
  await expect(page.locator('.ant-table')).toBeVisible();
});

test('invalid refresh token causes redirect to login', async ({ page }) => {
  await page.goto('/dashboard');

  // Wipe both tokens so refresh will fail
  await page.evaluate(() => {
    sessionStorage.setItem('access_token', 'expired.token.value');
    localStorage.setItem('refresh_token', 'invalid.refresh.token');
  });

  await page.goto('/camp-zayavki');

  // After refresh fails the app should redirect to login
  await expect(page).toHaveURL(/login/, { timeout: 10_000 });
});
