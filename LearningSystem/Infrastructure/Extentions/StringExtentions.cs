using System.Text.RegularExpressions;

namespace LearningSystem.Infrastructure.Extentions
{
    public static class StringExtentions
    {
        public static string ToFriendlyUrl(this string text)
            => Regex.Replace(text, @"[^A-Za-z0-9_\.~]", "-").ToLower();
    }
}
