using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.LegalProcess;

public class EfLegalProcessRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.LegalProcess.LegalProcess, Guid>, 
      ILegalProcessRepository
{
    public EfLegalProcessRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
