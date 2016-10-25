using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecipeCrawler.Core
{
    public class RobotsTxt
    {
        public RobotsTxt(Uri host, string rawFile)
        {
            var values = ParseRawFile(rawFile);
            Disallowed = new HashSet<Uri>(values["Disallow"].Select(
                v => new Uri(
                    new Uri($"https://{host.Host}/", UriKind.Absolute),
                    new Uri(v, UriKind.Relative))));
        }

        private HashSet<Uri> Disallowed { get; } = new HashSet<Uri>();

        /// <summary>
        /// Verify if a URI can be accessed according to the robots.txt rules.
        /// </summary>
        public bool Allow(Uri uri)
        {
            // TODO: validate

            foreach(Uri disallowed in Disallowed)
            {
                if (uri.AbsolutePath.StartsWith(
                    disallowed.AbsolutePath,
                    StringComparison.OrdinalIgnoreCase)) return false;
            }

            return true;
        }

        /// <summary>
        /// Extracts all values for each key into a dictionary,
        /// filtering by user agent.
        /// </summary>
        private Dictionary<string, HashSet<string>> ParseRawFile(string rawFile)
        {
            Dictionary<string, HashSet<string>> values =
                new Dictionary<string, HashSet<string>>(
                    StringComparer.OrdinalIgnoreCase);

            bool ignoreEntries = false;
            foreach (string line in rawFile.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries))
            {
                // read line as key/value pair
                string[] pair = line.Split(
                    new[] { ":" },
                    StringSplitOptions.None);
                string key = pair[0].Trim();
                string value = pair[1]?.Trim() ?? string.Empty;

                // for user-agent update filtering and skip adding to values
                if (key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                {
                    // update ignoreEntries
                    // TODO: need better user agent filtering
                    if (value == "*" || value.Equals(Crawler.CrawlerUserAgent,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        ignoreEntries = false;
                    }
                    else
                    {
                        ignoreEntries = true;
                    }
                    continue;
                }

                // check if entry should be ignored
                if (ignoreEntries)
                {
                    continue;
                }

                // add key if needed and value
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

            return values;
        }
    }
}