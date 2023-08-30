using EmailService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices((context, services) =>{
        services.AddScoped<IEmailService, EmailService.Services.EmailService>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
