using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class ProposalDbContextModelCreatingExtensions
{
    public static void ConfigureProposal(this ModelBuilder builder)
    {
        builder.Entity<Proposal>(b =>
        {
            b.ToTable(Sapienza.LexusConsts.DbTablePrefix + "Proposals", Sapienza.LexusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Proposals)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
