using System.Text.Json;
using BookStore.Application.Abstractions.Contracts.Interfaces;

namespace BookStore.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IExceptionsService exceptionService;

        public ExceptionHandlerMiddleware(RequestDelegate next, IExceptionsService exceptionService)
        {
            this.next = next;
            this.exceptionService = exceptionService;
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

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = exceptionService.GetStatusCodeOnException(ex);

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
