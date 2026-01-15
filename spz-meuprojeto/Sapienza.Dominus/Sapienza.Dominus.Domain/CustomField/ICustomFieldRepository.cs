using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.CustomField;

public interface ICustomFieldRepository : IRepository<Sapienza.Dominus.CustomField.CustomField, Guid>
{
}
