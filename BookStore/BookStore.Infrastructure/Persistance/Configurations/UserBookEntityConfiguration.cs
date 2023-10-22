using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.Persistance.Configurations
{
    public class UserBookEntityConfiguration : IEntityTypeConfiguration<UserBookEntity>
    {
        public void Configure(EntityTypeBuilder<UserBookEntity> builder)
        {
            builder.Property(fb => fb.Id).ValueGeneratedOnAdd();

            builder.HasKey(fb => new { fb.BookId, fb.UserId });

            builder.HasOne(b => b.Book)
                   .WithMany(p => p.UserBooks)
                   .HasForeignKey(b => b.BookId);

            builder.HasOne(b => b.User)
                   .WithMany(p => p.UserBooks)
                   .HasForeignKey(b => b.UserId);
        }
    }
}
