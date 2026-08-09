using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public static class ItemSearch
    {
        public static List<string> Match(string query, IEnumerable<string> names)
        {
            var matches = new List<string>();

            foreach (var name in names)
            {
                foreach (var word in name.Split(' '))
                {
                    if (word.StartsWith(query, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(name);
                        break;
                    }
                }
            }

            return matches;
        }
    }
}