using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public class ExternalLocalizationSaver_Tests : LanguageManagementDomainTestBase
{
    private readonly IExternalLocalizationSaver _externalLocalizationSaver;

    public ExternalLocalizationSaver_Tests()
    {
        _externalLocalizationSaver = GetRequiredService<IExternalLocalizationSaver>();
    }

    [Fact]
    public async Task Should_Save_All_Localization_Resources()
    {
        // Arrange
        
        var options = GetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
        var resourceRecordRepository = GetRequiredService<ILocalizationResourceRecordRepository>();
        var stringLocalizerFactory = GetRequiredService<IStringLocalizerFactory>();
        var resources = options.Resources.Values;

        // Act

        await _externalLocalizationSaver.SaveAsync();
        
        // Assert

        var recordsInRepository = await resourceRecordRepository.GetListAsync();

        recordsInRepository.Count.ShouldBe(resources.Count);

        var notSavedResources = resources
            .Where(r => recordsInRepository.All(x => x.Name != r.ResourceName))
            .ToList();
        
        notSavedResources.Count.ShouldBe(0);

        foreach (var resource in options.Resources.Values)
        {
            var resourceRecord = recordsInRepository.Single(x => x.Name == resource.ResourceName);
            resourceRecord.Name.ShouldBe(resource.ResourceName);
            resourceRecord.DefaultCulture.ShouldBe(resource.DefaultCultureName);
        }

        var textRecordRepository = GetRequiredService<ILocalizationTextRecordRepository>();
        foreach (var resource in resources)
        {
            var localizer = await stringLocalizerFactory.CreateByResourceNameAsync(resource.ResourceName);
            foreach (var cultureName in await localizer.GetSupportedCulturesAsync())
            {
                var textRecords = await textRecordRepository
                    .GetListAsync(
                        resource.ResourceName,
                        cultureName
                    );

                using (CultureHelper.Use(cultureName))
                {
                    foreach (var textRecord in textRecords)
                    {
                        localizer[textRecord.Value].Value.ShouldNotBeNull();
                    }
                }
            }
        }
    }
}