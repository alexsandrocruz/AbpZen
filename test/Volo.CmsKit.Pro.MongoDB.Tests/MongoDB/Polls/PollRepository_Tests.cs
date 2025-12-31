using Volo.CmsKit.Pro.Polls;
using Xunit;

namespace Volo.CmsKit.Pro.MongoDB.Polls;
[Collection(MongoTestCollection.Name)]
public class PollRepository_Tests : PollRepository_Tests<CmsKitProMongoDbTestModule>
{
}
