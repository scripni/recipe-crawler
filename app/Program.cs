using System;
using RecipeCrawler.Core;

namespace RecipeCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Crawler crawler = new Crawler(new Uri("http://allrecipes.com/"));
            crawler.LoadRobotsTxt().Wait();
            crawler.Crawl().Wait();
        }
    }
}
