using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadTag;

public interface ILeadTagRepository : IRepository<Sapienza.Dominus.LeadTag.LeadTag, Guid>
{
}
