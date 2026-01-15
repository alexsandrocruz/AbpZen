using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Comment.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Comment;

public interface ICommentAppService :
    ICrudAppService<
        CommentDto,
        Guid,
        CommentGetListInput,
        CreateUpdateCommentDto,
        CreateUpdateCommentDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCommentLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
