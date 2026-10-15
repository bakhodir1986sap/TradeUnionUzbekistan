import { test, expect } from '@playwright/test';
import { createCampZayavka, deleteCampZayavka, cleanupE2ECampZayavki } from '../../fixtures/api-client';

// Use a unique prefix per test run to avoid cross-run collisions
const RUN_ID = Date.now().toString().slice(-6);

test.describe('Camp Zayavki CRUD', () => {
  const createdIds: number[] = [];

  test.beforeAll(async () => {
    // Clean up leftover test data from previous runs to avoid pagination issues
    await cleanupE2ECampZayavki();
  });

  test.afterAll(async () => {
    for (const id of createdIds) {
      await deleteCampZayavka(id).catch(() => {});
    }
  });

  test('newly seeded zayavka appears in New tab', async ({ page }) => {
    const serial = `E2E-${RUN_ID}`;
    const record = await createCampZayavka({ campId: 1, serialBegin: serial, quantity: 5 });
    createdIds.push(record.id);

    await page.goto('/camp-zayavki');
    await expect(page.getByRole('tab', { name: 'Новые' })).toHaveAttribute('aria-selected', 'true', { timeout: 10_000 });

    await expect(page.locator('table')).toBeVisible({ timeout: 10_000 });
    // Use the seeded record's unique serial — find the first matching cell
    await expect(page.getByRole('cell', { name: serial }).first()).toBeVisible({ timeout: 5_000 });
  });

  test('create zayavka via UI modal', async ({ page }) => {
    await page.goto('/camp-zayavki');
    // Wait for the page to stabilise before clicking the Add button
    await expect(page.getByRole('heading').first()).toBeVisible({ timeout: 10_000 });
    await page.waitForTimeout(1000);

    await page.getByRole('button', { name: /добавить|add/i }).click({ force: true });

    // Wait for modal to open
    await expect(page.getByRole('dialog')).toBeVisible({ timeout: 5_000 });

    // Select first available camp option
    await page.getByRole('combobox').click();
    await page.locator('.ant-select-item-option').first().click();

    await page.getByRole('button', { name: /^ОК$|^OK$/i }).click();
    await expect(page.getByRole('table')).toBeVisible({ timeout: 10_000 });
  });

  test('delete zayavka via UI confirm dialog', async ({ page }) => {
    const serial = `DEL-${RUN_ID}`;
    const record = await createCampZayavka({ campId: 1, serialBegin: serial, quantity: 3 });
    createdIds.push(record.id);

    await page.goto('/camp-zayavki');
    await expect(page.getByRole('cell', { name: serial }).first()).toBeVisible({ timeout: 10_000 });

    const row = page.locator('tr', { hasText: serial }).first();
    const deleteBtn = row.getByRole('button').last();
    await deleteBtn.click();

    // Confirm the delete dialog
    await page.getByRole('button', { name: /^ОК$|^OK$/i }).click();

    await expect(page.getByRole('cell', { name: serial })).not.toBeVisible({ timeout: 5_000 });
    // Remove from cleanup since deleted
    const idx = createdIds.indexOf(record.id);
    if (idx !== -1) createdIds.splice(idx, 1);
  });
});
