import { test, expect } from '@playwright/test';

test.describe('Prixod Putevok Balance (Ostatki)', () => {
  test('balance page loads and renders table', async ({ page }) => {
    await page.goto('/prixod-putevok/ostatki');
    await expect(page.getByRole('heading', { name: 'Просмотр остатков путевок' })).toBeVisible({ timeout: 15_000 });
    await expect(page.locator('.ant-table')).toBeVisible({ timeout: 15_000 });
  });

  test('balance page does not show error alert on load', async ({ page }) => {
    await page.goto('/prixod-putevok/ostatki');
    await expect(page.getByRole('heading', { name: 'Просмотр остатков путевок' })).toBeVisible({ timeout: 15_000 });

    const errorAlert = page.locator('.ant-alert-error');
    await expect(errorAlert).not.toBeVisible();
  });
});
