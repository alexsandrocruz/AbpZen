using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class TurmaDbContextModelCreatingExtensions
{
    public static void ConfigureTurma(this ModelBuilder builder)
    {
        builder.Entity<Turma>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Turmas", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Nome).HasMaxLength(100);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Codigo).HasMaxLength(20);
            b.Property(x => x.Codigo).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
