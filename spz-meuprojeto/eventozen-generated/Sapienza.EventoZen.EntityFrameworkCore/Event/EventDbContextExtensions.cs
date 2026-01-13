using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class EventDbContextModelCreatingExtensions
{
    public static void ConfigureEvent(this ModelBuilder builder)
    {
        builder.Entity<Event>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "Events", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).HasMaxLength(256);
            b.Property(x => x.Description).HasMaxLength(2048);
            b.Property(x => x.ContractType).HasMaxLength(50);
            b.Property(x => x.NegotiationType).HasMaxLength(50);

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Artist>()
                .WithMany(p => p.Events)
                .HasForeignKey(x => x.ArtistId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<Client>()
                .WithMany(p => p.Events)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<Location>()
                .WithMany(p => p.Events)
                .HasForeignKey(x => x.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
