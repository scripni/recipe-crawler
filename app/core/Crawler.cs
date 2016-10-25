using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeCrawler.Core
{
    public class Crawler
    {
        public const string CrawlerUserAgent = "RecipeScanner/1.0 rumham.azurewebsites.net";

        public Crawler(Uri uri)
        {
            Client = new HttpClient();
            Client.BaseAddress = uri;
            Client.DefaultRequestHeaders.Add("User-Agent", CrawlerUserAgent);
        }

        HttpClient Client { get; }

        public async Task Crawl()
        {
        }

        public async Task LoadRobotsTxt()
        {
            RobotsTxt rt = new RobotsTxt(
                Client.BaseAddress,
                await ReadResponseAsync("robots.txt"));
            Console.WriteLine(rt.Allow(new Uri("http://allrecipes.com/test/", UriKind.Absolute)));
            Console.WriteLine(rt.Allow(new Uri("http://allrecipes.com/My/", UriKind.Absolute)));
        }

        private async Task<string> ReadResponseAsync(string path)
        {
            using (var response = await Client.GetAsync(path))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}