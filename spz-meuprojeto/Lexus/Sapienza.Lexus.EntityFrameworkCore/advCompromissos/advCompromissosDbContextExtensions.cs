using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advCompromissosDbContextModelCreatingExtensions
{
    public static void ConfigureadvCompromissos(this ModelBuilder builder)
    {
        builder.Entity<advCompromissos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advCompromissoses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advTarefas>()
                .WithMany(p => p.advTarefases)
                .HasForeignKey(x => x.advTarefasId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
