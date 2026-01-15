using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientContact;

public interface IClientContactRepository : IRepository<Dominus.ClientContact.ClientContact, Guid>
{
}
