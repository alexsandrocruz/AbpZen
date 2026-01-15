using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadFormSubmissionDbContextModelCreatingExtensions
{
    public static void ConfigureLeadFormSubmission(this ModelBuilder builder)
    {
        builder.Entity<LeadFormSubmission>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadFormSubmissions", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadForm>()
                .WithMany(p => p.LeadFormSubmissions)
                .HasForeignKey(x => x.LeadFormId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
