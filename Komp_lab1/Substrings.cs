using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Komp_lab1
{
    public class SubstringResult
    {
        public string Value { get; set; }
        public int Position { get; set; }
        public int Line { get; set; }
        public int Length => Value.Length;
    }

    internal class Substrings
    {
        public List<SubstringResult> Find(string text, int mode)
        {
            string pattern = "";

            switch (mode)
            {
                case 0:
                    pattern = "[еёиоуюыэяЕЁИОУЮЫЭЯ]";
                    break;

                case 1:
                    pattern = @"\b(?:[A-ZА-ЯЁ]\.){2,}|[A-ZА-ЯЁ]{2,}\b";
                    break;

                case 2: 
                    //pattern = @"\b(http|https|ftp):\/\/([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:[0-9]+)?";
                    //pattern = @"\b(?<protocol>http|https|ftp):\/\/(?<domain>([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,})(?<port>:\d+)?(?<path>\/[^\s]*)?";
                    pattern = @"\b(?<protocol>http|https|ftp)(?<sep>:\/\/)(?<domain>([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,})(?<port>:\d+)?(?<path>\/[^\s]*)?";
                    break;
            }

            var results = new List<SubstringResult>();
            var matches = Regex.Matches(text, pattern);

            foreach (Match match in matches)
            {
                int line = text.Substring(0, match.Index).Split('\n').Length;

                // ВЕСЬ URL
                results.Add(new SubstringResult
                {
                    Value = match.Value,
                    Position = match.Index,
                    Line = line
                });

                if (match.Groups["protocol"].Success)
                {
                    results.Add(new SubstringResult
                    {
                        Value = match.Groups["protocol"].Value,
                        Position = match.Groups["protocol"].Index,
                        Line = line
                    });
                }
                if (match.Groups["domain"].Success)
                {
                    results.Add(new SubstringResult
                    {
                        Value = match.Groups["domain"].Value,
                        Position = match.Groups["domain"].Index,
                        Line = line
                    });
                }
                if (match.Groups["port"].Success)
                {
                    results.Add(new SubstringResult
                    {
                        Value = match.Groups["port"].Value,
                        Position = match.Groups["port"].Index,
                        Line = line
                    });
                }
                if (match.Groups["path"].Success)
                {
                    results.Add(new SubstringResult
                    {
                        Value = match.Groups["path"].Value,
                        Position = match.Groups["path"].Index,
                        Line = line
                    });
                }
            }

            return results;
        }
    }
}