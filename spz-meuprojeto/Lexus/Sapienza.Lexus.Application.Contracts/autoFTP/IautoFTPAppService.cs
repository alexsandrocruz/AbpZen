using System;
using System.Threading.Tasks;
using Sapienza.Lexus.autoFTP.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.autoFTP;

public interface IautoFTPAppService :
    ICrudAppService<
        autoFTPDto,
        Guid,
        autoFTPGetListInput,
        CreateUpdateautoFTPDto,
        CreateUpdateautoFTPDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetautoFTPLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
