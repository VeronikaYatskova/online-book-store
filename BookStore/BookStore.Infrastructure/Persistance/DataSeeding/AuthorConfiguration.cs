using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistance.DataSeeding
{
    public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.HasData(
                new AuthorEntity
                {
                    AuthorGuid = new Guid("d791c427-0df6-410f-bf4a-bf623fe73888"),
                    AuthorName = "Daniel",
                    AuthorLastName = "Kanneman",
                }
            );
        }
    }
}