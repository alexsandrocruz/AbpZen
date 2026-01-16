using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuAcessosDbContextModelCreatingExtensions
{
    public static void ConfigureusuAcessos(this ModelBuilder builder)
    {
        builder.Entity<usuAcessos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "usuAcessoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
