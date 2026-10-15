import { test, expect } from '@playwright/test';
import { createCampZayavka, deleteCampZayavka, updateCampZayavkaStatus, cleanupE2ECampZayavki } from '../../fixtures/api-client';

const RUN_ID = Date.now().toString().slice(-6);

test.describe('Camp Zayavki workflow', () => {
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

  test('new → sent via Send button', async ({ page }) => {
    const serial = `WF1-${RUN_ID}`;
    const record = await createCampZayavka({ campId: 1, serialBegin: serial, quantity: 2 });
    createdIds.push(record.id);

    await page.goto('/camp-zayavki');
    // Wait for page to fully render before looking for the row
    await expect(page.locator('table')).toBeVisible({ timeout: 10_000 });

    const row = page.locator('tr', { hasText: serial }).first();
    await expect(row).toBeVisible({ timeout: 5_000 });

    await row.getByRole('button', { name: 'Отправить' }).click();

    // After status change the row moves to 'sent' tab — verify by navigating there
    await page.waitForTimeout(500);
    // Click Sent tab — use force to bypass detach issues with Ant Design tabs
    await page.locator('.ant-tabs-tab', { hasText: 'Отправленные' }).click({ force: true });

    await expect(page.locator('tr', { hasText: serial }).first()).toBeVisible({ timeout: 5_000 });
  });

  test('sent → accepted via Accept button', async ({ page }) => {
    const serial = `WF2-${RUN_ID}`;
    const record = await createCampZayavka({ campId: 1, serialBegin: serial, quantity: 2 });
    createdIds.push(record.id);
    await updateCampZayavkaStatus(record.id, '2'); // move to sent via API

    await page.goto('/camp-zayavki');
    await expect(page.locator('table')).toBeVisible({ timeout: 10_000 });

    // Wait for tabs to fully settle before clicking
    await page.waitForTimeout(1000);
    // Navigate to Sent tab — use force to bypass detach issues
    await page.locator('.ant-tabs-tab', { hasText: 'Отправленные' }).click({ force: true });

    const row = page.locator('tr', { hasText: serial }).first();
    await expect(row).toBeVisible({ timeout: 10_000 });

    await row.getByRole('button', { name: 'Принять' }).click();

    await page.waitForTimeout(500);
    await page.locator('.ant-tabs-tab', { hasText: 'Принятые' }).click({ force: true });

    await expect(page.locator('tr', { hasText: serial }).first()).toBeVisible({ timeout: 5_000 });
  });

  test('sent → rejected via Reject button', async ({ page }) => {
    const serial = `WF3-${RUN_ID}`;
    const record = await createCampZayavka({ campId: 1, serialBegin: serial, quantity: 2 });
    createdIds.push(record.id);
    await updateCampZayavkaStatus(record.id, '2'); // move to sent via API

    await page.goto('/camp-zayavki');
    await expect(page.locator('table')).toBeVisible({ timeout: 10_000 });

    // Wait for tabs to fully settle before clicking
    await page.waitForTimeout(1000);
    // Navigate to Sent tab — use force to bypass detach issues
    await page.locator('.ant-tabs-tab', { hasText: 'Отправленные' }).click({ force: true });

    const row = page.locator('tr', { hasText: serial }).first();
    await expect(row).toBeVisible({ timeout: 10_000 });

    await row.getByRole('button', { name: 'Откл.' }).click();

    await page.waitForTimeout(500);
    await page.locator('.ant-tabs-tab', { hasText: 'Отклоненные' }).click({ force: true });

    await expect(page.locator('tr', { hasText: serial }).first()).toBeVisible({ timeout: 5_000 });
  });
});
