using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProfissionaisDbContextModelCreatingExtensions
{
    public static void ConfigureadvProfissionais(this ModelBuilder builder)
    {
        builder.Entity<advProfissionais>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProfissionaises", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
