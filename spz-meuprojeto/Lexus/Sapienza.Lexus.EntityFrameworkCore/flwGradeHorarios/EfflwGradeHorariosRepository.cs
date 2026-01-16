using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwGradeHorarios;

public class EfflwGradeHorariosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid>, 
      IflwGradeHorariosRepository
{
    public EfflwGradeHorariosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
