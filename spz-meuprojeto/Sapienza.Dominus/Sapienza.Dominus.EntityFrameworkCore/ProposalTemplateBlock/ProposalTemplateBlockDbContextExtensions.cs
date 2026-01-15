using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProposalTemplateBlockDbContextModelCreatingExtensions
{
    public static void ConfigureProposalTemplateBlock(this ModelBuilder builder)
    {
        builder.Entity<ProposalTemplateBlock>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ProposalTemplateBlocks", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.BlockType).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
