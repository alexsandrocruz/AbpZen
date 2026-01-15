using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadForm;

public interface ILeadFormRepository : IRepository<Sapienza.Dominus.LeadForm.LeadForm, Guid>
{
}
