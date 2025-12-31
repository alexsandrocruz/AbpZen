using Volo.CmsKit.Pro.Faqs;
using Xunit;

namespace Volo.CmsKit.Pro.MongoDB.Faqs;

[Collection(MongoTestCollection.Name)]
public class FaqRepositoryTests : FaqRepository_Tests<CmsKitProMongoDbTestModule>
{
}
