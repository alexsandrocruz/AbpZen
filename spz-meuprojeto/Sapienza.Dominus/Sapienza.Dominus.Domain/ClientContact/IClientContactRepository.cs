using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientContact;

public interface IClientContactRepository : IRepository<Sapienza.Dominus.ClientContact.ClientContact, Guid>
{
}
