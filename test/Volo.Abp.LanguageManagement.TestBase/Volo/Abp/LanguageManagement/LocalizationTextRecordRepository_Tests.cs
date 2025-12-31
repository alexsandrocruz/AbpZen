using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.LanguageManagement.External;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public abstract class LocalizationTextRecordRepository_Tests<TStartupModule> : LanguageManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly ILocalizationTextRecordRepository _localizationTextRecordRepository;

    protected LocalizationTextRecordRepository_Tests()
    {
        _localizationTextRecordRepository = ServiceProvider.GetService<ILocalizationTextRecordRepository>();
    }

    [Fact]
    public void Repository_Should_Be_Registered()
    {
        _localizationTextRecordRepository.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetListAsync()
    {
        (await _localizationTextRecordRepository.GetListAsync("LanguageManagement", "tr")).Any().ShouldBeFalse();

        await _localizationTextRecordRepository.InsertAsync(new LocalizationTextRecord(Guid.NewGuid(), "LanguageManagement", "tr", "Tes"));

        (await _localizationTextRecordRepository.GetListAsync("LanguageManagement", "tr")).Any().ShouldBeTrue();
    }

    [Fact]
    public async Task FindAsync()
    {
        (await _localizationTextRecordRepository.FindAsync("LanguageManagement", "tr")).ShouldBeNull();

        await _localizationTextRecordRepository.InsertAsync(new LocalizationTextRecord(Guid.NewGuid(), "LanguageManagement", "tr", "Test"));

        var localizationTextRecord = await _localizationTextRecordRepository.FindAsync("LanguageManagement", "tr");
        localizationTextRecord.ShouldNotBeNull();
        localizationTextRecord.ResourceName.ShouldBe("LanguageManagement");
        localizationTextRecord.CultureName.ShouldBe("tr");
        localizationTextRecord.Value.ShouldBe("Test");
    }
}