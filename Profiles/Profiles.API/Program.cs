using MassTransit;
using Microsoft.Extensions.Options;
using FluentValidation.AspNetCore;
using Profiles.API.Extensions;
using Profiles.API.Middlewares.ExceptionMiddleware;
using Profiles.Application.Consumers;
using Profiles.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

// Add services to the container.

builder.Services.AddOptionsConfiguration(configuration);

builder.Services.AddControllers()
                .AddFluentValidation();
         
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLayers();

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

// builder.Services.AddMassTransit(x => 
// {
//     x.AddConsumer<UserRegisteredConsumer>();
//     x.UsingRabbitMq((context, configuration) => 
//     {
//         RabbitMqSettings rabbitMqSettings = context.GetRequiredService<RabbitMqSettings>();

//         configuration.Host(new Uri(rabbitMqSettings.Host!), h =>
//         {
//            h.Username(rabbitMqSettings.UserName);
//            h.Password(rabbitMqSettings.Password);
//         });

//         configuration.ReceiveEndpoint("user-registered-event", e => 
//         {
//             e.ConfigureConsumer<UserRegisteredConsumer>(context);   
//         });
//     });
// });

var app = builder.Build();

app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
