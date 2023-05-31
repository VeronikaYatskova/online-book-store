using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
    
namespace Infrastructure.Persistance.DataSeeding
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasData(
                new CategoryEntity 
                {
                    CategoryGuid = new Guid("c6debc11-73f4-4bed-8c94-9dff15ceee17"),
                    CategoryName = "Computers - Programming",
                }
            );
        }
    }
}