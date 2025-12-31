using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.Newsletters;
using Xunit;

namespace Volo.CmsKit.Pro.Newsletters;

public abstract class NewsletterRecordRepository_Tests<TStartupModule> : CmsKitProTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly INewsletterRecordRepository _newsletterRecordRepository;

    public NewsletterRecordRepository_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _newsletterRecordRepository = GetRequiredService<INewsletterRecordRepository>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var newsletters = await _newsletterRecordRepository.GetListAsync();

        newsletters.ShouldNotBeNull();
        newsletters.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task FindByEmailAddressAsync()
    {
        var newsletterRecord = await _newsletterRecordRepository.FindByEmailAddressAsync(_cmsKitProTestData.Email);

        newsletterRecord.EmailAddress.ShouldBe(_cmsKitProTestData.Email);
        newsletterRecord.Preferences.Count.ShouldBeGreaterThan(0);
        newsletterRecord.Preferences.ShouldContain(x => x.Preference == _cmsKitProTestData.Preference);
    }

    [Fact]
    public async Task GetCountByFilterAsync()
    {
        var count = await _newsletterRecordRepository.GetCountByFilterAsync(
            _cmsKitProTestData.Preference,
            _cmsKitProTestData.Source);

        count.ShouldBeGreaterThan(0);
    }
}
