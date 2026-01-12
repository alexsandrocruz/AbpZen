using System;
using System.Threading.Tasks;
using Sapienza.Cursos.LawyerSpecialization.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.LawyerSpecialization;

public interface ILawyerSpecializationAppService :
    ICrudAppService<
        LawyerSpecializationDto,
        Guid,
        LawyerSpecializationGetListInput,
        CreateUpdateLawyerSpecializationDto,
        CreateUpdateLawyerSpecializationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLawyerSpecializationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
