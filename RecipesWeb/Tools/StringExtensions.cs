namespace RecipesWeb.Tools
{
    public static class StringExtensions
    {
        public static string CapitalizeFirst(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (text.Length == 1)
            {
                return text.ToUpper();
            }

            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}