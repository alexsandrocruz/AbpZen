using Volo.Abp.MongoDB;

namespace Volo.Abp.OpenIddict.MongoDB;

public static class OpenIddictProDbContextModelCreatingExtensions
{
    public static void ConfigureOpenIddictPro(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureOpenIddict();
    }
}
