using DinkToPdf;
using DinkToPdf.Contracts;
using smptp_pdf_generation.Services;
using smptp_pdf_generation.Services.Contracts;

namespace smptp_pdf_generation
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
