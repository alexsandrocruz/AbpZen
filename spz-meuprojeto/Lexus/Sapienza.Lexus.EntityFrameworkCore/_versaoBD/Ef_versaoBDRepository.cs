using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus._versaoBD;

public class Ef_versaoBDRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus._versaoBD._versaoBD, Guid>, 
      I_versaoBDRepository
{
    public Ef_versaoBDRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
