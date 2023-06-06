using DinkToPdf;
using DinkToPdf.Contracts;
using Smtp.WebAPI.Services;
using Smtp.WebAPI.Services.Contracts;

namespace Smtp.WebAPI
{
    public static class IServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<ITemplateGenerator, TemplateGenerator>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
        }
    }
}
