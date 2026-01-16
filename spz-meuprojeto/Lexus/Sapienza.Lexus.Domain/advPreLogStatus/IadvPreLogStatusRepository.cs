using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreLogStatus;

public interface IadvPreLogStatusRepository : IRepository<Sapienza.Lexus.advPreLogStatus.advPreLogStatus, Guid>
{
}
