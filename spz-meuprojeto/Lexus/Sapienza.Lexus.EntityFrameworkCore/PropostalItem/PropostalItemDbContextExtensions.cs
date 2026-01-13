using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class PropostalItemDbContextModelCreatingExtensions
{
    public static void ConfigurePropostalItem(this ModelBuilder builder)
    {
        builder.Entity<PropostalItem>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "PropostalItems", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Proposal>()
                .WithMany(p => p.PropostalItems)
                .HasForeignKey(x => x.ProposalId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
