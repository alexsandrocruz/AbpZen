using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advTarefas;

public class EfadvTarefasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advTarefas.advTarefas, Guid>, 
      IadvTarefasRepository
{
    public EfadvTarefasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
