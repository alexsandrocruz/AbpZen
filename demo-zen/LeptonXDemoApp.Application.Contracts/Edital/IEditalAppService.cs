using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Edital.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Edital;

public interface IEditalAppService :
    ICrudAppService<
        EditalDto,
        Guid,
        EditalGetListInput,
        CreateUpdateEditalDto,
        CreateUpdateEditalDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetEditalLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
