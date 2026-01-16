using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreStatus(this ModelBuilder builder)
    {
        builder.Entity<advPreStatus>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreStatuses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
