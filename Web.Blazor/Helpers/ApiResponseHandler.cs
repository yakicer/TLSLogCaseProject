using Refit;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Web.Blazor.Helpers
{
    public class ApiResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return response;
            }

            try
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                using var document = await JsonDocument.ParseAsync(contentStream, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                var root = document.RootElement;

                var success = root.GetProperty("success").GetBoolean();

                if (!success)
                {
                    throw await ApiException.Create(request, request.Method, response,
                        new RefitSettings(new SystemTextJsonContentSerializer())).ConfigureAwait(false);
                }

                if (!root.TryGetProperty("data", out var dataElement))
                {
                    return response;
                }

                switch (dataElement.ValueKind)
                {
                    case JsonValueKind.Object or JsonValueKind.Array:
                        {
                            var dataJson = dataElement.GetRawText();
                            response.Content = new StringContent(dataJson, Encoding.UTF8, "application/json");
                            break;
                        }
                    case JsonValueKind.Number:
                        var dataLong = dataElement.GetInt64();
                        response.Content = new StringContent(dataLong.ToString(CultureInfo.InvariantCulture), Encoding.UTF8,
                            "text/plain");
                        break;
                    case JsonValueKind.String:
                        {
                            var dataString = dataElement.GetString() ?? string.Empty;
                            response.Content = new StringContent(dataString, Encoding.UTF8, "text/plain");
                            break;
                        }
                }

                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }
    }
}
