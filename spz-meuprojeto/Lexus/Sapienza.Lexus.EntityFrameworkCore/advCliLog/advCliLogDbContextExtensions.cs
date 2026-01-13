using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliLogDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliLog(this ModelBuilder builder)
    {
        builder.Entity<advCliLog>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliLogs", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
