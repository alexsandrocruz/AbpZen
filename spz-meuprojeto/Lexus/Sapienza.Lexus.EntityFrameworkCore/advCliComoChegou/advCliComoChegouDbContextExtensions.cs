using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliComoChegouDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliComoChegou(this ModelBuilder builder)
    {
        builder.Entity<advCliComoChegou>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliComoChegous", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
