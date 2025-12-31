using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.LanguageManagement.External;

namespace Volo.Abp.LanguageManagement.EntityFrameworkCore;

public static class LanguageManagementDbContextModelCreatingExtensions
{
    public static void ConfigureLanguageManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsHostDatabase())
        {
            builder.Entity<Language>(b =>
            {
                b.ToTable(LanguageManagementDbProperties.DbTablePrefix + "Languages", LanguageManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.CultureName).IsRequired().HasColumnName(nameof(Language.CultureName)).HasMaxLength(LanguageConsts.MaxCultureNameLength);
                b.Property(x => x.UiCultureName).IsRequired().HasColumnName(nameof(Language.UiCultureName)).HasMaxLength(LanguageConsts.MaxUiCultureNameLength);
                b.Property(x => x.DisplayName).IsRequired().HasColumnName(nameof(Language.DisplayName)).HasMaxLength(LanguageConsts.MaxDisplayNameLength);
                b.Property(x => x.IsEnabled).IsRequired().HasColumnName(nameof(Language.IsEnabled));

                b.HasIndex(x => x.CultureName);

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.Entity<LanguageText>(b =>
        {
            b.ToTable(LanguageManagementDbProperties.DbTablePrefix + "LanguageTexts", LanguageManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.ResourceName).IsRequired().HasColumnName(nameof(LanguageText.ResourceName)).HasMaxLength(LanguageTextConsts.MaxResourceNameLength);
            b.Property(x => x.Name).IsRequired().HasColumnName(nameof(LanguageText.Name)).HasMaxLength(LanguageTextConsts.MaxKeyNameLength);
            b.Property(x => x.Value).IsRequired().HasColumnName(nameof(LanguageText.Value)).HasMaxLength(LanguageTextConsts.MaxValueLength);
            b.Property(x => x.CultureName).IsRequired().HasColumnName(nameof(LanguageText.CultureName)).HasMaxLength(LanguageTextConsts.MaxCultureNameLength);

            b.HasIndex(x => new { x.TenantId, x.ResourceName, x.CultureName });

            b.ApplyObjectExtensionMappings();
        });

        if (builder.IsHostDatabase())
        {
            builder.Entity<LocalizationResourceRecord>(b =>
            {
                b.ToTable(LanguageManagementDbProperties.DbTablePrefix + "LocalizationResources", LanguageManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasColumnName(nameof(LocalizationResourceRecord.Name)).HasMaxLength(LocalizationResourceRecordConsts.MaxNameLength);
                b.Property(x => x.DefaultCulture).HasColumnName(nameof(LocalizationResourceRecord.DefaultCulture)).HasMaxLength(LocalizationResourceRecordConsts.MaxDefaultCultureLength);
                b.Property(x => x.BaseResources).HasColumnName(nameof(LocalizationResourceRecord.BaseResources)).HasMaxLength(LocalizationResourceRecordConsts.MaxBaseResourcesLength);
                b.Property(x => x.SupportedCultures).HasColumnName(nameof(LocalizationResourceRecord.SupportedCultures)).HasMaxLength(LocalizationResourceRecordConsts.MaxSupportedCulturesLength);

                b.HasIndex(x => new { x.Name }).IsUnique();
            });

            builder.Entity<LocalizationTextRecord>(b =>
            {
                b.ToTable(LanguageManagementDbProperties.DbTablePrefix + "LocalizationTexts", LanguageManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.ResourceName).IsRequired().HasColumnName(nameof(LocalizationTextRecord.ResourceName)).HasMaxLength(LocalizationTextRecordConsts.MaxResourceNameLength);
                b.Property(x => x.CultureName).IsRequired().HasColumnName(nameof(LocalizationTextRecord.CultureName)).HasMaxLength(LocalizationTextRecordConsts.MaxCultureNameLength);
                b.Property(x => x.Value).HasColumnName(nameof(LocalizationTextRecord.Value)).HasMaxLength(LocalizationTextRecordConsts.MaxValueLength);

                b.HasIndex(x => new { x.ResourceName, x.CultureName }).IsUnique();
            });
        }

        builder.TryConfigureObjectExtensions<LanguageManagementDbContext>();
    }
}
