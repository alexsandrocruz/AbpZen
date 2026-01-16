using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class flwAcoesDbContextModelCreatingExtensions
{
    public static void ConfigureflwAcoes(this ModelBuilder builder)
    {
        builder.Entity<flwAcoes>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "flwAcoeses", LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<flwFollows>()
                .WithMany(p => p.flwFollowses)
                .HasForeignKey(x => x.flwFollowsId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
