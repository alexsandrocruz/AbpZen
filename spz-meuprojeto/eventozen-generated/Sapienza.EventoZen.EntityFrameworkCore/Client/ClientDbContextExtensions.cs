using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.EventoZen.EntityFrameworkCore;

public static class ClientDbContextModelCreatingExtensions
{
    public static void ConfigureClient(this ModelBuilder builder)
    {
        builder.Entity<Client>(b =>
        {
            b.ToTable(EventoZenConsts.DbTablePrefix + "Clients", EventoZenConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).HasMaxLength(256);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Document).HasMaxLength(14);
            b.Property(x => x.Email).HasMaxLength(256);
            b.Property(x => x.Phone).HasMaxLength(20);
            b.Property(x => x.Address).HasMaxLength(256);
            b.Property(x => x.City).HasMaxLength(256);
            b.Property(x => x.State).HasMaxLength(2);
            b.Property(x => x.Notes).HasMaxLength(2048);

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
