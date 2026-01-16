using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuCargosDbContextModelCreatingExtensions
{
    public static void ConfigureusuCargos(this ModelBuilder builder)
    {
        builder.Entity<usuCargos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "usuCargoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
