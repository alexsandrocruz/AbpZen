using System;
using System.Threading.Tasks;
using Sapienza.Lexus.flwGradeHorarios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.flwGradeHorarios;

public interface IflwGradeHorariosAppService :
    ICrudAppService<
        flwGradeHorariosDto,
        Guid,
        flwGradeHorariosGetListInput,
        CreateUpdateflwGradeHorariosDto,
        CreateUpdateflwGradeHorariosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetflwGradeHorariosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
