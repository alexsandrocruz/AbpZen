using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

public static class OpenIddictProDbContextModelCreatingExtensions
{
    public static void ConfigureOpenIddictPro(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureOpenIddict();
        builder.TryConfigureObjectExtensions<OpenIddictProDbContext>();
    }
}
