using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientes_bkpDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientes_bkp(this ModelBuilder builder)
    {
        builder.Entity<advClientes_bkp>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientes_bkps", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
