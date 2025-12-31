using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public class Localization_Tests : LanguageManagementDomainTestBase
{
    [Fact]
    public void Localization_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<LanguageManagementResource>>()["Permission:LanguageManagement"].Value.ShouldBe("Language management");
        }
    }
}
