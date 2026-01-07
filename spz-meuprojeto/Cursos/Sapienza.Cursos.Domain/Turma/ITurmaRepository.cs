using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Turma;

public interface ITurmaRepository : IRepository<Sapienza.Cursos.Turma.Turma, Guid>
{
}
