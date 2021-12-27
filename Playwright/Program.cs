// See https://aka.ms/new-console-template for more information
using Microsoft.Playwright;
using PlaywrightDemo;

Console.WriteLine("Hello, World!");

using var playwright = await Playwright.CreateAsync();

await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Devtools=false, Headless=false});
//toplist page
var page = await browser.NewPageAsync();
//image page
var page1 = await browser.NewPageAsync(new BrowserNewPageOptions { AcceptDownloads = true });
ImageHandler imageHandler = new ImageHandler(page);
await imageHandler.GetTotalPage();
await imageHandler.HandlerImage(page1, ".thumb-listing-page>ul>li");

Console.WriteLine("执行完成");


