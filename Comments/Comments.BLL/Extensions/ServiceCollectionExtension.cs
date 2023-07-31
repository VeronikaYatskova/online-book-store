using Comments.BLL.Services.Implementation;
using Comments.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace Comments.BLL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddFluentValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(fv => {
                fv.ImplicitlyValidateChildProperties = true;
                fv.ImplicitlyValidateRootCollectionElements = true;
                
                fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICommentsService, CommentsService>();
        }
    }
}
