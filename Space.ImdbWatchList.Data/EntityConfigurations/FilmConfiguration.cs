using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Space.ImdbWatchList.Data.Entities;

namespace Space.ImdbWatchList.Data
{
    public class FilmConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasMaxLength(10);

            builder.Property(f => f.Rating)
                .HasPrecision(3,1);

            builder.Property(f => f.Title)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}