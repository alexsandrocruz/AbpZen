using System;
using System.Threading.Tasks;
using Sapienza.Cursos.Client.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos.Client;

public interface IClientAppService :
    ICrudAppService<
        ClientDto,
        Guid,
        ClientGetListInput,
        CreateUpdateClientDto,
        CreateUpdateClientDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetClientLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
