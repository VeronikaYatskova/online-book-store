using System.Text;
using HandlebarsDotNet;
using PdfGenerator.Interfaces;
using PdfGenerator.Models;

namespace PdfGenerator.Templates
{
    public class BookPublishedMessageTemplateGenerator : IBookPublishedMessageTemplateGenerator
    {
        public string GenerateHtmlTemplate(BookInPdf message)
        {
            var htmlString = new StringBuilder();
            htmlString.Append(@"
            <html>
            <head>
                <style>
                    .container {
                        border-bottom: 0.5ch solid red;
                        width: 618px;
                        margin-top: 30px;
                        margin-left: 64px;
                    }
                    .title {
                        font-size: 24px;
                        font-weight: 600;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        margin-bottom: 50px;
                    }
                    .book-info {
                        display: flex;
                        flex-direction: column;
                        margin-bottom: 20px;
                    }
                    .property {
                        display: flex;
                        justify-content: space-between;
                        flex-direction: row;
                        margin-bottom: 10px;
                    }
                    .property-name {
                        font-size: medium;
                    }
                </style>
            </head>
                <body>
                    <div class='container'>
                        <div class='title'>{{bookName}} was published on {{publishedAt}}</div>
                        <div class='book-info'>
                            <div class='property'>
                                <div class='property-name'>Name:</div>
                                <div class='book-property'>{{bookName}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>ISBN 10:</div>
                                <div class='book-property'>{{isbn10}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>ISBN 13:</div>
                                <div class='book-property'>{{isbn13}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Count of pages:</div>
                                <div class='book-property'>{{pagesCount}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Year:</div>
                                <div class='book-property'>{{publishedIn}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Language:</div>
                                <div class='book-property'>{{language}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Category:</div>
                                <div class='book-property'>{{category}}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Publisher:</div>
                                <div class='book-property'>{{publisher}}</div>
                            </div>
                        </div>
                    </div>
                </body>
            <html>");

            var context = new 
            { 
                bookName = message.BookName,
                publishedAt = DateTime.UtcNow,
                isbn10 = message.ISBN10,
                isbn13 = message.ISBN13,
                pagesCount = message.PagesCount,
                publishedIn = message.PublishYear,
                language = message.Language,
                category = message.Category,
                publisher = message.Publisher,
            };

            var template = Handlebars.Compile(htmlString.ToString());
            var htmlTemplate = template(context);
            
            return htmlTemplate;
        }
    }
}
