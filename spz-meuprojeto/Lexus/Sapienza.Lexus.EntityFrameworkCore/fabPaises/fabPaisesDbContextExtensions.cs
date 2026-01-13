using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class fabPaisesDbContextModelCreatingExtensions
{
    public static void ConfigurefabPaises(this ModelBuilder builder)
    {
        builder.Entity<fabPaises>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "fabPaiseses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
