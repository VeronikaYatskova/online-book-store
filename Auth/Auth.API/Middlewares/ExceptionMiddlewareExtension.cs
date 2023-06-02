namespace Auth.API.Middlewares
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
