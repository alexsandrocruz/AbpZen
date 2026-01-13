using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fdtDevs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fdtDevs;

public interface IfdtDevsAppService :
    ICrudAppService<
        fdtDevsDto,
        Guid,
        fdtDevsGetListInput,
        CreateUpdatefdtDevsDto,
        CreateUpdatefdtDevsDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfdtDevsLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
