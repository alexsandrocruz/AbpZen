using Volo.Abp.LanguageManagement.External;
using Volo.Abp.MongoDB;

namespace Volo.Abp.LanguageManagement.MongoDB;

public static class LanguageManagementMongoDbContextExtensions
{
    public static void ConfigureLanguageManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Language>(b =>
        {
            b.CollectionName = LanguageManagementDbProperties.DbTablePrefix + "Languages";
        });

        builder.Entity<LanguageText>(b =>
        {
            b.CollectionName = LanguageManagementDbProperties.DbTablePrefix + "LanguageTexts";
        });
        
        builder.Entity<LocalizationResourceRecord>(b =>
        {
            b.CollectionName = LanguageManagementDbProperties.DbTablePrefix + "LocalizationResources";
        });
        
        builder.Entity<LocalizationTextRecord>(b =>
        {
            b.CollectionName = LanguageManagementDbProperties.DbTablePrefix + "LocalizationTexts";
        });
    }
}
