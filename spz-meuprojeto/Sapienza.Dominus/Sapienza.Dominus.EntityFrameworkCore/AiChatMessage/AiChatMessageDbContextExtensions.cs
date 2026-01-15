using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class AiChatMessageDbContextModelCreatingExtensions
{
    public static void ConfigureAiChatMessage(this ModelBuilder builder)
    {
        builder.Entity<AiChatMessage>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "AiChatMessages", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Role).IsRequired();
            b.Property(x => x.Content).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
            b.HasOne<AiChatSession>()
                .WithMany(p => p.AiChatMessages)
                .HasForeignKey(x => x.AiChatSessionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
