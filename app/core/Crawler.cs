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

        private async Task<byte[]> ReadAsync(Stream responseStream)
        {
            byte[] buffer = new byte[32 * 1024];
            using (MemoryStream memStream = new MemoryStream())
            {
                int bytesRead;
                while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }

                return memStream.ToArray();
            }
        }
    }
}