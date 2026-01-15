using System;
using System.Threading.Tasks;
using Sapienza.Dominus.TaskComment.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.TaskComment;

public interface ITaskCommentAppService :
    ICrudAppService<
        TaskCommentDto,
        Guid,
        TaskCommentGetListInput,
        CreateUpdateTaskCommentDto,
        CreateUpdateTaskCommentDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetTaskCommentLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
