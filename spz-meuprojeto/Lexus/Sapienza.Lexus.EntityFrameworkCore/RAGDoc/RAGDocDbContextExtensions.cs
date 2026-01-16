using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class RAGDocDbContextModelCreatingExtensions
{
    public static void ConfigureRAGDoc(this ModelBuilder builder)
    {
        builder.Entity<RAGDoc>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "RAGDocs", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
