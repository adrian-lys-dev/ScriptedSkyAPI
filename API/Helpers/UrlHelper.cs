using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public static class UrlHelper
    {
        private static string? _baseUrl;

        public static void Configure(IConfiguration config)
        {
            _baseUrl = config["ApiUrl"] ?? throw new InvalidOperationException("ApiUrl is missing in configuration.");
        }

        public static string BuildImageUrl(string relativePath)
        {
            if (_baseUrl == null)
                throw new InvalidOperationException("UrlHelper is not configured. Call Configure() at startup.");

            return $"{_baseUrl.TrimEnd('/')}{relativePath}";
        }
    }
}
