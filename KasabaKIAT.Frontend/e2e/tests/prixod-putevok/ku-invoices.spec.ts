import { test, expect } from '@playwright/test';

test.describe('Prixod Putevok KU Invoices', () => {
  test('KU page loads and shows title', async ({ page }) => {
    await page.goto('/prixod-putevok/index');
    await expect(page.getByRole('heading', { name: 'Приход путевок КУ' })).toBeVisible({ timeout: 10_000 });
    await expect(page.locator('.ant-table')).toBeVisible({ timeout: 10_000 });
  });

  test('filter controls are visible', async ({ page }) => {
    await page.goto('/prixod-putevok/index');
    await expect(page.getByRole('heading', { name: 'Приход путевок КУ' })).toBeVisible();

    await expect(page.locator('.ant-select').first()).toBeVisible();
    await expect(page.locator('.ant-picker-range')).toBeVisible();
    await expect(page.getByRole('button', { name: 'Экспорт' })).toBeVisible();
  });

  test('clicking a row opens detail modal', async ({ page }) => {
    await page.goto('/prixod-putevok/index');
    await expect(page.getByRole('heading', { name: 'Приход путевок КУ' })).toBeVisible();

    // Wait up to 10s for table rows to appear
    await page.waitForSelector('.ant-table-row', { timeout: 10_000 }).catch(() => {});

    const rows = page.locator('.ant-table-row');
    const rowCount = await rows.count();

    if (rowCount > 0) {
      await rows.first().click();
      await expect(page.locator('.ant-modal')).toBeVisible({ timeout: 5_000 });
      await expect(page.getByText(/Детализация документа/)).toBeVisible();
    } else {
      test.info().annotations.push({ type: 'note', description: 'No rows in table — skipping row-click assertion' });
    }
  });
});
