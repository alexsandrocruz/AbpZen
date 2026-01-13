using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliCargosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliCargos(this ModelBuilder builder)
    {
        builder.Entity<advCliCargos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliCargoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
