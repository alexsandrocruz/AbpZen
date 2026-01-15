using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ProjectFollower.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ProjectFollower;

public interface IProjectFollowerAppService :
    ICrudAppService<
        ProjectFollowerDto,
        Guid,
        ProjectFollowerGetListInput,
        CreateUpdateProjectFollowerDto,
        CreateUpdateProjectFollowerDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProjectFollowerLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
