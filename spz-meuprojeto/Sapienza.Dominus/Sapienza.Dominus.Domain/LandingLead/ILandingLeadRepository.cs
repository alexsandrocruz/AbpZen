using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LandingLead;

public interface ILandingLeadRepository : IRepository<Sapienza.Dominus.LandingLead.LandingLead, Guid>
{
}
