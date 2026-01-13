using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuAreasDbContextModelCreatingExtensions
{
    public static void ConfigureusuAreas(this ModelBuilder builder)
    {
        builder.Entity<usuAreas>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "usuAreases", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
