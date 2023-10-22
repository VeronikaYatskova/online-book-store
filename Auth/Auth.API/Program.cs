using Auth.API.Middlewares;
using Auth.API.Extensions;
using FluentValidation.AspNetCore;
using Auth.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

builder.Services.AddControllers()
                .AddFluentValidation();

builder.Services.AddLayers(configuration);

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(configuration
    .GetSection("RabbitMqConfig"));

builder.Services.AddMassTransitConfig(configuration);

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
