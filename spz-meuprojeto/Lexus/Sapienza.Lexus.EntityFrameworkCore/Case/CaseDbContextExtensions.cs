using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class CaseDbContextModelCreatingExtensions
{
    public static void ConfigureCase(this ModelBuilder builder)
    {
        builder.Entity<Case>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "Cases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
