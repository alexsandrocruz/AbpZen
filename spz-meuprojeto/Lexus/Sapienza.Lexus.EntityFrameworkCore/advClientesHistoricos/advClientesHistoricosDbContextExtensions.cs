using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesHistoricosDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesHistoricos(this ModelBuilder builder)
    {
        builder.Entity<advClientesHistoricos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advClientesHistoricoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.data).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
