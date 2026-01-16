using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosHonorariosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosHonorarios(this ModelBuilder builder)
    {
        builder.Entity<advProcessosHonorarios>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advProcessosHonorarioses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advProcessosDadosHerdeiros>()
                .WithMany(p => p.advProcessosDadosHerdeiroses)
                .HasForeignKey(x => x.advProcessosDadosHerdeirosId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
