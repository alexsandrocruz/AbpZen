using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Project.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Project;

public interface IProjectAppService :
    ICrudAppService<
        ProjectDto,
        Guid,
        ProjectGetListInput,
        CreateUpdateProjectDto,
        CreateUpdateProjectDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProjectLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
