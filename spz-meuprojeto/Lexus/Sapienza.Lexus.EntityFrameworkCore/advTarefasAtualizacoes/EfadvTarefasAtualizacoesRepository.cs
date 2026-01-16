using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advTarefasAtualizacoes;

public class EfadvTarefasAtualizacoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, Guid>, 
      IadvTarefasAtualizacoesRepository
{
    public EfadvTarefasAtualizacoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
