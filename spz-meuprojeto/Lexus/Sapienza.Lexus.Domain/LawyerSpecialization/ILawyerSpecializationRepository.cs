#nullable enable
using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.LawyerSpecialization;

public interface ILawyerSpecializationRepository : IRepository<Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization, Guid>
{
}
