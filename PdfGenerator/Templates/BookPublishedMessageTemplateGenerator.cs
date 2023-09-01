using System.Text;
using PdfGenerator.Interfaces;
using PdfGenerator.Models;

namespace PdfGenerator.Templates
{
    public class BookPublishedMessageTemplateGenerator : IBookPublishedMessageTemplateGenerator
    {
        public string GenerateHtmlTemplate(BookInPdf message)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(@"
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
                        <div class='title'>Book was {0} published on {1}</div>
                        <div class='book-info'>
                            <div class='property'>
                                <div class='property-name'>Name:</div>
                                <div class='book-property'>{2}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>ISBN 10:</div>
                                <div class='book-property'>{3}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>ISBN 13:</div>
                                <div class='book-property'>{4}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Count of pages:</div>
                                <div class='book-property'>{5}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Year:</div>
                                <div class='book-property'>{6}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Language:</div>
                                <div class='book-property'>{7}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Category:</div>
                                <div class='book-property'>{8}</div>
                            </div>
                            <div class='property'>
                                <div class='property-name'>Publisher:</div>
                                <div class='book-property'>{9}</div>
                            </div>
                        </div>
                    </div>
                </body>
            <html>",            
            message.BookGuid, 
            message.BookName,
            message.ISBN10,
            message.ISBN13,
            message.PagesCount.ToString(),
            message.PublishYear,
            message.Language,
            message.Category,
            message.Publisher,
            DateTime.UtcNow.ToShortDateString());

            return stringBuilder.ToString();
        }
    }
}
