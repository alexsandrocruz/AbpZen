using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Edital;

public interface IEditalRepository : IRepository<LeptonXDemoApp.Edital.Edital, Guid>
{
}
