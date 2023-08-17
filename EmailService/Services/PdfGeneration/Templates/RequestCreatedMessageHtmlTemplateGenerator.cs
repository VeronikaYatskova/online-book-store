using System.Text;
using EmailService.Services.PdfGeneration.Interfaces;
using OnlineBookStore.Messages.Models.Messages;

namespace EmailService.Services.PdfGeneration.Templates
{
    public class RequestCreatedMessageHtmlTemplateGenerator : IRequestCreatedMessageHtmlTemplateGenerator
    {
        public string GenerateHtmlTemplate(RequestCreatedMessage message)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"
            <head>
                <style>
                    .container {
                        border-bottom: 0.5ch solid red;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <p>Your book publishing request was received. You will get a letter with the result soon.</p>
                </div>
            </body>");

            return stringBuilder.ToString();
        }
    }
}
