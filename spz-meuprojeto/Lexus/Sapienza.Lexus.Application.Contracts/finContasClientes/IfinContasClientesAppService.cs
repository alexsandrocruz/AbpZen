using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finContasClientes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finContasClientes;

public interface IfinContasClientesAppService :
    ICrudAppService<
        finContasClientesDto,
        Guid,
        finContasClientesGetListInput,
        CreateUpdatefinContasClientesDto,
        CreateUpdatefinContasClientesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinContasClientesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
