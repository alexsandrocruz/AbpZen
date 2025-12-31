using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public abstract class LocalizationResourceRecordRepository_Tests<TStartupModule> : LanguageManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    [Fact]
    public void Repository_Should_Be_Registered()
    {
        var repository = ServiceProvider.GetService<ILocalizationResourceRecordRepository>(); ; 
        repository.ShouldNotBeNull();
    }
}