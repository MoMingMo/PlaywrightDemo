using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightDemo
{
    public class ImageHandler
    {
        public readonly IPage _page;
        //上次执行的页码 实际页码-1
        private int pageIndex =int.Parse(Configuration.GetConfiguration("LastPageIndex"));
        private int totalPage = 1;
        
        public ImageHandler(IPage page)
        {
            _page = page;
        }
        public async Task GotoPage(int pageIndex)
        {
            await _page.GotoAsync($"https://wallhaven.cc/toplist?page={pageIndex}");
            Console.WriteLine($"当前页为{pageIndex},共{totalPage}页");
        }
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <returns></returns>
        public async Task GetTotalPage()
        {
            await _page.GotoAsync($"https://wallhaven.cc/toplist?page=2");
            var pageElement = await _page.QuerySelectorAsync(".thumb-listing-page-header>h2");
            var pageStr = await pageElement.TextContentAsync();
            totalPage = int.Parse(pageStr.Split("/")[1]);
            Console.WriteLine($"一共{totalPage}页");
        }
        public async Task HandlerImage(IPage page, string selector)
        {
            pageIndex++;
            await GotoPage(pageIndex);
            var elements = await _page.QuerySelectorAllAsync(selector);
            Console.WriteLine($"本页共{elements.Count}张图片");
            //上次下载图片序号
            var picIndex = int.Parse(Configuration.GetConfiguration("LastPicIndex"));
            for (int i = 0; i < elements.Count; i++)
            {
                //从上次下载的图片继续下载
                if (picIndex != 0&&i==0) 
                {
                    i = picIndex;
                    picIndex = 0;
                }
                Console.WriteLine($"当前第{i+1}/{elements.Count}张图片");
                var a = await elements[i].QuerySelectorAsync("a");
                var url = await a.GetAttributeAsync("href");
                Console.WriteLine(url);
                await page.GotoAsync(url);

                var img = await page.QuerySelectorAsync("img#wallpaper");

                var name = await img.GetAttributeAsync("data-wallpaper-id");
               
                var infoElement = await page.QuerySelectorAsync("h3.showcase-resolution");
                var info = await infoElement.TextContentAsync();
                Console.WriteLine("图像尺寸是：" + info);

                var width = info.Split('x')[0];
                var height = info.Split('x')[1];

                // Click text=Crop & Scale Download
                await page.ClickAsync("text=Crop & Scale Download");

                //await page1.ClickAsync("#form-respicker-custom-width");
                await page.FillAsync("#form-respicker-custom-width", width.Trim());

                //await page1.ClickAsync("#form-respicker-custom-height");
                await page.FillAsync("#form-respicker-custom-height", height.Trim());

                // Click text=Done
                await page.ClickAsync("button.green");
                // Click text=Continue
                var download2 = await page.RunAndWaitForDownloadAsync(async () =>
                {
                    Thread.Sleep(200);
                    await page.ClickAsync("a.green");
                });
                await download2.SaveAsAsync($"Pic\\{pageIndex}-{i}-{name}.jpg");
                Console.WriteLine($"name is {pageIndex}-{i}-{name}.jpg");
            }
            if (pageIndex <= totalPage)
                await HandlerImage(page, selector);
        }
    }
}
