using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProposalBlockInstanceDbContextModelCreatingExtensions
{
    public static void ConfigureProposalBlockInstance(this ModelBuilder builder)
    {
        builder.Entity<ProposalBlockInstance>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ProposalBlockInstances", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.BlockType).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Proposal>()
                .WithMany(p => p.ProposalBlockInstances)
                .HasForeignKey(x => x.ProposalId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
