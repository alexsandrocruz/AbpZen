using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadTag.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadTag;

public interface ILeadTagAppService :
    ICrudAppService<
        LeadTagDto,
        Guid,
        LeadTagGetListInput,
        CreateUpdateLeadTagDto,
        CreateUpdateLeadTagDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadTagLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
