using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadMessageTemplateDbContextModelCreatingExtensions
{
    public static void ConfigureLeadMessageTemplate(this ModelBuilder builder)
    {
        builder.Entity<LeadMessageTemplate>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadMessageTemplates", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.LeadMessageTemplates)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
