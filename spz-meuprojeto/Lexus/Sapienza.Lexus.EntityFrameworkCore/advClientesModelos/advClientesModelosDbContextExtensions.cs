using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesModelosDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesModelos(this ModelBuilder builder)
    {
        builder.Entity<advClientesModelos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientesModeloses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
