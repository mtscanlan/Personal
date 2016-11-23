using System.Text.RegularExpressions;

namespace Utilities
{
    public class RegexHelper
    {
        public const string REGEX_REPLACE_PATTERN = "[^a-zA-Z\\d:]";
        public const string REGEX_TRIMMER_PATTERN = "\\s\\s+";

        public static readonly Regex RegexReplace = new Regex(REGEX_REPLACE_PATTERN);
        public static readonly Regex RegexTrimmer = new Regex(REGEX_TRIMMER_PATTERN);
    }
}
