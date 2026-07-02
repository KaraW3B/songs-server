using System.Text;
using KaraWeb.Shared.Exceptions;

namespace KaraWeb.Core.Helper
{
    public static class EncodingHelper
    {
        private static bool _isCodePageLoaded;
        private const string DefaultEncoding = "UTF8";

        public static Encoding GetDefaultEncoding()
        {
            return Encoding.UTF8;
        }

        public static string SanitizeEncodingName(string encoding)
        {
            return encoding.Trim().Replace("-", string.Empty).ToUpperInvariant();
        }

        public static bool IsDefaultEncoding(string encoding)
        {
            return encoding.Equals(DefaultEncoding);
        }

        public static Encoding GetEncoding(string encodingName)
        {
            return encodingName switch
            {
                DefaultEncoding => GetDefaultEncoding(),
                "CP1250" => GetCodePageEncoding(1250),
                "CP1252" => GetCodePageEncoding(1252),
                _ => throw new KaraWebException($"Encoding {encodingName} is not supported")
            };
        }

        public static Encoding GetCodePageEncoding(int codePage)
        {
            if (!_isCodePageLoaded)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                _isCodePageLoaded = true;
            }

            return Encoding.GetEncoding(codePage);
        }
    }
}