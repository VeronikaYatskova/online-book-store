using Requests.API.Extension;
using Requests.DAL.Extensions;
using Requests.BLL.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddCustomLogger();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApiLayer(configuration);
builder.Services.AddDataAccessLayer();
builder.Services.AddBussinessLogicLayer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
