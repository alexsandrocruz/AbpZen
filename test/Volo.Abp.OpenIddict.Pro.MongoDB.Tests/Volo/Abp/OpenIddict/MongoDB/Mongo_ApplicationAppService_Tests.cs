using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class Mongo_ApplicationAppService_Tests : ApplicationAppService_Tests<OpenIddictProMongoDbTestModule>
{

}
