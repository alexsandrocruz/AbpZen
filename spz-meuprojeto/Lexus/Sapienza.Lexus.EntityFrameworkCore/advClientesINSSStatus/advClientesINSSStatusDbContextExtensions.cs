using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesINSSStatusDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesINSSStatus(this ModelBuilder builder)
    {
        builder.Entity<advClientesINSSStatus>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advClientesINSSStatuses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
