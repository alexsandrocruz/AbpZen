using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Identity;

public class Localization_Tests : AbpIdentityDomainTestBase
{
    [Fact]
    public void Localization_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<IdentityResource>>()["Permission:IdentityManagement"].Value.ShouldBe("Identity management");
        }
    }
}
