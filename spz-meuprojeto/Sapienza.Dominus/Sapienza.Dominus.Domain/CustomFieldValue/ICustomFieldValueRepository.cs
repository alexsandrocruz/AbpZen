using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.CustomFieldValue;

public interface ICustomFieldValueRepository : IRepository<Dominus.CustomFieldValue.CustomFieldValue, Guid>
{
}
