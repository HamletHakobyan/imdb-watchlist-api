using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Space.ImdbWatchList.Data.Entities;

namespace Space.ImdbWatchList.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(user => user.Name)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(u => u.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Hamlet",
                        Email = "hamlet.h.hakobyan@gmail.com",
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Aleq",
                        Email = "aleq.hakobyan@gmail.com",
                    });
        }
    }
}