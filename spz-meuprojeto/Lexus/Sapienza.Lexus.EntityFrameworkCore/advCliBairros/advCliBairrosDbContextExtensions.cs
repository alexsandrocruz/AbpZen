using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliBairrosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliBairros(this ModelBuilder builder)
    {
        builder.Entity<advCliBairros>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCliBairroses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
