using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.DataSeeding
{
    public class BooksConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasData(
                new BookEntity 
                {
                    BookGuid = new Guid("cb961ea8-3605-45dd-b590-7ce3a255ac6c"),
                    BookName = "ASP.NET Core in Action",
                    ISBN10 = "1617298301",
                    ISBN13 = "9781617298301",
                    PagesCount = 834,
                    PublishYear = "2021",
                    Language = "English",
                    PublisherGuid = Guid.Parse("8441c32a-8f7c-4c72-8b38-ff2a35a43284"),
                    CategoryGuid = Guid.Parse("c6debc11-73f4-4bed-8c94-9dff15ceee17"),
                }
            );
        }
    }
}
