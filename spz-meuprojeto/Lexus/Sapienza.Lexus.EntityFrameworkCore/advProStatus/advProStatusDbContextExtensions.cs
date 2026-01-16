using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvProStatus(this ModelBuilder builder)
    {
        builder.Entity<advProStatus>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProStatuses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
