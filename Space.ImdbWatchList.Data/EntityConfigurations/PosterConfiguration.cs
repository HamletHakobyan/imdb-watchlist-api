using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Space.ImdbWatchList.Data.Entities;

namespace Space.ImdbWatchList.Data
{
    public class PosterConfiguration : IEntityTypeConfiguration<Poster>
    {
        public void Configure(EntityTypeBuilder<Poster> builder)
        {

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasMaxLength(100);

            builder.Property(f => f.Link)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasOne(p => p.Film)
                .WithMany(f => f.Posters)
                .HasForeignKey(p => p.FilmId)
                .IsRequired();
        }
    }
}