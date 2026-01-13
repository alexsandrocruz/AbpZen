using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientes_bkp.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientes_bkp;

public interface IadvClientes_bkpAppService :
    ICrudAppService<
        advClientes_bkpDto,
        Guid,
        advClientes_bkpGetListInput,
        CreateUpdateadvClientes_bkpDto,
        CreateUpdateadvClientes_bkpDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientes_bkpLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
