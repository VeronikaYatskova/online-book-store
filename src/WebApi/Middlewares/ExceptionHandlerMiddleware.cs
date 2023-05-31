using System.Net;
using System.Text.Json;
using Application.Exceptions;

namespace WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            if (ex is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (ex is ValidationException)
            {
                code = HttpStatusCode.UnprocessableEntity;
            }
            else if (ex is AlreadyExistsException)
            {
                code = HttpStatusCode.Conflict;
            }

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
