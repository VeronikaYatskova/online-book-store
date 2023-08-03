using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistance.Configurations
{
    public class BookEntityConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.BookGuid);

            builder.Property(b => b.BookGuid).ValueGeneratedOnAdd();
            
            builder.Property(b => b.BookName).IsRequired();
            
            builder.Property(b => b.ISBN10).IsRequired();
            
            builder.Property(b => b.ISBN13).IsRequired();
            
            builder.Property(b => b.PagesCount).IsRequired();
            
            builder.Property(b => b.PublisherGuid).IsRequired();
            
            builder.Property(b => b.CategoryGuid).IsRequired();

            builder.HasOne(b => b.Category)
                   .WithMany(p => p.Books)
                   .HasForeignKey(b => b.CategoryGuid);

            builder.HasOne(b => b.Publisher)
                   .WithMany(p => p.PublisherBooks)
                   .HasForeignKey(b => b.PublisherGuid);
        }
    }
}