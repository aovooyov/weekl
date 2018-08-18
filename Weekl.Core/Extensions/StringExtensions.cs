using System.Text.RegularExpressions;

namespace Weekl.Core.Extensions
{
    public static class StringExtensions
    {
        public static string StripHtml(this string source)
        {
            return string.IsNullOrEmpty(source)
                ? string.Empty
                : Regex.Replace(source, "<.*?>", string.Empty).Trim();
        }

        public static string StripImgTag(this string source)
        {
            return string.IsNullOrEmpty(source)
                ? string.Empty
                : Regex.Replace(source, "<img[^>]*?>", string.Empty).Trim();
        }

        public static string StripScriptTag(this string source)
        {
            return string.IsNullOrEmpty(source)
                ? string.Empty
                : Regex.Replace(source, @"<script(.+?)*</script>", string.Empty).Trim();
        }

        public static int ContainsCount(this string source, string text)
        {
            return Regex.Matches(source, text).Count;
        }
    }
}