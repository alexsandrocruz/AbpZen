using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwGradeHorarios;

public class EfflwGradeHorariosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid>, 
      IflwGradeHorariosRepository
{
    public EfflwGradeHorariosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
