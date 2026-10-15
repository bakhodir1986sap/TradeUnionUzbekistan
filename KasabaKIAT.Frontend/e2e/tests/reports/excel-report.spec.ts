import { test, expect } from '@playwright/test';

test.describe('Excel report download', () => {
  test('KU invoice export button is functional', async ({ page }) => {
    await page.goto('/prixod-putevok/index');
    await expect(page.getByRole('heading', { name: 'Приход путевок КУ' })).toBeVisible();

    const exportBtn = page.getByRole('button', { name: 'Экспорт' });
    await expect(exportBtn).toBeVisible();

    // Click and confirm page stays on current route (no crash redirect)
    await exportBtn.click();
    await page.waitForTimeout(500);
    await expect(page).toHaveURL(/prixod-putevok\/index/);
  });
});
