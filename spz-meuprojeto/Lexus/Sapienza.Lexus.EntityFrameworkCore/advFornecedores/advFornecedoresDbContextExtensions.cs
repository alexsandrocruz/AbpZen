using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advFornecedoresDbContextModelCreatingExtensions
{
    public static void ConfigureadvFornecedores(this ModelBuilder builder)
    {
        builder.Entity<advFornecedores>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advFornecedoreses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advClientes>()
                .WithMany(p => p.advClienteses)
                .HasForeignKey(x => x.advClientesId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
