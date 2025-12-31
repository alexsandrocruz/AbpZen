using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

public static class IdentityProMongoDbContextExtensions
{
    public static void ConfigureIdentityPro(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureIdentity();
    }
}
