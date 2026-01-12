using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Aluno;

public interface IAlunoRepository : IRepository<LeptonXDemoApp.Aluno.Aluno, Guid>
{
}
