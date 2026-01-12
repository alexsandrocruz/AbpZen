using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Specialization.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Specialization;

public interface ISpecializationAppService :
    ICrudAppService<
        SpecializationDto,
        Guid,
        SpecializationGetListInput,
        CreateUpdateSpecializationDto,
        CreateUpdateSpecializationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSpecializationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
