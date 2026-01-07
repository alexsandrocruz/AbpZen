using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Aluno;

public interface IAlunoRepository : IRepository<Sapienza.Cursos.Aluno.Aluno, Guid>
{
}
