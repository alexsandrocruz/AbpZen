using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ChatMessageDbContextModelCreatingExtensions
{
    public static void ConfigureChatMessage(this ModelBuilder builder)
    {
        builder.Entity<ChatMessage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "ChatMessages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Content).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Conversation>()
                .WithMany(p => p.ChatMessages)
                .HasForeignKey(x => x.ConversationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
