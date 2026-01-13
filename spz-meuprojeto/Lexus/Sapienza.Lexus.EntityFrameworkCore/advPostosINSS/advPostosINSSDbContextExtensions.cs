using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPostosINSSDbContextModelCreatingExtensions
{
    public static void ConfigureadvPostosINSS(this ModelBuilder builder)
    {
        builder.Entity<advPostosINSS>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPostosINSSes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
