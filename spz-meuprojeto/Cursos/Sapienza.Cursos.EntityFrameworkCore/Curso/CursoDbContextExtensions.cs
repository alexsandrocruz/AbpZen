using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class CursoDbContextModelCreatingExtensions
{
    public static void ConfigureCurso(this ModelBuilder builder)
    {
        builder.Entity<Curso>(b =>
        {
            b.ToTable(Sapienza.CursosConsts.DbTablePrefix + "Cursos", Sapienza.CursosConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
