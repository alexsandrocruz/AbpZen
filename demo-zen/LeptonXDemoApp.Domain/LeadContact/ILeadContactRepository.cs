using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.LeadContact;

public interface ILeadContactRepository : IRepository<LeptonXDemoApp.LeadContact.LeadContact, Guid>
{
}
