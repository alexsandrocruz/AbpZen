using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreProcessosCheckListsDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreProcessosCheckLists(this ModelBuilder builder)
    {
        builder.Entity<advPreProcessosCheckLists>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreProcessosCheckListses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
