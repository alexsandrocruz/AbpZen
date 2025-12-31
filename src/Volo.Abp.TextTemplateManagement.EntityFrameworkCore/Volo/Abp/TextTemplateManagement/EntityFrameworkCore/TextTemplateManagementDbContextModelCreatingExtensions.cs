using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.TextTemplateManagement.TextTemplates;

namespace Volo.Abp.TextTemplateManagement.EntityFrameworkCore;

public static class TextTemplateManagementDbContextModelCreatingExtensions
{
    public static void ConfigureTextTemplateManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<TextTemplateContent>(b =>
        {
            b.ToTable(TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplateContents", TextTemplateManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(e => e.Name).IsRequired().HasMaxLength(TextTemplateConsts.MaxNameLength);
            b.Property(e => e.CultureName).HasMaxLength(TextTemplateConsts.MaxCultureNameLength);
            b.Property(e => e.Content).IsRequired().HasMaxLength(TextTemplateConsts.MaxContentLength);

            b.ApplyObjectExtensionMappings();
        });

        if (builder.IsHostDatabase())
        {
            builder.Entity<TextTemplateDefinitionRecord>(b =>
            {
                b.ToTable(TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplateDefinitionRecords", TextTemplateManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(TemplateDefinitionRecordConsts.MaxNameLength).IsRequired();
                b.Property(x => x.DisplayName).HasMaxLength(TemplateDefinitionRecordConsts.MaxDisplayNameLength);
                b.Property(x => x.Layout).HasMaxLength(TemplateDefinitionRecordConsts.MaxLayoutLength);
                b.Property(x => x.LocalizationResourceName).HasMaxLength(TemplateDefinitionRecordConsts.MaxLocalizationResourceNameLength);
                b.Property(x => x.DefaultCultureName).HasMaxLength(TemplateDefinitionRecordConsts.MaxDefaultCultureNameLength);
                b.Property(x => x.RenderEngine).HasMaxLength(TemplateDefinitionRecordConsts.MaxRenderEngineLength);

                b.HasMany<TextTemplateDefinitionContentRecord>().WithOne().HasForeignKey(x => x.DefinitionId);

                b.HasIndex(x => new { x.Name }).IsUnique();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<TextTemplateDefinitionContentRecord>(b =>
            {
                b.ToTable(TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplateDefinitionContentRecords", TextTemplateManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.FileName).HasMaxLength(TemplateDefinitionContentRecordConsts.MaxFileNameLength).IsRequired();

                b.HasOne<TextTemplateDefinitionRecord>().WithMany().HasForeignKey(x => x.DefinitionId);

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.TryConfigureObjectExtensions<TextTemplateManagementDbContext>();
    }
}
