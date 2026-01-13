using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliTiposHistoricosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliTiposHistoricos(this ModelBuilder builder)
    {
        builder.Entity<advCliTiposHistoricos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliTiposHistoricoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
