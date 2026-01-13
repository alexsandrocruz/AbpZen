using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finProcuracoesRPV.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finProcuracoesRPV;

public interface IfinProcuracoesRPVAppService :
    ICrudAppService<
        finProcuracoesRPVDto,
        Guid,
        finProcuracoesRPVGetListInput,
        CreateUpdatefinProcuracoesRPVDto,
        CreateUpdatefinProcuracoesRPVDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinProcuracoesRPVLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
