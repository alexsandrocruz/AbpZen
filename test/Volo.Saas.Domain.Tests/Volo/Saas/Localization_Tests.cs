using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Volo.Saas.Localization;
using Xunit;

namespace Volo.Saas;

public class Localization_Tests : SaasDomainTestBase
{
    [Fact]
    public void Localization_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<SaasResource>>()["Permission:Saas"].Value.ShouldBe("Saas");
        }
    }
}
