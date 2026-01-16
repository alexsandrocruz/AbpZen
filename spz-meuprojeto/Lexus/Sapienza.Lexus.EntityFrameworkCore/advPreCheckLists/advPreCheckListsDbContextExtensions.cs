using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreCheckListsDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreCheckLists(this ModelBuilder builder)
    {
        builder.Entity<advPreCheckLists>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreCheckListses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
