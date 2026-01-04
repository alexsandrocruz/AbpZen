using System;
using System.Threading.Tasks;
using LeptonXDemoApp.AlunoTurma.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.AlunoTurma;

public interface IAlunoTurmaAppService :
    ICrudAppService<
        AlunoTurmaDto,
        Guid,
        AlunoTurmaGetListInput,
        CreateUpdateAlunoTurmaDto,
        CreateUpdateAlunoTurmaDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetAlunoTurmaLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
