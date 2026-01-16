using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwConfigDbContextModelCreatingExtensions
{
    public static void ConfigureflwConfig(this ModelBuilder builder)
    {
        builder.Entity<flwConfig>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "flwConfigs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
