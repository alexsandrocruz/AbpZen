using System.Threading.Tasks;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Public.PageFeedbacks;
using Xunit;
using System.Linq;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.PageFeedbacks;

namespace Volo.CmsKit.Pro.PageFeedbacks;

public class PageFeedbackAdminAppService_Test : CmsKitProApplicationTestBase
{
    private readonly IPageFeedbackAdminAppService _pageFeedbackAdminAppService;
    private readonly CmsKitProTestData _cmsKitProTestData;

    public PageFeedbackAdminAppService_Test()
    {
        _pageFeedbackAdminAppService = GetRequiredService<IPageFeedbackAdminAppService>();
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var pageFeedbacks = await _pageFeedbackAdminAppService.GetListAsync(new GetPageFeedbackListInput
        {
            EntityType = _cmsKitProTestData.EntityType1,
            EntityId = _cmsKitProTestData.EntityId1,
            IsHandled = false
        });

        pageFeedbacks.ShouldNotBeNull();
        pageFeedbacks.TotalCount.ShouldBeGreaterThan(0);
        
        var pageFeedback = pageFeedbacks.Items.Last();
        
        pageFeedback.ShouldNotBeNull();
        pageFeedback.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedback.EntityId.ShouldBe(_cmsKitProTestData.EntityId1);
        pageFeedback.Url.ShouldBe("http://localhost");
        pageFeedback.IsUseful.ShouldBe(true);
        pageFeedback.UserNote.ShouldBe("User Note");
        pageFeedback.IsHandled.ShouldBe(false);
        pageFeedback.AdminNote.ShouldBeNull();
    }
    
    [Fact]
    public async Task UpdateAsync()
    {
        var pageFeedbacks = await _pageFeedbackAdminAppService.GetListAsync(new GetPageFeedbackListInput());

        var pageFeedback = pageFeedbacks.Items.Last();
        
        await _pageFeedbackAdminAppService.UpdateAsync(pageFeedback.Id, new UpdatePageFeedbackDto
        {
            IsHandled = true,
            AdminNote = "Admin Note"
        });

        pageFeedback = await _pageFeedbackAdminAppService.GetAsync(pageFeedback.Id);
        
        pageFeedback.ShouldNotBeNull();
        pageFeedback.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedback.EntityId.ShouldBe(_cmsKitProTestData.EntityId1);
        pageFeedback.IsHandled.ShouldBe(true);
        pageFeedback.AdminNote.ShouldBe("Admin Note");
    }
    
    [Fact]
    public async Task DeleteAsync()
    {
        var pageFeedbacks = await _pageFeedbackAdminAppService.GetListAsync(new GetPageFeedbackListInput());
        var pageFeedback = pageFeedbacks.Items.Last();

        await _pageFeedbackAdminAppService.DeleteAsync(pageFeedback.Id);
        
        Should.Throw<EntityNotFoundException>(async () => await _pageFeedbackAdminAppService.GetAsync(pageFeedback.Id));
    }

}