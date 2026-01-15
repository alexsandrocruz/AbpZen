using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Lead;

public interface ILeadRepository : IRepository<Dominus.Lead.Lead, Guid>
{
}
