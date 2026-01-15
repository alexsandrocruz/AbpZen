using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ProposalDbContextModelCreatingExtensions
{
    public static void ConfigureProposal(this ModelBuilder builder)
    {
        builder.Entity<Proposal>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Proposals", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Proposals)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
