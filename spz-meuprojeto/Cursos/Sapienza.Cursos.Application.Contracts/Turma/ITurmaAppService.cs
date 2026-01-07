using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Turma.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Turma;

public interface ITurmaAppService :
    ICrudAppService<
        TurmaDto,
        Guid,
        TurmaGetListInput,
        CreateUpdateTurmaDto,
        CreateUpdateTurmaDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetTurmaLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
