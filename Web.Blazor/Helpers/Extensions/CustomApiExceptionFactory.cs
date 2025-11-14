using System.Net;

namespace Web.Blazor.Helpers.Extensions
{
    /// <summary>
    /// This is a custom exception handler for Refit ExceptionFactory. Still in development but can be used.
    /// </summary>
    public static class CustomApiExceptionFactory
    {
        public static async Task<Exception?> Create(HttpResponseMessage response)
        {
            var statusCode = response.StatusCode;
            if (response.IsSuccessStatusCode)
                return null;
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return new Exception("Oturumunuzun süresi dolmuş olabilir. Lütfen tekrar giriş yapınız.")
                    {
                        Source = "Unauthorized"
                    };

                case HttpStatusCode.Forbidden:
                    return new Exception("Yetkiniz yok. Bu işlemi gerçekleştiremezsiniz.")
                    {
                        Source = "Forbidden"
                    };

                case HttpStatusCode.BadRequest:
                    return new Exception("Geçersiz istek gönderildi.")
                    {
                        Source = "BadRequest"
                    };

                case HttpStatusCode.InternalServerError:
                    return new Exception("Sunucuda beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.")
                    {
                        Source = "InternalServerError"
                    };

                default:
                    return new Exception($"Beklenmeyen bir hata oluştu. Durum Kodu: {statusCode}")
                    {
                        Source = "Unknown"
                    };
            }

            // Eğer exception fırlatmak istiyorsan:
            //return await ApiException.Create(response.RequestMessage!, response.RequestMessage!.Method, response, new RefitSettings());

            // Eğer exception fırlatmak istemiyorsan:
            //return null;
        }
    }
}
