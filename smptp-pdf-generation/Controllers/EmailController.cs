using Microsoft.AspNetCore.Mvc;
using smptp_pdf_generation.Models;
using smptp_pdf_generation.Services.Contracts;

namespace smptp_pdf_generation.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender emailSender;
        private readonly IPdfGenerator pdfGenerator;

        public EmailController(IEmailSender emailSender, IPdfGenerator pdfGenerator)
        {
            this.emailSender = emailSender;
            this.pdfGenerator = pdfGenerator;
        }

        public Task<IActionResult> SendEmailWithPdf()
        {
            throw new NotImplementedException();
        }

        [HttpGet("pdf")]
        public IActionResult GetPdf([FromBody] Book book)
        {
            var file = pdfGenerator.CreatePDF(book);

            return File(file, "application/pdf");
        }
    }
}
