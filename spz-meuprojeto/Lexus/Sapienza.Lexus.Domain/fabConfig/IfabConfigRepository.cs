using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabConfig;

public interface IfabConfigRepository : IRepository<Sapienza.Lexus.fabConfig.fabConfig, Guid>
{
}
