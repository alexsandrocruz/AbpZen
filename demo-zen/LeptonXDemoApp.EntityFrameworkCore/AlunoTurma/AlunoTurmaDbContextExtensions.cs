using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class AlunoTurmaDbContextModelCreatingExtensions
{
    public static void ConfigureAlunoTurma(this ModelBuilder builder)
    {
        builder.Entity<AlunoTurma>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "AlunoTurmas", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Situacao).HasMaxLength(50);

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Aluno>()
                .WithMany(p => p.AlunoTurmas)
                .HasForeignKey(x => x.AlunoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Turma>()
                .WithMany(p => p.AlunoTurmas)
                .HasForeignKey(x => x.TurmaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
