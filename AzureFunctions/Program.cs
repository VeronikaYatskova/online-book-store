using EmailService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices((context, services) =>{
        context.Configuration = new ConfigurationBuilder()
                .SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
                
        services.Configure<EmailService.Models.EmailConfiguration>(context.Configuration.GetSection("EmailConfig"));
        services.AddScoped<IEmailService, EmailService.Services.EmailService>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
