using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class opoOrcamentosDbContextModelCreatingExtensions
{
    public static void ConfigureopoOrcamentos(this ModelBuilder builder)
    {
        builder.Entity<opoOrcamentos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "opoOrcamentoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
