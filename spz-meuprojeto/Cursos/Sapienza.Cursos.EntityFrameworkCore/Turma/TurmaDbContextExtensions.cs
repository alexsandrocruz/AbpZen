using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class TurmaDbContextModelCreatingExtensions
{
    public static void ConfigureTurma(this ModelBuilder builder)
    {
        builder.Entity<Turma>(b =>
        {
            b.ToTable(Sapienza.CursosConsts.DbTablePrefix + "Turmas", Sapienza.CursosConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
