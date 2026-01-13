using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class ArtistSpecialtyDbContextModelCreatingExtensions
{
    public static void ConfigureArtistSpecialty(this ModelBuilder builder)
    {
        builder.Entity<ArtistSpecialty>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "ArtistSpecialties", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Artist>()
                .WithMany(p => p.ArtistSpecialties)
                .HasForeignKey(x => x.ArtistId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
