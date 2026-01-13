using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProMeritosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProMeritos(this ModelBuilder builder)
    {
        builder.Entity<advProMeritos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProMeritoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
