import { test, expect } from '@playwright/test';

test.describe('Prixod Putevok Excel Export', () => {
  test('Export button is visible and clickable', async ({ page }) => {
    await page.goto('/prixod-putevok/index');
    await expect(page.getByRole('heading', { name: 'Приход путевок КУ' })).toBeVisible();

    const exportBtn = page.getByRole('button', { name: 'Экспорт' });
    await expect(exportBtn).toBeVisible();
    await expect(exportBtn).toBeEnabled();

    // Click and verify no JS error crashes the page
    await exportBtn.click();
    // Brief wait to let any error message appear
    await page.waitForTimeout(1000);

    // Page should still be on the KU invoices page (not redirected away)
    await expect(page).toHaveURL(/prixod-putevok\/index/);
  });
});
