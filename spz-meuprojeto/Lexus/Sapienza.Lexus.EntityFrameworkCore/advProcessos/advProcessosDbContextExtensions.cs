using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessos(this ModelBuilder builder)
    {
        builder.Entity<advProcessos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProcessoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
