using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadLandingPage;

public interface ILeadLandingPageRepository : IRepository<Sapienza.Dominus.LeadLandingPage.LeadLandingPage, Guid>
{
}
