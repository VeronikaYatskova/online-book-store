using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasOne(e => e.Role)
                   .WithMany(e => e.Users)
                   .HasForeignKey(e => e.RoleId);

            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}
