using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class AlunoDbContextModelCreatingExtensions
{
    public static void ConfigureAluno(this ModelBuilder builder)
    {
        builder.Entity<Aluno>(b =>
        {
            b.ToTable(CursosConsts.DbTablePrefix + "Alunos", CursosConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
