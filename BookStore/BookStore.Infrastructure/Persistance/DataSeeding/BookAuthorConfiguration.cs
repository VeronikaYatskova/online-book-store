using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistance.DataSeeding
{
    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthorEntity>
    {
        public void Configure(EntityTypeBuilder<BookAuthorEntity> builder)
        {
            builder.HasData(
                new BookAuthorEntity 
                {
                    Guid = new Guid("8af1bf87-a423-4795-af25-afbb6662b35d"),
                    BookGuid = Guid.Parse("cb961ea8-3605-45dd-b590-7ce3a255ac6c"),
                    AuthorGuid = Guid.Parse("d791c427-0df6-410f-bf4a-bf623fe73888"),
                }
            );
        }
    }
}