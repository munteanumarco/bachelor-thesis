import { test, expect } from '@playwright/test';

test.describe('Report Emergency Event Functionality', () => {
  test('should report an emergency successfully', async ({ page }) => {
    await page.goto('http://localhost:4200/report-emergency');

    await page.selectOption('select[name="type"]', { value: '1' }); // Assuming type values are integers
    await page.fill('textarea[name="description"]', 'Test emergency description');
    await page.selectOption('select[name="severity"]', { value: '2' }); // Assuming severity values are integers

    const context = page.context();
    await context.grantPermissions(['geolocation']);
    await context.setGeolocation({ latitude: 37.7749, longitude: -122.4194 }); // San Francisco

    await page.click('#location-fetch-button'); // Button to trigger location fetch

    await page.waitForFunction(() => document.querySelector('input[name="location"]'));

    await page.click('button#submit-report'); // Submit the form

    await expect(page.locator('text=Emergency event reported successfully')).toBeVisible();

    await expect(page).toHaveURL('http://localhost:4200/');
  });

});
