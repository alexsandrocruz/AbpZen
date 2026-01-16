using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProNaturezasDbContextModelCreatingExtensions
{
    public static void ConfigureadvProNaturezas(this ModelBuilder builder)
    {
        builder.Entity<advProNaturezas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProNaturezases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advProfissionaisNaturezas>()
                .WithMany(p => p.advProfissionaisNaturezases)
                .HasForeignKey(x => x.advProfissionaisNaturezasId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
