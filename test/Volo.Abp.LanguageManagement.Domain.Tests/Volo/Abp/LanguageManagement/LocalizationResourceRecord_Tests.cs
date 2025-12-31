using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Localization;
using Volo.Abp.Validation.Localization;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public class LocalizationResourceRecord_Tests : LanguageManagementDomainTestBase
{
    [Fact]
    public async Task Should_Create_From_LocalizationResourceBase()
    {
        // Arrange
        
        var localizer = GetRequiredService<IStringLocalizer<AbpValidationResource>>();
        var options = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
        var validationResource = options
            .Resources
            .Values
            .Single(r =>
                r.ResourceName == LocalizationResourceNameAttribute.GetName(typeof(AbpValidationResource))
            );
        
        // Act

        var record = new LocalizationResourceRecord(
            validationResource,
            await localizer.GetSupportedCulturesAsync()
        );
        
        // Assert
        
        record.Name.ShouldBe(validationResource.ResourceName);
        record.SupportedCultures.ShouldNotBeNull();
        record.DefaultCulture.ShouldBe(validationResource.DefaultCultureName);
        
        record.SupportedCultures.ShouldNotBeNullOrWhiteSpace();
        
        if (validationResource.BaseResourceNames.Any())
        {
            record.BaseResources.ShouldBe(validationResource.BaseResourceNames.JoinAsString(","));
        }
        else
        {
            record.BaseResources.ShouldBeNull();
        }
    }
}