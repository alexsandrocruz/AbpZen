using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Curso.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Curso;

public interface ICursoAppService :
    ICrudAppService<
        CursoDto,
        Guid,
        CursoGetListInput,
        CreateUpdateCursoDto,
        CreateUpdateCursoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCursoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
