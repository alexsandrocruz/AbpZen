using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Lead;

public interface ILeadRepository : IRepository<LeptonXDemoApp.Lead.Lead, Guid>
{
}
