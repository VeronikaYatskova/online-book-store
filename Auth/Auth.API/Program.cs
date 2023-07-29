using Auth.API.Middlewares;
using Auth.API.Extensions;
using MassTransit;
using Auth.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

builder.Services.AddOptionsConfiguration(configuration);

builder.Services.AddControllers();

builder.Services.AddLayers(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomAuthentication(configuration);

builder.Services.AddMassTransit(busConfigurator =>
{   
    busConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        var settings = provider.GetRequiredService<RabbitMqSettings>();
        config.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });
    }));
    
    // busConfigurator.UsingRabbitMq((context, configurator) =>
    // {
    //     RabbitMqSettings settings = context.GetRequiredService<RabbitMqSettings>();

    //     configurator.Host(new Uri(settings.Host), h =>
    //     {
    //         h.Username(settings.UserName);
    //         h.Password(settings.Password);
    //     });
    // });
});

var app = builder.Build();

app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
