using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SimpleBookingApp.Persistence.Behaviors
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                message = exception.Message,  // Kullanıcıya gösterilecek mesaj
                details = exception.StackTrace // Debugging için hata detayı (Geliştiriciler için)
            };

            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest, // Eksik parametre
                InvalidOperationException => StatusCodes.Status400BadRequest, // Geçersiz işlem
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // Kimlik doğrulama hatası
                TimeoutException => StatusCodes.Status408RequestTimeout, // Zaman aşımı hatası
                _ => StatusCodes.Status500InternalServerError // Beklenmeyen hatalar için 500
            };

            // Hata mesajını JSON formatında döndür
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
