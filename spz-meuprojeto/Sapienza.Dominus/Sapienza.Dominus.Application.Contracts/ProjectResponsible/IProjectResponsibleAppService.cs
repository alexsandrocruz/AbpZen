using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProjectResponsible.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProjectResponsible;

public interface IProjectResponsibleAppService :
    ICrudAppService<
        ProjectResponsibleDto,
        Guid,
        ProjectResponsibleGetListInput,
        CreateUpdateProjectResponsibleDto,
        CreateUpdateProjectResponsibleDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProjectResponsibleLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
