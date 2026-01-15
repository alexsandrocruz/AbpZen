using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.CustomField;

public interface ICustomFieldRepository : IRepository<Dominus.CustomField.CustomField, Guid>
{
}
