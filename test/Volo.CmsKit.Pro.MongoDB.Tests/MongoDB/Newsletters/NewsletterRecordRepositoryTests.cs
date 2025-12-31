using Volo.CmsKit.Pro.Newsletters;
using Xunit;

namespace Volo.CmsKit.Pro.MongoDB.Newsletters;

[Collection(MongoTestCollection.Name)]
public class NewsletterRecordRepositoryTests : NewsletterRecordRepository_Tests<CmsKitProMongoDbTestModule>
{
}
