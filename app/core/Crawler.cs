using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecipeCrawler.Core
{
    public class Crawler
    {
        private const string CrawlerUserAgent = "RecipeScanner/1.0 rumham.azurewebsites.net";

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
            string content = await ReadResponseAsync("robots.txt");
            Dictionary<string, HashSet<string>> values =
                new Dictionary<string, HashSet<string>>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (string line in content.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries))
            {
                string[] pair = line.Split(
                    new[] { ":" },
                    StringSplitOptions.None);
                string key = pair[0];
                string value = pair[1] ?? string.Empty;
                if (!values.ContainsKey(key))
                {
                    values.Add(key, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
                }

                values[key].Add(value);
            }

            foreach(var value in values)
            {
                Console.WriteLine($"{value.Key}: Count:{value.Value.Count}");
            }
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