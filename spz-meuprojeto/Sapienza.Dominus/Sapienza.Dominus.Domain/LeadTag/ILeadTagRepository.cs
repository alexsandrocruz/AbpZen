using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadTag;

public interface ILeadTagRepository : IRepository<Dominus.LeadTag.LeadTag, Guid>
{
}
