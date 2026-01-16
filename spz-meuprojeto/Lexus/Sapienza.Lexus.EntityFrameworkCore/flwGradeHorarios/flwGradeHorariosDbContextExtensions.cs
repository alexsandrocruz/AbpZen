using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwGradeHorariosDbContextModelCreatingExtensions
{
    public static void ConfigureflwGradeHorarios(this ModelBuilder builder)
    {
        builder.Entity<flwGradeHorarios>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "flwGradeHorarioses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
