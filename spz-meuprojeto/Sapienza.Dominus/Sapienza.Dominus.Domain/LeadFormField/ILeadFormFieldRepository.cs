using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormField;

public interface ILeadFormFieldRepository : IRepository<Sapienza.Dominus.LeadFormField.LeadFormField, Guid>
{
}
