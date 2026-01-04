using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LeptonXDemoApp.EntityFrameworkCore;

public static class MessageTemplateDbContextModelCreatingExtensions
{
    public static void ConfigureMessageTemplate(this ModelBuilder builder)
    {
        builder.Entity<MessageTemplate>(b =>
        {
            b.ToTable(LeptonXDemoAppConsts.DbTablePrefix + "MessageTemplates", LeptonXDemoAppConsts.DbSchema);
            b.ConfigureByConvention();

            // ========== Relationship Configuration (1:N) ==========
        });
    }
}
