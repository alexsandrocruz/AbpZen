using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.LawyerSpecialization;

public interface ILawyerSpecializationRepository : IRepository<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, Guid>
{
}
