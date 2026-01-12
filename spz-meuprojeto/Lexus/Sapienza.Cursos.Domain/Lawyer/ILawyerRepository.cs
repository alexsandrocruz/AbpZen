using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Lawyer;

public interface ILawyerRepository : IRepository<Sapienza.Cursos.Lawyer.Lawyer, Guid>
{
}
