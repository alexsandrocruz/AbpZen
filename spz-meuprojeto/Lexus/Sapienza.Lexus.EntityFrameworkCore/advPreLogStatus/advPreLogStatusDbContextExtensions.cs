using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreLogStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreLogStatus(this ModelBuilder builder)
    {
        builder.Entity<advPreLogStatus>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreLogStatuses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
