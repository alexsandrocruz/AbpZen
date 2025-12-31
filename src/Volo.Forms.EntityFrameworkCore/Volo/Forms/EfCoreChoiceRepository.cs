using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Forms.Choices;
using Volo.Forms.EntityFrameworkCore;

namespace Volo.Forms;

public class EfCoreChoiceRepository : EfCoreRepository<IFormsDbContext, Choice, Guid>, IChoiceRepository
{
    public EfCoreChoiceRepository(IDbContextProvider<IFormsDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
