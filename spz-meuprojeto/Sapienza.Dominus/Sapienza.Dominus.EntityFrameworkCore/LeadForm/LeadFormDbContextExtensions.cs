using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadFormDbContextModelCreatingExtensions
{
    public static void ConfigureLeadForm(this ModelBuilder builder)
    {
        builder.Entity<LeadForm>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadForms", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadWorkflow>()
                .WithMany(p => p.LeadForms)
                .HasForeignKey(x => x.LeadWorkflowId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
