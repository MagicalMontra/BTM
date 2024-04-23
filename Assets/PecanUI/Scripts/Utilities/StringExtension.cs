#nullable enable
namespace HotPlay.PecanUI
{
    public static class StringExtension
    {
        public static string GetLocalizedString(this string str)
        {
            if (str is null)
            {
                return string.Empty;
            }

            string result = I2.Loc.LocalizationManager.GetTranslation(str);

            if (string.IsNullOrEmpty(result))
            {
                return str;
            }

            return result;
        }
    }
}
