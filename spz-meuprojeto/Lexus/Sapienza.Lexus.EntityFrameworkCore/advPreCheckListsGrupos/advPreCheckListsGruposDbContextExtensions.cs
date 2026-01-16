using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class advPreCheckListsGruposDbContextModelCreatingExtensions
{
    public static void ConfigureadvPreCheckListsGrupos(this ModelBuilder builder)
    {
        builder.Entity<advPreCheckListsGrupos>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "advPreCheckListsGruposes", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<advPreCheckLists>()
                .WithMany(p => p.advPreCheckListses)
                .HasForeignKey(x => x.advPreCheckListsId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
