using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Specialization;

public interface ISpecializationRepository : IRepository<Sapienza.Lexus.Specialization.Specialization, Guid>
{
}
