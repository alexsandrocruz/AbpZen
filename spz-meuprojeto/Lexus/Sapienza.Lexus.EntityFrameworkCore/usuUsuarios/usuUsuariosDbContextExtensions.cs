using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuUsuariosDbContextModelCreatingExtensions
{
    public static void ConfigureusuUsuarios(this ModelBuilder builder)
    {
        builder.Entity<usuUsuarios>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "usuUsuarioses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
