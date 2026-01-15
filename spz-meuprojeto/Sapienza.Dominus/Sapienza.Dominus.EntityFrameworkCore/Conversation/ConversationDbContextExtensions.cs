using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class ConversationDbContextModelCreatingExtensions
{
    public static void ConfigureConversation(this ModelBuilder builder)
    {
        builder.Entity<Conversation>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "Conversations", DominusConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<Client>()
                .WithMany(p => p.Conversations)
                .HasForeignKey(x => x.ClientId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
