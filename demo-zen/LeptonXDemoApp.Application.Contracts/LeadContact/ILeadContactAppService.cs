using System;
using System.Threading.Tasks;
using LeptonXDemoApp.LeadContact.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.LeadContact;

public interface ILeadContactAppService :
    ICrudAppService<
        LeadContactDto,
        Guid,
        LeadContactGetListInput,
        CreateUpdateLeadContactDto,
        CreateUpdateLeadContactDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadContactLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
