using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormField;

public interface ILeadFormFieldRepository : IRepository<Dominus.LeadFormField.LeadFormField, Guid>
{
}
