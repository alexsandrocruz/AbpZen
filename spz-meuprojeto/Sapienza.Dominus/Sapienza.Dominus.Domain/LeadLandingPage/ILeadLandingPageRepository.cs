using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadLandingPage;

public interface ILeadLandingPageRepository : IRepository<Dominus.LeadLandingPage.LeadLandingPage, Guid>
{
}
