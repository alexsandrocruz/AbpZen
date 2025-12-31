using System.Threading.Tasks;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Public.PageFeedbacks;
using Xunit;
using System.Linq;
using Shouldly;
using Volo.Abp.SettingManagement;

namespace Volo.CmsKit.Pro.PageFeedbacks;

public class PageFeedbackPublicAppService_Test : CmsKitProApplicationTestBase
{
    private readonly IPageFeedbackPublicAppService _pageFeedbackPublicAppService;
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly IPageFeedbackRepository _pageFeedbackRepository;
    private readonly ISettingManager _settingManager;

    public PageFeedbackPublicAppService_Test()
    {
        _pageFeedbackPublicAppService = GetRequiredService<IPageFeedbackPublicAppService>();
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _pageFeedbackRepository = GetRequiredService<IPageFeedbackRepository>();
        _settingManager = GetRequiredService<ISettingManager>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var input = new CreatePageFeedbackInput
        {
            EntityType = _cmsKitProTestData.EntityType1,
            EntityId = _cmsKitProTestData.EntityId1,
            Url = "http://localhost",
            IsUseful = true,
            UserNote = "User Note",
            FeedbackUserId = _cmsKitProTestData.PageFeedbackUserId
        };
        
        await _pageFeedbackPublicAppService.CreateAsync(input);

        var pageFeedbacks = await _pageFeedbackRepository.GetListAsync();
        
        pageFeedbacks.ShouldNotBeNull();
        pageFeedbacks.Count.ShouldBeGreaterThan(0);
        
        var pageFeedback = pageFeedbacks.Last();
        
        pageFeedback.ShouldNotBeNull();
        pageFeedback.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedback.EntityId.ShouldBe(_cmsKitProTestData.EntityId1);
        pageFeedback.Url.ShouldBe("http://localhost");
        pageFeedback.IsUseful.ShouldBe(true);
        pageFeedback.UserNote.ShouldBe("User Note");
        pageFeedback.IsHandled.ShouldBe(false);
        pageFeedback.AdminNote.ShouldBeNull();
        pageFeedback.FeedbackUserId.ShouldBe(_cmsKitProTestData.PageFeedbackUserId);
    }
    
    [Fact]
    public async Task CreateAsync_WithAutoHandle()
    {
        await _settingManager.SetForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.IsAutoHandled, true.ToString());
        
        var input = new CreatePageFeedbackInput
        {
            EntityType = _cmsKitProTestData.EntityType1,
            EntityId = _cmsKitProTestData.EntityId1,
            Url = "http://localhost",
            IsUseful = true,
            FeedbackUserId = _cmsKitProTestData.PageFeedbackUserId
        };
        
        var pageFeedbackDto = await _pageFeedbackPublicAppService.CreateAsync(input);

        var pageFeedback = await _pageFeedbackRepository.GetAsync(pageFeedbackDto.Id);
        
        pageFeedback.ShouldNotBeNull();
        pageFeedback.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedback.EntityId.ShouldBe(_cmsKitProTestData.EntityId1);
        pageFeedback.Url.ShouldBe("http://localhost");
        pageFeedback.IsUseful.ShouldBe(true);
        pageFeedback.IsHandled.ShouldBe(true);
        pageFeedback.AdminNote.ShouldBeNull();
        pageFeedback.UserNote.ShouldBeNull();
        pageFeedback.FeedbackUserId.ShouldBe(_cmsKitProTestData.PageFeedbackUserId);
    }
    
    [Fact]
    public async Task CreateAsync_WithRequireCommentsForNegativeFeedback()
    {
        await _settingManager.SetForCurrentTenantAsync(CmsKitProSettingNames.PageFeedback.RequireCommentsForNegativeFeedback, true.ToString());
        
        var input = new CreatePageFeedbackInput
        {
            EntityType = _cmsKitProTestData.EntityType1,
            EntityId = _cmsKitProTestData.EntityId1,
            Url = "http://localhost",
            IsUseful = false,
            FeedbackUserId = _cmsKitProTestData.PageFeedbackUserId
        };
        
        await Assert.ThrowsAsync<RequireCommentsForNegativeFeedbackException>(async () =>
        {
            await _pageFeedbackPublicAppService.CreateAsync(input);
        });
    }
}