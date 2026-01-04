using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class LeadMessageDbContextModelCreatingExtensions
{
    public static void ConfigureLeadMessage(this ModelBuilder builder)
    {
        builder.Entity<LeadMessage>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "LeadMessages", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<MessageTemplate>()
                .WithMany(p => p.LeadMessages)
                .HasForeignKey(x => x.MessageTemplateId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            b.HasOne<Lead>()
                .WithMany(p => p.LeadMessages)
                .HasForeignKey(x => x.LeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
