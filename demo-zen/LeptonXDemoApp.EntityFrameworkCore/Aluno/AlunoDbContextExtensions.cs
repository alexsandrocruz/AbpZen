using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class AlunoDbContextModelCreatingExtensions
{
    public static void ConfigureAluno(this ModelBuilder builder)
    {
        builder.Entity<Aluno>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "Alunos", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Nome).HasMaxLength(100);
            b.Property(x => x.Nome).IsRequired();
            b.Property(x => x.Email).HasMaxLength(200);
            b.Property(x => x.Email).IsRequired();
            b.Property(x => x.Matricula).HasMaxLength(20);
            b.Property(x => x.Matricula).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
