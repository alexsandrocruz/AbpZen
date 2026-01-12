#nullable enable
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Lexus.EntityFrameworkCore;

public static class LegalProcessDbContextModelCreatingExtensions
{
    public static void ConfigureLegalProcess(this ModelBuilder builder)
    {
        builder.Entity<LegalProcess>(b =>
        {
            b.ToTable(LexusConsts.DbTablePrefix + "LegalProcesses", LexusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.ProcessNumber).HasMaxLength(32);
            b.Property(x => x.ProcessNumber).IsRequired();
            b.Property(x => x.Title).HasMaxLength(128);
            b.Property(x => x.Title).IsRequired();
            b.Property(x => x.Description).HasMaxLength(2048);

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Lawyer>()
                .WithMany(p => p.Processes)
                .HasForeignKey(x => x.LawyerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne<Client>()
                .WithMany(p => p.Processes)
                .HasForeignKey(x => x.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
