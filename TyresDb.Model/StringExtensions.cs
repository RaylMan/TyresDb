using System.Text.RegularExpressions;

namespace TyresDb.Model
{
    public static class StringExtensions
    {
        private static readonly Regex _regex = new Regex("[^0-9.,]+");
        public static bool IsDigit(this string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
