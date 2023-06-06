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

        [HttpPost]
        public IActionResult SendEmailWithPdf([FromBody] EmailInfo emailInfo)
        {
            var file = pdfGenerator.CreatePDF(emailInfo.Book);
            emailSender.SendEmail(emailInfo.EmailTo, file);

            return Ok("Your email is sent.");
        }
    }
}
