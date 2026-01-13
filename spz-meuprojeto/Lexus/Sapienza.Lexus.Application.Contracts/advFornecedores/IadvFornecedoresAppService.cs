using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advFornecedores.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advFornecedores;

public interface IadvFornecedoresAppService :
    ICrudAppService<
        advFornecedoresDto,
        Guid,
        advFornecedoresGetListInput,
        CreateUpdateadvFornecedoresDto,
        CreateUpdateadvFornecedoresDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvFornecedoresLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
