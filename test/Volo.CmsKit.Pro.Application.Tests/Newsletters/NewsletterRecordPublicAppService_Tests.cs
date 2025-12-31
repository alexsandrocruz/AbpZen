using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Newsletters.Helpers;
using Volo.CmsKit.Public.Newsletters;
using Xunit;

namespace Volo.CmsKit.Pro.Newsletters;

public class NewsletterRecordPublicAppService_Tests : CmsKitProApplicationTestBase
{
    private readonly INewsletterRecordPublicAppService _newsletterRecordAppService;
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly INewsletterRecordAdminAppService _newsletterRecordAdminAppService;
    private readonly SecurityCodeProvider _securityCodeProvider;

    public NewsletterRecordPublicAppService_Tests()
    {
        _newsletterRecordAppService = GetRequiredService<INewsletterRecordPublicAppService>();
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _newsletterRecordAdminAppService = GetRequiredService<INewsletterRecordAdminAppService>();
        _securityCodeProvider = GetRequiredService<SecurityCodeProvider>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var newNewsletterRecord = new CreateNewsletterRecordInput
        {
            Preference = _cmsKitProTestData.Preference,
            Source = _cmsKitProTestData.Source,
            SourceUrl = "sourceUrl",
            EmailAddress = "info2@abp.io",
            PrivacyPolicyUrl = "privacy-policy-url",
            AdditionalPreferences = new List<string>() { "community", "blog" }
        };
        await _newsletterRecordAppService.CreateAsync(newNewsletterRecord);

        UsingDbContext(context =>
        {
            var newsletters = context.Set<NewsletterRecord>().ToList();

            newsletters
                .Any(c => c.EmailAddress == "info2@abp.io")
                .ShouldBeTrue();
        });
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowAbpValidationException_If_Email_Is_Invalid()
    {
        await Should.ThrowAsync<AbpValidationException>(async () =>
        {
            var newNewsletterRecord = new CreateNewsletterRecordInput
            {
                Preference = _cmsKitProTestData.Preference,
                Source = _cmsKitProTestData.Source,
                SourceUrl = "sourceUrl",
                EmailAddress = "<invalid-email>",
                PrivacyPolicyUrl = "privacy-policy-url",
                AdditionalPreferences = new List<string>() { "community", "blog" }
            };
        
            await _newsletterRecordAppService.CreateAsync(newNewsletterRecord);
        });
    }


    [Fact]
    public async Task UpdatePreferencesAsync()
    {
        await _newsletterRecordAppService.UpdatePreferencesAsync(new UpdatePreferenceRequestInput
        {
            EmailAddress = _cmsKitProTestData.Email,
            Source = "updatedSource",
            SourceUrl = "updatedSourceUrl",
            PreferenceDetails = new List<PreferenceDetailsDto>
                {
                    new PreferenceDetailsDto
                    {
                        Preference = "updatedPreference",
                        IsEnabled = true
                    }
                },
            SecurityCode = _securityCodeProvider.GetSecurityCode(_cmsKitProTestData.Email)
        });

        UsingDbContext(context =>
        {
            var newsletterPreferences = context.Set<NewsletterPreference>().ToList();

            newsletterPreferences
                .Any(x => x.Preference == "updatedPreference" && x.Source == "updatedSource" & x.SourceUrl == "updatedSourceUrl")
                .ShouldBeTrue();
        });
    }

    [Fact]
    public async Task GetNewsletterPreferencesAsync()
    {
        var newsletterPreferences = await _newsletterRecordAppService.GetNewsletterPreferencesAsync(_cmsKitProTestData.Email);

        newsletterPreferences.ShouldNotBeNull();
        newsletterPreferences.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetOptionByPreference()
    {
        var newsletterEmailOptions = await _newsletterRecordAppService.GetOptionByPreferenceAsync(_cmsKitProTestData.Preference);

        newsletterEmailOptions.PrivacyPolicyConfirmation.ShouldNotBeNull();
        newsletterEmailOptions.DisplayAdditionalPreferences.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetOptionByPreference_Should_Be_Null_PrivacyPolicy()
    {
        var newsletterEmailOptions = await _newsletterRecordAppService.GetOptionByPreferenceAsync("preference3");

        newsletterEmailOptions.PrivacyPolicyConfirmation.ShouldBeNull();
        newsletterEmailOptions.DisplayAdditionalPreferences.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetOptionByPreference_Should_Be_Null_AdditionalPreferences_Count()
    {
        var newsletterEmailOptions = await _newsletterRecordAppService.GetOptionByPreferenceAsync("preference2");

        newsletterEmailOptions.PrivacyPolicyConfirmation.ShouldBeNullOrEmpty();
        newsletterEmailOptions.DisplayAdditionalPreferences.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetOptionByPreference_Should_Be_Zero_AdditionalPreferences_Count()
    {
        var newsletterEmailOptions = await _newsletterRecordAppService.GetOptionByPreferenceAsync("blog");

        newsletterEmailOptions.DisplayAdditionalPreferences.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetAsync()
    {
        var newsletterRecord = await _newsletterRecordAdminAppService.GetAsync(_cmsKitProTestData.NewsletterEmailId);

        newsletterRecord.Id.ShouldBe(_cmsKitProTestData.NewsletterEmailId);
        newsletterRecord.EmailAddress.ShouldBe(_cmsKitProTestData.Email);
        newsletterRecord.Preferences.Count.ShouldBeGreaterThan(0);
        newsletterRecord.Preferences.ShouldContain(x => x.Preference == _cmsKitProTestData.Preference && x.Source == _cmsKitProTestData.Source);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var newsletterRecords = await _newsletterRecordAdminAppService.GetListAsync(
            new GetNewsletterRecordsRequestInput
            {
                Preference = _cmsKitProTestData.Preference,
                Source = _cmsKitProTestData.Source
            });

        newsletterRecords.TotalCount.ShouldBeGreaterThan(0);
        newsletterRecords.Items.Count.ShouldBeGreaterThan(0);
        newsletterRecords.Items.ShouldContain(x => x.EmailAddress == _cmsKitProTestData.Email);
    }

    [Fact]
    public async Task GetNewsletterRecordsCsvDetailAsync()
    {
        var newsletterRecordsCsvDetail = await _newsletterRecordAdminAppService.GetNewsletterRecordsCsvDetailAsync(new GetNewsletterRecordsCsvRequestInput
        {
            Preference = _cmsKitProTestData.Preference,
            Source = _cmsKitProTestData.Source
        });

        newsletterRecordsCsvDetail.Count.ShouldBeGreaterThan(0);
        newsletterRecordsCsvDetail.ShouldContain(x => x.EmailAddress == _cmsKitProTestData.Email);
    }
}
