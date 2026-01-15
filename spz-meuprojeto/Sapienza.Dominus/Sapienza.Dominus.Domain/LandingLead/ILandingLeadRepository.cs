using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LandingLead;

public interface ILandingLeadRepository : IRepository<Dominus.LandingLead.LandingLead, Guid>
{
}
