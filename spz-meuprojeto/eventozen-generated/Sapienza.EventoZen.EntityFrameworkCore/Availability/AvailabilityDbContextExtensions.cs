using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class AvailabilityDbContextModelCreatingExtensions
{
    public static void ConfigureAvailability(this ModelBuilder builder)
    {
        builder.Entity<Availability>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "Availabilities", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Artist>()
                .WithMany(p => p.Availabilities)
                .HasForeignKey(x => x.ArtistId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
