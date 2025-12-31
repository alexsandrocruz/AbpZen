using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.CmsKit.Newsletters;
using Xunit;

namespace Volo.CmsKit.Pro.Newsletters;

public class NewsletterRecordManager_Tests : CmsKitProDomainTestBase
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly NewsletterRecordManager _newsletterRecordManager;
    private readonly INewsletterRecordRepository _newsletterRecordRepository;

    public NewsletterRecordManager_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _newsletterRecordManager = GetRequiredService<NewsletterRecordManager>();
        _newsletterRecordRepository = GetRequiredService<INewsletterRecordRepository>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var newsletterRecord = await _newsletterRecordManager.CreateOrUpdateAsync(
            _cmsKitProTestData.Email,
            _cmsKitProTestData.Preference,
            _cmsKitProTestData.Source,
            "sourceUrl",
            new List<string>()
        );

        newsletterRecord.EmailAddress.ShouldBe(_cmsKitProTestData.Email);
        newsletterRecord.Preferences.Count.ShouldBeGreaterThan(0);
        newsletterRecord.Preferences.ShouldContain(x =>
            x.Preference == _cmsKitProTestData.Preference && x.Source == _cmsKitProTestData.Source);
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Create_If_EmailAndPreference_Exist()
    {
        var newsletterRecord = await _newsletterRecordManager.CreateOrUpdateAsync(
            _cmsKitProTestData.Email,
            _cmsKitProTestData.Preference,
            _cmsKitProTestData.Source,
            "sourceUrl",
            new List<string>()
        );

        var count = newsletterRecord.Preferences.Count;

        newsletterRecord = await _newsletterRecordManager.CreateOrUpdateAsync(
            _cmsKitProTestData.Email,
            _cmsKitProTestData.Preference,
            "newSource",
            "newSourceUrl",
            null
        );

        count.ShouldBe(newsletterRecord.Preferences.Count);
    }

    [Fact]
    public async Task CreateAsync_Should_Update_Preferences_If_Preference_Not_Exist()
    {
        var newsletterRecord = await _newsletterRecordRepository.FindByEmailAddressAsync(_cmsKitProTestData.Email);
        var count = newsletterRecord.Preferences.Count;

        newsletterRecord = await _newsletterRecordManager.CreateOrUpdateAsync(
            _cmsKitProTestData.Email,
            "newPreference4",
            "newSource",
            "newSourceUrl",
            new List<string>
            {
                    "additional-preference-1"
            }
        );

        newsletterRecord.ShouldNotBeNull();
        newsletterRecord.Preferences.Count.ShouldBeGreaterThan(0);
        newsletterRecord.Preferences.Count.ShouldBe(count + 2);
    }

    [Fact]
    public async Task GetNewsletterPreferencesAsync()
    {
        var newsletterPreferences = await _newsletterRecordManager.GetNewsletterPreferencesAsync();

        newsletterPreferences.ShouldNotBeNull();
        newsletterPreferences.Count.ShouldBeGreaterThan(0);
    }
}
