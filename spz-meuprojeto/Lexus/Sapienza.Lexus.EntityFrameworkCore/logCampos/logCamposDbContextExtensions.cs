using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class logCamposDbContextModelCreatingExtensions
{
    public static void ConfigurelogCampos(this ModelBuilder builder)
    {
        builder.Entity<logCampos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "logCamposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
