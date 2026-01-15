using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadScheduledMessageDbContextModelCreatingExtensions
{
    public static void ConfigureLeadScheduledMessage(this ModelBuilder builder)
    {
        builder.Entity<LeadScheduledMessage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadScheduledMessages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Status).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadAutomation>()
                .WithMany(p => p.LeadScheduledMessages)
                .HasForeignKey(x => x.LeadAutomationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
