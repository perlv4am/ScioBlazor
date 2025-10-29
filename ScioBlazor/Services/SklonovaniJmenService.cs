using System.Net.Http;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScioBlazor.Services
{
    public sealed class SklonovaniJmenService : INameDeclensionService
    {
        private readonly HttpClient _http;
        private readonly NameDeclensionOptions _options;
        private readonly ILogger<SklonovaniJmenService> _logger;

        public SklonovaniJmenService(HttpClient http, IOptions<NameDeclensionOptions> options, ILogger<SklonovaniJmenService> logger)
        {
            _http = http;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string?> GetInstrumentalFirstNameAsync(string firstName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(firstName)) return null;

            try
            {
                var apiKey = _options.ApiKey;
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    // No key configured – skip calling external API
                    return null;
                }

                var builder = new UriBuilder(_options.BaseUrl);
                var query = HttpUtility.ParseQueryString(string.Empty);
                query["klic"] = apiKey!;
                query["pad"] = "7"; // instrumental
                query["jmeno"] = firstName;
                builder.Query = query.ToString()!;

                using var req = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
                using var resp = await _http.SendAsync(req, cancellationToken);
                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Name declension API failed with status {Status}", (int)resp.StatusCode);
                    return null;
                }

                // API returns a plain text like "panem Václavem" or "paní Klárou"
                var body = await resp.Content.ReadAsStringAsync(cancellationToken);
                var extracted = ExtractInstrumentalFromPlainText(body);
                return string.IsNullOrWhiteSpace(extracted) ? null : extracted;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get instrumental form for '{FirstName}'", firstName);
                return null;
            }
        }

        private static string? ExtractInstrumentalFromPlainText(string input)
        {
            // Examples: "panem Václavem", "paní Klárou"
            // Heuristic: take the last whitespace-separated token as the instrumental first name.
            var s = (input ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(s)) return null;

            // Normalize whitespace
            var parts = s.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;
            var last = parts[^1];

            // Trim possible quotes or punctuation
            last = last.Trim('"', '\'', '.', ',', ';', ':', ')', '(', '[', ']', '{', '}', '«', '»');

            return string.IsNullOrWhiteSpace(last) ? null : last;
        }
    }
}

