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
                                <p>Your request to publish the {0} with below characteristics is accepted.</p>
                            </div>
                            <div class='books-chars'>
                                <div class='info'>
                                    <p class='field'>Title:</p>
                                    <p class='value'>book_title</p>
                                </div>
                                <div class='info'>
                                    <p class='field'>Authors: </p>
                            
            ", 
            book.BookName, book.PublisherName, book.PublishDate);

            foreach (var author in book.BookAuthors)
            {
                sb.AppendFormat(@"<p class='value'>{0}</p>", author);
            }

            sb.Append(@"
                                </div>
                                <div class='info'>
                                    <p class='field'>Publisher:</p>
                                    <p class='value'>publisher</p>
                                </div>
                                <div class='info'>
                                    <p class='field'>Year: </p>
                                    <p class='value'>year</p>
                                </div>
                            </div>
                        </div>
                    </body>
                </html>
            ");

            return sb.ToString();
        }
    }
}
