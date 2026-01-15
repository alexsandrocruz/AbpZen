using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ClientContact.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ClientContact;

public interface IClientContactAppService :
    ICrudAppService<
        ClientContactDto,
        Guid,
        ClientContactGetListInput,
        CreateUpdateClientContactDto,
        CreateUpdateClientContactDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetClientContactLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
