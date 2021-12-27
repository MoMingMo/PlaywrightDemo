using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Playwright_5
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchPersistentContextAsync(@"C:\Program Files\Google\Chrome\Application\");
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.baidu.com/");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });

            Console.ReadKey();
        }
    }
}
