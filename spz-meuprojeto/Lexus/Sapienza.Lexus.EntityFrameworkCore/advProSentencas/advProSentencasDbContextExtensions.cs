using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProSentencasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProSentencas(this ModelBuilder builder)
    {
        builder.Entity<advProSentencas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProSentencases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
