using Infrastructure;
using Application;
using WebApi.Middlewares;
using Serilog;
using Serilog.Events;
using WebApi;
using Infrastructure.Persistance.DataContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(configuration);

var app = builder.Build();

// app.MigrateDatabase<AppDbContext>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
