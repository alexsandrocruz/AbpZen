using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreCheckListsGrupos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreCheckListsGrupos;

public interface IadvPreCheckListsGruposAppService :
    ICrudAppService<
        advPreCheckListsGruposDto,
        Guid,
        advPreCheckListsGruposGetListInput,
        CreateUpdateadvPreCheckListsGruposDto,
        CreateUpdateadvPreCheckListsGruposDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreCheckListsGruposLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
