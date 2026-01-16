using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class finGruposDREDbContextModelCreatingExtensions
{
    public static void ConfigurefinGruposDRE(this ModelBuilder builder)
    {
        builder.Entity<finGruposDRE>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "finGruposDREs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<finPlanoContasGrupos>()
                .WithMany(p => p.finPlanoContasGruposes)
                .HasForeignKey(x => x.finPlanoContasGruposId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
