using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class LeadContactDbContextModelCreatingExtensions
{
    public static void ConfigureLeadContact(this ModelBuilder builder)
    {
        builder.Entity<LeadContact>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "LeadContacts", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Lead>()
                .WithMany(p => p.LeadContacts)
                .HasForeignKey(x => x.LeadId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
