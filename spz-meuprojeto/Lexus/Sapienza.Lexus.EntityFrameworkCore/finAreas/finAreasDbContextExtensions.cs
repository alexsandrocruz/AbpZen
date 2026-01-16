using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finAreasDbContextModelCreatingExtensions
{
    public static void ConfigurefinAreas(this ModelBuilder builder)
    {
        builder.Entity<finAreas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finAreases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finLancamentos>()
                .WithMany(p => p.finLancamentoses)
                .HasForeignKey(x => x.finLancamentosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
