using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuPermissoesDbContextModelCreatingExtensions
{
    public static void ConfigureusuPermissoes(this ModelBuilder builder)
    {
        builder.Entity<usuPermissoes>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "usuPermissoeses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
