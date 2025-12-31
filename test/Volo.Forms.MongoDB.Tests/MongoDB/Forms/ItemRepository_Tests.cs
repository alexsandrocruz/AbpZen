using Volo.Forms.Forms;
using Xunit;

namespace Volo.Forms.MongoDB.Forms;

[Collection(MongoTestCollection.Name)]
public class ItemRepository_Tests : ItemRepository_Tests<FormsMongoDbTestModule>
{
}
