using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Xunit;

namespace Volo.Abp.LanguageManagement;

public class LanguageAppService_Tests : LanguageManagementApplicationTestBase
{
    private readonly ILanguageAppService _languageAppService;
    private readonly LanguageManager _languageManager;
    private readonly ILanguageRepository _languageRepository;
    private readonly ISettingManager _settingManager;

    public LanguageAppService_Tests()
    {
        _languageAppService = GetRequiredService<ILanguageAppService>();
        _languageManager = GetRequiredService<LanguageManager>();
        _languageRepository = GetRequiredService<ILanguageRepository>();
        _settingManager = GetRequiredService<ISettingManager>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateAsync(bool isEnabled)
    {
        var output = await _languageAppService.CreateAsync(
            new CreateLanguageDto
            {
                CultureName = "ko",
                UiCultureName = "ko",
                IsEnabled = isEnabled,
                DisplayName = "Korean"
            }
        );

        output.CultureName.ShouldBe("ko");
        output.UiCultureName.ShouldBe("ko");
        output.IsEnabled.ShouldBe(isEnabled);
        output.DisplayName.ShouldBe("Korean");
        output.Id.ShouldNotBe(Guid.Empty);

        var language = await _languageRepository.GetAsync(output.Id);
        language.CultureName.ShouldBe("ko");
        language.UiCultureName.ShouldBe("ko");
        language.IsEnabled.ShouldBe(isEnabled);
        language.DisplayName.ShouldBe("Korean");
    }

    [Fact]
    public async Task GetListAsync()
    {
        // Arrange

        // Act
        var list1 = await _languageAppService.GetAllListAsync();

        // Assert
        var count = await _languageRepository.GetCountAsync();

        list1.Items.Count.ShouldBe((int)count);
    }

    [Fact]
    public async Task GetListAsync_OrderBy_()
    {
        // make sure creation time is different
        await Task.Delay(200);

        // Arrange
        await _languageAppService.CreateAsync(
            new CreateLanguageDto
            {
                CultureName = "zh",
                UiCultureName = "zh",
                IsEnabled = true,
                DisplayName = "zh"
            }
        );

        // Act
        var list1 = await _languageAppService.GetAllListAsync();

        // Assert
        list1.Items.First().CultureName.ShouldBe("zh");
    }

    [Fact]
    public async Task GetAsync()
    {
        // Arrange
        var cultureName = "cultureName";
        var entity = await _languageManager.CreateAsync(cultureName);
        await _languageRepository.InsertAsync(entity);

        // Act
        var language = await _languageAppService.GetAsync(entity.Id);

        // Assert
        language.CultureName.ShouldBe(cultureName);
    }

    [Fact]
    public async Task SetAsDefaultAsync()
    {
        // Arrange
        var aLanguage = (await _languageRepository.GetListAsync()).First(x => x.CultureName == "tr");

        // Act
        await _languageAppService.SetAsDefaultAsync(aLanguage.Id);

        // Assert
        var settingValue = await _settingManager.GetOrNullForCurrentTenantAsync(LocalizationSettingNames.DefaultLanguage);
        var (cultureName, uiCultureName) = LocalizationSettingHelper.ParseLanguageSetting(settingValue);

        cultureName.ShouldBe(aLanguage.CultureName);
        uiCultureName.ShouldBe(aLanguage.UiCultureName);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        // Arrange
        var languageToUpdate = (await _languageRepository.GetListAsync()).FirstOrDefault();

        // Act
        var updatedLanguage = await _languageAppService.UpdateAsync(languageToUpdate.Id, new UpdateLanguageDto()
        {
            DisplayName = languageToUpdate.DisplayName + " updated",
            IsEnabled = languageToUpdate.IsEnabled
        });

        // Assert
        updatedLanguage.DisplayName.ShouldBe(languageToUpdate.DisplayName + " updated");
    }


    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        var languageToDelete = (await _languageRepository.GetListAsync()).FirstOrDefault();

        // Act
        languageToDelete.ShouldNotBeNull();

        await _languageAppService.DeleteAsync(languageToDelete.Id);

        (await _languageRepository.FindAsync(languageToDelete.Id)).ShouldBeNull();
    }
}
