import { test, expect } from '@playwright/test';

test.describe('Core Reports (Основные отчеты)', () => {
  test('page loads with tabs', async ({ page }) => {
    await page.goto('/otchoty/osnovnye');
    await expect(page.getByRole('heading', { name: 'Основные отчеты' })).toBeVisible({ timeout: 10_000 });
    // Use exact match to avoid matching "Поступления N"
    await expect(page.getByRole('tab', { name: 'Сводный отчет', exact: true })).toBeVisible();
    await expect(page.getByRole('tab', { name: 'Поступления', exact: true })).toBeVisible();
  });

  test('Поступления tab: search loads table without error', async ({ page }) => {
    await page.goto('/otchoty/osnovnye');
    await expect(page.getByRole('heading', { name: 'Основные отчеты' })).toBeVisible({ timeout: 10_000 });

    // Wait for tabs to stabilise — use .ant-tabs-tab to avoid detach issues
    await page.waitForTimeout(500);
    await page.locator('.ant-tabs-tab', { hasText: 'Поступления' }).click({ force: true });

    // Click search with no filters — should load all rows
    await page.getByRole('button', { name: 'Поиск' }).click();

    await expect(page.locator('.ant-table')).toBeVisible({ timeout: 15_000 });
    await expect(page.locator('.ant-spin-spinning')).not.toBeVisible({ timeout: 15_000 });
  });

  test('Сводный отчет: date range search runs without error', async ({ page }) => {
    await page.goto('/otchoty/osnovnye');
    await expect(page.getByRole('heading', { name: 'Основные отчеты' })).toBeVisible({ timeout: 10_000 });

    // Сводный отчет is the first/default tab — wait for it to render
    await page.waitForTimeout(500);
    await page.waitForSelector('.ant-picker-range', { timeout: 15_000 });

    // Fill date range picker
    const inputs = page.locator('.ant-picker-input input');
    await inputs.first().click();
    await inputs.first().fill('2026-01-01');
    await inputs.last().click();
    await inputs.last().fill('2026-12-31');
    await page.keyboard.press('Enter');

    await page.getByRole('button', { name: 'Поиск' }).click();

    await expect(page.locator('.ant-table')).toBeVisible({ timeout: 15_000 });
    await expect(page.locator('.ant-spin-spinning')).not.toBeVisible({ timeout: 15_000 });
    await expect(page.locator('.ant-alert-error')).not.toBeVisible();
  });

  test('navigation to core report sub-routes succeeds', async ({ page }) => {
    const routes = [
      '/otchoty/osnovnye',
      '/otchoty/putyovki',
      '/otchoty/kartochka',
    ];

    for (const route of routes) {
      try {
        await page.goto(route, { waitUntil: 'domcontentloaded', timeout: 15_000 });
      } catch {
        // ERR_ABORTED is acceptable — page may still render via client-side routing
      }
      // If we got redirected to login, the auth session is broken
      const url = page.url();
      if (url.includes('/login')) {
        test.fail(true, `Route ${route} redirected to login — auth session broken`);
        break;
      }
      // Use .first() to avoid strict mode violations when multiple matching elements exist
      await expect(page.locator('.ant-tabs, .ant-table, h4').first()).toBeVisible({ timeout: 10_000 });
    }
  });
});
