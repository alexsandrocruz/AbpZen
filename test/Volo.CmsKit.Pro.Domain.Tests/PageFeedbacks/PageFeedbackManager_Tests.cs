using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Xunit;

namespace Volo.CmsKit.Pro.PageFeedbacks;

public class PageFeedbackManager_Tests : CmsKitProDomainTestBase
{
    private readonly CmsKitProTestData _cmsKitProTestData;
    private readonly PageFeedbackManager _pageFeedbackManager;

    public PageFeedbackManager_Tests()
    {
        _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        _pageFeedbackManager = GetRequiredService<PageFeedbackManager>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var pageFeedback = await _pageFeedbackManager.CreateAsync(
            _cmsKitProTestData.EntityType1,
            _cmsKitProTestData.EntityId1,
            "http://localhost",
            true,
            "User Note",
            _cmsKitProTestData.PageFeedbackUserId
        );

        pageFeedback.ShouldNotBeNull();
        pageFeedback.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedback.EntityId.ShouldBe(_cmsKitProTestData.EntityId1);
        pageFeedback.Url.ShouldBe("http://localhost");
        pageFeedback.IsUseful.ShouldBe(true);
        pageFeedback.UserNote.ShouldBe("User Note");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowCantHavePageFeedbackException_WithNotConfigureEntityType()
    {
        var entityType = "Not.Configured.EntityType";

        var exception = Should.Throw<EntityCantHavePageFeedbackException>(async () =>
        {
            await _pageFeedbackManager.CreateAsync(
                entityType,
                _cmsKitProTestData.EntityId1,
                "http://localhost",
                true,
                "User Note",
                _cmsKitProTestData.PageFeedbackUserId
            );
        });

        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task CreateSettingAsync()
    {
        var pageFeedbackSetting = await _pageFeedbackManager.CreateSettingForEntityTypeAsync(
            _cmsKitProTestData.EntityType1,
            new List<string> { _cmsKitProTestData.EmailAddresses, _cmsKitProTestData.EmailAddresses2 }.JoinAsString(PageFeedbackConst.EmailAddressesSeparator)
        );

        pageFeedbackSetting.ShouldNotBeNull();
        pageFeedbackSetting.EntityType.ShouldBe(_cmsKitProTestData.EntityType1);
        pageFeedbackSetting.EmailAddresses.ShouldBe(
            new List<string> { _cmsKitProTestData.EmailAddresses, _cmsKitProTestData.EmailAddresses2 }.JoinAsString(PageFeedbackConst.EmailAddressesSeparator));
    }

    [Fact]
    public async Task CreateSettingAsync_ShouldThrowCantHavePageFeedbackException_WithNotConfigureEntityType()
    {
        var entityType = "Not.Configured.EntityType";

        var exception = Should.Throw<EntityCantHavePageFeedbackException>(async () =>
        {
            await _pageFeedbackManager.CreateSettingForEntityTypeAsync(
                entityType,
                new List<string> { _cmsKitProTestData.EmailAddresses, _cmsKitProTestData.EmailAddresses2 }.JoinAsString(PageFeedbackConst.EmailAddressesSeparator)
            );
        });

        exception.ShouldNotBeNull();
    }
}