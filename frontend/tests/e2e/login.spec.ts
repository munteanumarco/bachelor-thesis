import { test, expect } from '@playwright/test';

test.describe('Login Functionality', () => {
  test('should log in a user via username and password', async ({ page }) => {
    await page.goto('http://localhost:4200/login');

    // Assume inputs are identifiable by unique placeholders or test-ids
    await page.fill('input[placeholder="Username"]', 'testuser');
    await page.fill('input[placeholder="Password"]', 'password123');

    await page.click('button#login-button'); // Assuming the login button has an ID

    // Expect to navigate to the home page after successful login
    await expect(page).toHaveURL('http://localhost:4200/');

    // Optionally check for local storage or cookie changes
    const token = await page.evaluate(() => localStorage.getItem('token'));
    expect(token).not.toBeNull();
  });

  test('should handle failed login attempts', async ({ page }) => {
    await page.goto('http://localhost:4200/login');

    await page.fill('input[placeholder="Username"]', 'wronguser');
    await page.fill('input[placeholder="Password"]', 'wrongpassword');

    await page.click('button#login-button'); // Assuming the login button has an ID

    // Check for an error message
    const errorMessage = await page.locator('text=Login Failed').textContent();
    expect(errorMessage).toContain('Login Failed');
  });

  test('google login simulation', async ({ page }) => {
    await page.goto('http://localhost:4200/login');

    // Simulate Google API script load and button render
    await page.addScriptTag({ url: 'https://accounts.google.com/gsi/client' });
    await page.waitForSelector('#google-button'); // Make sure the Google button is rendered

    // Simulate Google login button click
    await page.click('#google-button');

    // Depending on how you handle the callback and navigation post-Google login, add those checks here
    await expect(page).toHaveURL('http://localhost:4200/');
  });
});
