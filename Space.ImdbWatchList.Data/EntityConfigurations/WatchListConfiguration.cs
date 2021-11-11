using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Space.ImdbWatchList.Data.Entities;

namespace Space.ImdbWatchList.Data
{
    public class WatchListConfiguration : IEntityTypeConfiguration<WatchList>
    {
        public void Configure(EntityTypeBuilder<WatchList> builder)
        {

            builder.HasKey(wl => new { wl.UserId, wl.FilmId });

            builder.Property(wl => wl.FilmId)
                .HasMaxLength(10);
            builder.Property(wl => wl.Added)
                .HasDefaultValueSql("getutcdate()");

            builder.HasOne(wl => wl.User)
                .WithMany(u => u.FilmsInWatchList)
                .HasForeignKey(wl => wl.UserId);

            builder.HasOne(wl => wl.Film)
                .WithMany(f => f.InterestedUsers)
                .HasForeignKey(wl => wl.FilmId);
        }
    }
}