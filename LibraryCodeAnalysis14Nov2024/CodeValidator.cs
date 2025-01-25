using System.Text.RegularExpressions;

namespace LibraryCodeAnalysis14Nov2024
{
    public class CodeValidator
    {
        public static bool IsCsharpBlockProperlyClosed(string input)
        {
            string[] startTags = { "```csharp", "```C#" };
            const string endTag = "```";

            int currentIndex = 0;

            while (true)
            {
                int startIndex = startTags
                    .Select(tag => input.IndexOf(tag, currentIndex, StringComparison.OrdinalIgnoreCase))
                    .Where(index => index != -1)
                    .DefaultIfEmpty(-1)
                    .Min();

                if (startIndex == -1) break;

                int endIndex = input.IndexOf(endTag, startIndex + 1);
                if (endIndex == -1) return false;

                currentIndex = endIndex + endTag.Length;
            }

            return true;
        }

        public static List<string> ExtractCSharpCode(string input)
        {
            string pattern = @"```(?:csharp|C#)(.*?)```";
            var matches = Regex.Matches(input, pattern, RegexOptions.Singleline);

            return matches.Cast<Match>().Select(m => m.Groups[1].Value.Trim()).ToList();
        }
    }

}
