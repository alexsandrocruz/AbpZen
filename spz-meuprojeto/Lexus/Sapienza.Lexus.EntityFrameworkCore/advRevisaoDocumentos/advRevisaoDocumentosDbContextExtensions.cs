using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advRevisaoDocumentosDbContextModelCreatingExtensions
{
    public static void ConfigureadvRevisaoDocumentos(this ModelBuilder builder)
    {
        builder.Entity<advRevisaoDocumentos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advRevisaoDocumentoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
