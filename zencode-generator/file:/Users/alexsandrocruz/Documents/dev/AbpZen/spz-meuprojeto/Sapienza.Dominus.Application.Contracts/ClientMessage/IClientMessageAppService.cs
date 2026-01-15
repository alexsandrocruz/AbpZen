using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ClientMessage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ClientMessage;

public interface IClientMessageAppService :
    ICrudAppService<
        ClientMessageDto,
        Guid,
        ClientMessageGetListInput,
        CreateUpdateClientMessageDto,
        CreateUpdateClientMessageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetClientMessageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
