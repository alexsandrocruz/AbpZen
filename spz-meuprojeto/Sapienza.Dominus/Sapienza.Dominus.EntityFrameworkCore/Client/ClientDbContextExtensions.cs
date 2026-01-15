using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ClientDbContextModelCreatingExtensions
{
    public static void ConfigureClient(this ModelBuilder builder)
    {
        builder.Entity<Client>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Clients", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Email).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
