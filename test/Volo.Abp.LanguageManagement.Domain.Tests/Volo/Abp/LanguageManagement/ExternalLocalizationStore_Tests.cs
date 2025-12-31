using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public class ExternalLocalizationStore_Tests : LanguageManagementDomainTestBase
{
    private readonly IExternalLocalizationStore _externalLocalizationStore;

    public ExternalLocalizationStore_Tests()
    {
        _externalLocalizationStore = ServiceProvider.GetRequiredService<IExternalLocalizationStore>();
    }

    [Fact]
    public async Task GetResourceOrNullAsync()
    {
        await AddResourcesAsync();
        var options = ServiceProvider.GetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
       
        (await _externalLocalizationStore.GetResourceOrNullAsync("NotExistingResourceName")).ShouldBeNull();
        (await _externalLocalizationStore.GetResourceOrNullAsync(options.Resources.First().Value.ResourceName)).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetResourcesAsync()
    {
        await AddResourcesAsync();
        (await _externalLocalizationStore.GetResourcesAsync()).Any().ShouldBeFalse();
    }

    [Fact]
    public async Task GetResourceNamesAsync()
    {
        await AddResourcesAsync();
        (await _externalLocalizationStore.GetResourceNamesAsync()).Any().ShouldBeFalse();
    }
    
    private async Task AddResourcesAsync()
    {
        await ServiceProvider.GetRequiredService<IExternalLocalizationSaver>().SaveAsync();
    }
}