using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finLancamentosDbContextModelCreatingExtensions
{
    public static void ConfigurefinLancamentos(this ModelBuilder builder)
    {
        builder.Entity<finLancamentos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finLancamentoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
