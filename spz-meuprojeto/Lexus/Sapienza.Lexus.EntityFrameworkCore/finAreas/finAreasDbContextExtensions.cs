using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finAreasDbContextModelCreatingExtensions
{
    public static void ConfigurefinAreas(this ModelBuilder builder)
    {
        builder.Entity<finAreas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "finAreases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
