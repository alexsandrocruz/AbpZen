using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class Mongo_ScopeAppService_Tests : ScopeAppService_Tests<OpenIddictProMongoDbTestModule>
{
    
}
