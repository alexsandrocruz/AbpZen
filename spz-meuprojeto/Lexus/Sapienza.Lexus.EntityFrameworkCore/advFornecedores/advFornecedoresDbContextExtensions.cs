using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advFornecedoresDbContextModelCreatingExtensions
{
    public static void ConfigureadvFornecedores(this ModelBuilder builder)
    {
        builder.Entity<advFornecedores>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "advFornecedoreses", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
