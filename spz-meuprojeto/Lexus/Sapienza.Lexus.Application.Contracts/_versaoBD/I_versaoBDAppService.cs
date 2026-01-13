using System;
using System.Threading.Tasks;
using Sapienza.Lexus._versaoBD.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus._versaoBD;

public interface I_versaoBDAppService :
    ICrudAppService<
        _versaoBDDto,
        Guid,
        _versaoBDGetListInput,
        CreateUpdate_versaoBDDto,
        CreateUpdate_versaoBDDto>
{
    Task<ListResultDto<LookupDto<Guid>>> Get_versaoBDLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
