using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Cursos.EntityFrameworkCore;

public static class NewEntity1DbContextModelCreatingExtensions
{
    public static void ConfigureNewEntity1(this ModelBuilder builder)
    {
        builder.Entity<NewEntity1>(b =>
        {
            b.ToTable(Sapienza.CursosConsts.DbTablePrefix + "NewEntity1s", Sapienza.CursosConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
