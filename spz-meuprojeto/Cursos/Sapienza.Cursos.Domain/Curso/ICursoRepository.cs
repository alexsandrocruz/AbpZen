using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Curso;

public interface ICursoRepository : IRepository<Sapienza.Cursos.Curso.Curso, Guid>
{
}
