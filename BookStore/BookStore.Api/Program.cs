using BookStore.WebApi.Middlewares;
using BookStore.WebApi.Extensions;
using BookStore.Api.Extensions;
using MassTransit;
using BookStore.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLayers(configuration);

builder.Services.AddOptions(configuration);

builder.Services.AddMassTransit(busConfigurator =>
{   
    busConfigurator.AddConsumer<UserRegisteredConsumer>();
    
    busConfigurator.UsingRabbitMq((context, configuration) => 
    {
        var rabbitMqSettings = context.GetRequiredService<RabbitMqSettings>();

        configuration.Host(new Uri(rabbitMqSettings.Host!), h =>
        {
           h.Username(rabbitMqSettings.UserName);
           h.Password(rabbitMqSettings.Password);
        });

        configuration.ReceiveEndpoint("user-registered-event", c => 
        {
            c.ConfigureConsumer<UserRegisteredConsumer>(context);   
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDataToDbAsync();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
