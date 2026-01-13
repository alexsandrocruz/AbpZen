using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finCentrosCusto.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finCentrosCusto;

public interface IfinCentrosCustoAppService :
    ICrudAppService<
        finCentrosCustoDto,
        Guid,
        finCentrosCustoGetListInput,
        CreateUpdatefinCentrosCustoDto,
        CreateUpdatefinCentrosCustoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinCentrosCustoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
