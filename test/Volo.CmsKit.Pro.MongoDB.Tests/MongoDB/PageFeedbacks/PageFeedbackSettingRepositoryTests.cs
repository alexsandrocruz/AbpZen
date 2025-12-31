using Volo.CmsKit.Pro.PageFeedbacks;
using Xunit;

namespace Volo.CmsKit.Pro.MongoDB.PageFeedbacks;

[Collection(MongoTestCollection.Name)]
public class PageFeedbackSettingRepositoryTests : PageFeedbackSettingRepository_Tests<CmsKitProMongoDbTestModule>
{
}