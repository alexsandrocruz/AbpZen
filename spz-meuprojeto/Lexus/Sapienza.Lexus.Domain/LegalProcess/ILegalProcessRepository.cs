#nullable enable
using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.LegalProcess;

public interface ILegalProcessRepository : IRepository<Sapienza.Lexus.LegalProcess.LegalProcess, Guid>
{
}
