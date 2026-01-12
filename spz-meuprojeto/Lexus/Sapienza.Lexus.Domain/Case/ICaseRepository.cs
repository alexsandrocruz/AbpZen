using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Case;

public interface ICaseRepository : IRepository<Sapienza.Lexus.Case.Case, Guid>
{
}
