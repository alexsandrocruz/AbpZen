using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finRecibosDbContextModelCreatingExtensions
{
    public static void ConfigurefinRecibos(this ModelBuilder builder)
    {
        builder.Entity<finRecibos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finReciboses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
