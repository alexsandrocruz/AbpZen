using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwConfigDbContextModelCreatingExtensions
{
    public static void ConfigureflwConfig(this ModelBuilder builder)
    {
        builder.Entity<flwConfig>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "flwConfigs", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
