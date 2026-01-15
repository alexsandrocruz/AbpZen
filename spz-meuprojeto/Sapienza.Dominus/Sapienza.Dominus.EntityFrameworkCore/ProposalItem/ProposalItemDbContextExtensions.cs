using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProposalItemDbContextModelCreatingExtensions
{
    public static void ConfigureProposalItem(this ModelBuilder builder)
    {
        builder.Entity<ProposalItem>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ProposalItems", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Description).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Proposal>()
                .WithMany(p => p.ProposalItems)
                .HasForeignKey(x => x.ProposalId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
