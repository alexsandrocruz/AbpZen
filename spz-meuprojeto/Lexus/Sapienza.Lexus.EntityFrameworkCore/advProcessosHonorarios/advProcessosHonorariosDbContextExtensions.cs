using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advProcessosHonorariosDbContextModelCreatingExtensions
{
    public static void ConfigureadvProcessosHonorarios(this ModelBuilder builder)
    {
        builder.Entity<advProcessosHonorarios>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advProcessosHonorarioses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
