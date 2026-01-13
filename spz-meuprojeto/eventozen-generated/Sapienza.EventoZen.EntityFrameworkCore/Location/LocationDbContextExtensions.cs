using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class LocationDbContextModelCreatingExtensions
{
    public static void ConfigureLocation(this ModelBuilder builder)
    {
        builder.Entity<Location>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "Locations", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Address).HasMaxLength(256);
            b.Property(x => x.City).HasMaxLength(256);
            b.Property(x => x.State).HasMaxLength(2);

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
