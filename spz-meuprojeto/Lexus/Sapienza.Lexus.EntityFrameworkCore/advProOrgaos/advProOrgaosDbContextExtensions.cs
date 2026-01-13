using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProOrgaosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProOrgaos(this ModelBuilder builder)
    {
        builder.Entity<advProOrgaos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProOrgaoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
