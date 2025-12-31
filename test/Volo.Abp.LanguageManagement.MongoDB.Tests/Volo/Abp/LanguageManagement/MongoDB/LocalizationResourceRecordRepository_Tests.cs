using Xunit;

namespace Volo.Abp.LanguageManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class LocalizationResourceRecordRepository_Tests : LocalizationResourceRecordRepository_Tests<LanguageManagementMongoDbTestModule>
{
    
}