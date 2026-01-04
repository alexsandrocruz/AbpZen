using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Aluno.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Aluno;

public interface IAlunoAppService :
    ICrudAppService<
        AlunoDto,
        Guid,
        AlunoGetListInput,
        CreateUpdateAlunoDto,
        CreateUpdateAlunoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetAlunoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
