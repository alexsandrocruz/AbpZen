using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class TransactionAttachmentDbContextModelCreatingExtensions
{
    public static void ConfigureTransactionAttachment(this ModelBuilder builder)
    {
        builder.Entity<TransactionAttachment>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "TransactionAttachments", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FileName).IsRequired();
            b.Property(x => x.FileUrl).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Transaction>()
                .WithMany(p => p.TransactionAttachments)
                .HasForeignKey(x => x.TransactionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
