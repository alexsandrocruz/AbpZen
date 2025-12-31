using Xunit;

namespace Volo.Abp.LanguageManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class LocalizationTextRecordRepository_Tests : LocalizationTextRecordRepository_Tests<LanguageManagementMongoDbTestModule>
{
    
}