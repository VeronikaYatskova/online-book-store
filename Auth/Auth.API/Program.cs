using Auth.API.Middlewares;
using Auth.API.Extensions;
using MassTransit;
using Auth.Domain.Entities;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

builder.Services.AddControllers();

builder.Services.AddLayers(configuration);

builder.Services.AddOptions<RabbitMqSettings>().Bind(configuration.GetSection("RabbitMqConfig"));

builder.Services.AddMassTransit(x =>
{
   x.UsingRabbitMq((context, config) =>
   {
        var options = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

        config.Host(options.Host, h =>
        {
            h.Username(options.UserName);
            h.Password(options.Password);
        });
   });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();

builder.Services.AddCustomAuthentication(configuration);

builder.Services.AddOptions(configuration);

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
