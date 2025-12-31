using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.PageFeedbacks;
using Xunit;

namespace Volo.CmsKit.Pro.PageFeedbacks;

public abstract class PageFeedbackSettingRepository_Tests<TStartupModule> : CmsKitProTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly IPageFeedbackSettingRepository _pageFeedbackSettingRepository;
    
    public PageFeedbackSettingRepository_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _pageFeedbackSettingRepository = GetRequiredService<IPageFeedbackSettingRepository>();
    }
    
    [Fact]
    public async Task FindByEntityTypeAsync()
    {
        var pageFeedbackSetting = await _pageFeedbackSettingRepository.FindByEntityTypeAsync(
            _cmsKitProTestData.EntityType1);
        
        pageFeedbackSetting.ShouldNotBeNull();
        pageFeedbackSetting.EmailAddresses.ShouldNotBeNull();
        pageFeedbackSetting.EmailAddresses.ShouldBe(new List<string> {_cmsKitProTestData.EmailAddresses, _cmsKitProTestData.EmailAddresses2}.JoinAsString(PageFeedbackConst.EmailAddressesSeparator));
        
        pageFeedbackSetting = await _pageFeedbackSettingRepository.FindByEntityTypeAsync(
            _cmsKitProTestData.EntityType2);
        
        pageFeedbackSetting.ShouldBeNull();
        
        pageFeedbackSetting = await _pageFeedbackSettingRepository.FindByEntityTypeAsync(PageFeedbackConst.DefaultSettingEntityType);
        
        pageFeedbackSetting.ShouldNotBeNull();
        pageFeedbackSetting.EmailAddresses.ShouldNotBeNull();
        pageFeedbackSetting.EmailAddresses.ShouldBe(_cmsKitProTestData.FallBackEmailAddresses.JoinAsString(PageFeedbackConst.EmailAddressesSeparator));
    }
}