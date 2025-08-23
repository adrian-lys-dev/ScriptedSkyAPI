namespace Application.Helpers
{
    public static class UrlHelper
    {
        private static string? _baseUrl;

        public static void Configure(string baseUrl)
        {
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        public static string BuildImageUrl(string relativePath)
        {
            if (_baseUrl == null)
                throw new InvalidOperationException("UrlHelper is not configured. Call Configure() at startup.");

            return $"{_baseUrl.TrimEnd('/')}{relativePath}";
        }
    }
}
