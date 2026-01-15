using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProjectCommunication.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProjectCommunication;

public interface IProjectCommunicationAppService :
    ICrudAppService<
        ProjectCommunicationDto,
        Guid,
        ProjectCommunicationGetListInput,
        CreateUpdateProjectCommunicationDto,
        CreateUpdateProjectCommunicationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProjectCommunicationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
