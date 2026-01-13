using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCliTiposArquivosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCliTiposArquivos(this ModelBuilder builder)
    {
        builder.Entity<advCliTiposArquivos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advCliTiposArquivoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
