using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class usuAreasDbContextModelCreatingExtensions
{
    public static void ConfigureusuAreas(this ModelBuilder builder)
    {
        builder.Entity<usuAreas>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "usuAreases", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<usuCargos>()
                .WithMany(p => p.usuCargoses)
                .HasForeignKey(x => x.usuCargosId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
