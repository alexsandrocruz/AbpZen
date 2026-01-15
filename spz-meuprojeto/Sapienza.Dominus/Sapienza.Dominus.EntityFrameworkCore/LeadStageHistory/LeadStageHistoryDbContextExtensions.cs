using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadStageHistoryDbContextModelCreatingExtensions
{
    public static void ConfigureLeadStageHistory(this ModelBuilder builder)
    {
        builder.Entity<LeadStageHistory>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadStageHistories", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Lead>()
                .WithMany(p => p.LeadStageHistories)
                .HasForeignKey(x => x.LeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
