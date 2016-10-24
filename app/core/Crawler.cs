using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeCrawler.Core
{
    public class Crawler
    {
        public async Task Crawl()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://allrecipes.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "RecipeScanner/1.0 rumham.azurewebsites.net");

            using (var response = await client.GetAsync("robots.txt"))
            {
                Console.WriteLine("Got response");
            }
        }
    }
}