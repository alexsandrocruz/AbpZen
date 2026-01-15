using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Sapienza.Dominus.EntityFrameworkCore;

public static class AiChatSessionDbContextModelCreatingExtensions
{
    public static void ConfigureAiChatSession(this ModelBuilder builder)
    {
        builder.Entity<AiChatSession>(b =>
        {
            b.ToTable(DominusConsts.DbTablePrefix + "AiChatSessions", DominusConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
