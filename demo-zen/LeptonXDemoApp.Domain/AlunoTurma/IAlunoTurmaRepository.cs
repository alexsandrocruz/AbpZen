using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.AlunoTurma;

public interface IAlunoTurmaRepository : IRepository<LeptonXDemoApp.AlunoTurma.AlunoTurma, Guid>
{
}
