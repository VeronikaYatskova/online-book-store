using FluentValidation.AspNetCore;
using Profiles.API.Extensions;
using Profiles.API.Middlewares.ExceptionMiddleware;

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
