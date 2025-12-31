using Volo.CmsKit.Pro.UrlShorting;
using Xunit;

namespace Volo.CmsKit.Pro.MongoDB.UrlShorting;

[Collection(MongoTestCollection.Name)]
public class ShortenedUrlRepository_Tests : ShortenedUrlRepository_Tests<CmsKitProMongoDbTestModule>
{
}
