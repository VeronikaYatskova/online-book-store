using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.DataSeeding
{
    public class PublisherConfiguration : IEntityTypeConfiguration<PublisherEntity>
    {
        public void Configure(EntityTypeBuilder<PublisherEntity> builder)
        {
            builder.HasData(
                new PublisherEntity 
                {
                    PublisherGuid = new Guid("8441c32a-8f7c-4c72-8b38-ff2a35a43284"),
                    PublisherName = "Manning Publications",
                }
            );
        }
    }
}