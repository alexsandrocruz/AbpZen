using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Lawyer;

public interface ILawyerRepository : IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid>
{
}
