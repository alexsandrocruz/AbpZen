using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class LeadFormFieldDbContextModelCreatingExtensions
{
    public static void ConfigureLeadFormField(this ModelBuilder builder)
    {
        builder.Entity<LeadFormField>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "LeadFormFields", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Label).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<LeadForm>()
                .WithMany(p => p.LeadFormFields)
                .HasForeignKey(x => x.LeadFormId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
