using System.Text;
using PdfGenerator.Interfaces;
using OnlineBookStore.Messages.Models.Messages;

namespace PdfGenerator.Templates
{
    public class RequestUpdatedMessageHtmlTemplateGenerator : IRequestUpdatedMessageHtmlTemplateGenerator
    {
        public string GenerateHtmlTemplate(RequestUpdatedMessage message)
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
                    <p>Your book publishing request was approved. The book will be published soon.</p>
                </div>
            </body>
            ");

            return stringBuilder.ToString();
        }
    }
}
