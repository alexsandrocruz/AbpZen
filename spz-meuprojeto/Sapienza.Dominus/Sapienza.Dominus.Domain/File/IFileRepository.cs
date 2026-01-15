using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.File;

public interface IFileRepository : IRepository<Sapienza.Dominus.File.File, Guid>
{
}
