using BookStore.Application.Abstractions.Contracts.Interfaces;
using Newtonsoft.Json;

namespace BookStore.WebApi.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionsService _exceptionService;

        public ExceptionHandlerMiddleware(RequestDelegate next, IExceptionsService exceptionService)
        {
            _next = next;
            _exceptionService = exceptionService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = _exceptionService.GetStatusCodeOnException(ex);

            var result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
