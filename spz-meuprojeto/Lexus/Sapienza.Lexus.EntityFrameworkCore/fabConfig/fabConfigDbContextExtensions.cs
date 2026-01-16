using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabConfigDbContextModelCreatingExtensions
{
    public static void ConfigurefabConfig(this ModelBuilder builder)
    {
        builder.Entity<fabConfig>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "fabConfigs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
