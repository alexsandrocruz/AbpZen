using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;

namespace Volo.Saas.MongoDB;

public static class SaasMongoDbContextExtensions
{
    public static void ConfigureSaas(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Tenant>(b =>
        {
            b.CollectionName = SaasDbProperties.DbTablePrefix + "Tenants";
        });

        builder.Entity<Edition>(b =>
        {
            b.CollectionName = SaasDbProperties.DbTablePrefix + "Editions";
        });
    }
}
