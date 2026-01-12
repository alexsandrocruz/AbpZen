using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Lawyer.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Lawyer;

public interface ILawyerAppService :
    ICrudAppService<
        LawyerDto,
        Guid,
        LawyerGetListInput,
        CreateUpdateLawyerDto,
        CreateUpdateLawyerDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLawyerLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
