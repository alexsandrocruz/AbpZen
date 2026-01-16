using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advVerbasDbContextModelCreatingExtensions
{
    public static void ConfigureadvVerbas(this ModelBuilder builder)
    {
        builder.Entity<advVerbas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advVerbases", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.dataDe).IsRequired();
            b.Property(x => x.dataAte).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
