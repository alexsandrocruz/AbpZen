using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProVarasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProVaras(this ModelBuilder builder)
    {
        builder.Entity<advProVaras>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProVarases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
