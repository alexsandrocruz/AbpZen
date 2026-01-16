using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesINSSDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesINSS(this ModelBuilder builder)
    {
        builder.Entity<advClientesINSS>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advClientesINSSes", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.tsInclusao).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
