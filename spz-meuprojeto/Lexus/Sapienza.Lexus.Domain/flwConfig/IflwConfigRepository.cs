using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwConfig;

public interface IflwConfigRepository : IRepository<Sapienza.Lexus.flwConfig.flwConfig, Guid>
{
}
