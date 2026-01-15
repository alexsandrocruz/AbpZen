using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class MessageAttachmentDbContextModelCreatingExtensions
{
    public static void ConfigureMessageAttachment(this ModelBuilder builder)
    {
        builder.Entity<MessageAttachment>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "MessageAttachments", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FileName).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<ChatMessage>()
                .WithMany(p => p.MessageAttachments)
                .HasForeignKey(x => x.ChatMessageId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
