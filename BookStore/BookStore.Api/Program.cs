using BookStore.WebApi.Middlewares;
using BookStore.WebApi.Extensions;
using BookStore.Api.Extensions;
using Hangfire;
using BookStore.Application.Abstractions.Contracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// builder.Logging.AddCustomLogger(configuration);
// builder.Host.UseSerilog();

builder.Services.AddControllers();

// builder.Services.AddCustomAuthentication(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLayers(configuration);

builder.Services.AddOptions(configuration);

builder.Services.AddMassTransitConfig();

builder.Services.AddHangfireConfig(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDataToDbAsync();
}

app.ConfigureCustomExceptionMiddleware();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IBackgroudService>(s => s.SendDailyEmail(), Cron.MinuteInterval(1));

app.Run();
