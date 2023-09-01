using Comments.API.Extensions;
using Comments.API.Middlewares.ExceptionHandlerMiddleware;
using Comments.BLL.Consumers;
using Comments.BLL.DTOs.General;
using Comments.BLL.Extensions;
using Comments.DAL.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.AddCustomLogger();

// Add services to the container.

builder.Services.AddControllers()
                .AddFluentValidation();

builder.Services.AddDataAccessLayer(configuration);
builder.Services.AddBusinessLogicLayer();
builder.Services.AddApiLayer(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransitConfig(configuration);

var app = builder.Build();

app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
