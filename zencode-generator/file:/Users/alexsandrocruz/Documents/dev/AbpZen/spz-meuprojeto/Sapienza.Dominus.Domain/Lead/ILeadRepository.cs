using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Lead;

public interface ILeadRepository : IRepository<Sapienza.Dominus.Lead.Lead, Guid>
{
}
