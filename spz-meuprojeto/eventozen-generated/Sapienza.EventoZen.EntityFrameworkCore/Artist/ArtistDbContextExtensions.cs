using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class ArtistDbContextModelCreatingExtensions
{
    public static void ConfigureArtist(this ModelBuilder builder)
    {
        builder.Entity<Artist>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "Artists", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Biography).HasMaxLength(2048);
            b.Property(x => x.PhotoUrl).HasMaxLength(256);
            b.Property(x => x.InstagramHandle).HasMaxLength(256);
            b.Property(x => x.WebsiteUrl).HasMaxLength(256);
            b.Property(x => x.LogoUrl).HasMaxLength(256);
            b.Property(x => x.BannerUrl).HasMaxLength(256);
            b.Property(x => x.HexColor).HasMaxLength(7);

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
