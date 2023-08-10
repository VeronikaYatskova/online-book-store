using Microsoft.Extensions.DependencyInjection;
using Requests.BLL.Services.Implementations;
using Requests.BLL.Services.Interfaces;

namespace Requests.BLL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddBussinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestsService, RequestsService>();
            services.AddScoped<IUsersService, UsersService>(); 
        }
    }
}
