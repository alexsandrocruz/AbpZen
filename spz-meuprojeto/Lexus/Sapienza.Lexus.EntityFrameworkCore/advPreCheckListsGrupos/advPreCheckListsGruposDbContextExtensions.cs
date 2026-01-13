using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreCheckListsGruposDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreCheckListsGrupos(this ModelBuilder builder)
    {
        builder.Entity<advPreCheckListsGrupos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advPreCheckListsGruposes", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
