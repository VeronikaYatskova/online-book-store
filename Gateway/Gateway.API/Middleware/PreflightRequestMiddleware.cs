using System.Net;

namespace Gateway.API.Middleware
{
    public class PreflightRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PreflightRequestMiddleware> _logger;
        
        public PreflightRequestMiddleware(RequestDelegate next, ILogger<PreflightRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }
        
        private Task BeginInvoke(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "*");
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            _logger.LogError("Set Headers");
            _logger.LogError("Request method: " + context.Request.Method);

            if (context.Request.Method == HttpMethod.Options.Method)
            {
                _logger.LogError("Inside if");

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                return context.Response.WriteAsync("OK");
            }

            return _next(context);
        }
    }
}
