using System.Net;
using System.Text.Json;
using Azure;
using Azure.Storage.Blobs.Models;

namespace Requests.API.Middlewares.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
