using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.LegalProcess;

public interface ILegalProcessRepository : IRepository<Sapienza.Cursos.LegalProcess.LegalProcess, Guid>
{
}
