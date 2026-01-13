using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliComoChegou.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliComoChegou;

public interface IadvCliComoChegouAppService :
    ICrudAppService<
        advCliComoChegouDto,
        Guid,
        advCliComoChegouGetListInput,
        CreateUpdateadvCliComoChegouDto,
        CreateUpdateadvCliComoChegouDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliComoChegouLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
