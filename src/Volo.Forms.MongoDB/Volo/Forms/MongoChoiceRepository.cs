using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Forms.Choices;
using Volo.Forms.MongoDB;

namespace Volo.Forms;

public class MongoChoiceRepository : MongoDbRepository<IFormsMongoDbContext, Choice, Guid>, IChoiceRepository
{
    public MongoChoiceRepository(IMongoDbContextProvider<IFormsMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
}
