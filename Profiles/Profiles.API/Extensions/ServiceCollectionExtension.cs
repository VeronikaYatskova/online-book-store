using Profiles.Infrastructure.Extensions;
using Profiles.Application.Extensions;
using Serilog;
using Serilog.Events;

namespace Profiles.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddLayers(this IServiceCollection services)
        {
            services.AddInfrastrucureLayer();
            services.AddApplicationLayer();
        }

        public static void AddCustomLogger(this ILoggingBuilder loggingBuilder)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);
        }
    }
}
