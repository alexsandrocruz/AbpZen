using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Turma;

public interface ITurmaRepository : IRepository<LeptonXDemoApp.Turma.Turma, Guid>
{
}
