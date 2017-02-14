using System.Text.RegularExpressions;

namespace ProductListing.Utilities
{
    internal class RegexHelper
    {
        public const string REGEX_REPLACE_PATTERN = "[^\\p{L}\\d:\\.]";
        public const string REGEX_TRIMMER_PATTERN = "\\s\\s+";

        public static readonly Regex RegexReplace = new Regex(REGEX_REPLACE_PATTERN, RegexOptions.Compiled);
        public static readonly Regex RegexTrimmer = new Regex(REGEX_TRIMMER_PATTERN, RegexOptions.Compiled);
    }
}
