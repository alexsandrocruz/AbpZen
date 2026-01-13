using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwFollows;

public class EfflwFollowsRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.flwFollows.flwFollows, Guid>, 
      IflwFollowsRepository
{
    public EfflwFollowsRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
