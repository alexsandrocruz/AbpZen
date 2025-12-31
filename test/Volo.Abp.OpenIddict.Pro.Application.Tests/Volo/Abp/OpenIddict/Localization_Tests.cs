using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict.Localization;
using Xunit;

namespace Volo.Abp.OpenIddict;

public class Localization_Tests : OpenIddictProTestBase<AbpOpenIddictProDomainSharedModule>
{
    [Fact]
    public void Localization_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<AbpOpenIddictResource>>()["Permission:OpenIddictPro"].Value.ShouldBe("OpenId");
        }
    }
}
