using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadMessageTemplate;

public interface ILeadMessageTemplateRepository : IRepository<Dominus.LeadMessageTemplate.LeadMessageTemplate, Guid>
{
}
