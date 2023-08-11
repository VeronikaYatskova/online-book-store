using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using BookStore.Application.PipelineBehaviors;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Services;
using BookStore.Application.Services.CloudServices.Amazon;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using BookStore.Application.Services.CloudServices.Azurite;

namespace BookStore.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IExceptionsService, ExceptionsService>();
            services.AddScoped<IAwsS3Service, AwsS3Service>();
            services.AddScoped<IAzureService, AzureService>();
        }
    }
}
