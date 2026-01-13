using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProRelevanciasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProRelevancias(this ModelBuilder builder)
    {
        builder.Entity<advProRelevancias>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProRelevanciases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
