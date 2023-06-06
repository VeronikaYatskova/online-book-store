using System.Text;
using smptp_pdf_generation.Models;
using smptp_pdf_generation.Services.Contracts;

namespace smptp_pdf_generation.Services
{
    public class TemplateGenerator : ITemplateGenerator
    {
        public string GetHTMLString(Book book)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                <html>
                    <head>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='title'>
                                <p>Your request to publish the book with below characteristics is accepted.</p>
                            </div>
                            <div class='books-chars'>
                                <div class='info'>
                                    <p class='field'>Title:</p>
                                    <p class='value'>{0}</p>
                                </div>
                                <div class='info'>
                                    <p class='field'>Authors: </p>
                            
            ", 
            book.BookName, book.PublisherName, book.PublishDate);

            foreach (var author in book.BookAuthors)
            {
                sb.AppendFormat(@"<p class='value'>{0}</p>", author);
            }

            sb.AppendFormat(@"
                                </div>
                                <div class='info'>
                                    <p class='field'>Publisher:</p>
                                    <p class='value'>{0}</p>
                                </div>
                                <div class='info'>
                                    <p class='field'>Year: </p>
                                    <p class='value'>{1}</p>
                                </div>
                            </div>
                        </div>
                    </body>
                </html>
            ", book.PublisherName, book.PublishDate);

            return sb.ToString();
        }
    }
}
