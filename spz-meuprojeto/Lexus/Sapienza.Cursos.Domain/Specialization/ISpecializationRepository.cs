using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Specialization;

public interface ISpecializationRepository : IRepository<Sapienza.Cursos.Specialization.Specialization, Guid>
{
}
