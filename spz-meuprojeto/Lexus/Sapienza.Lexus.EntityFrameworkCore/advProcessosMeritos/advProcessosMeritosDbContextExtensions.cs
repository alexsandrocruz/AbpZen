using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosMeritosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosMeritos(this ModelBuilder builder)
    {
        builder.Entity<advProcessosMeritos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProcessosMeritoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
