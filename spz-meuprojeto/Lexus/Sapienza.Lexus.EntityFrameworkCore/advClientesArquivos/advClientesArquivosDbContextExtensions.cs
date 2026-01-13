using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advClientesArquivosDbContextModelCreatingExtensions
{
    public static void ConfigureadvClientesArquivos(this ModelBuilder builder)
    {
        builder.Entity<advClientesArquivos>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advClientesArquivoses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
