using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreMetasDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreMetas(this ModelBuilder builder)
    {
        builder.Entity<advPreMetas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreMetases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
