using Gateway.API.Extensions;
using Gateway.API.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "CorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.ConfigureAuthentication(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot(configuration).AddPolly();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var config = new OcelotPipelineConfiguration
{
    AuthorizationMiddleware = async (context, next) =>
    {
        await OcelotJwtMiddleware.Authorize(context, next);
    }
};

app.UsePreflightRequestHandler();

await app.UseOcelot(config);

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
