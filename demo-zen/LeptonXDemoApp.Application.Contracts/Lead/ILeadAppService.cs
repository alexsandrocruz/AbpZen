using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Lead.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Lead;

public interface ILeadAppService :
    ICrudAppService<
        LeadDto,
        Guid,
        LeadGetListInput,
        CreateUpdateLeadDto,
        CreateUpdateLeadDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
