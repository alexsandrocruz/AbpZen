using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Forms.Questions;

public interface IQuestionAppService : IApplicationService
{
    Task<List<QuestionDto>> GetListAsync(GetQuestionListDto input);

    Task<QuestionDto> GetAsync(Guid id);

    Task<QuestionDto> UpdateAsync(Guid id, UpdateQuestionDto input);

    Task DeleteAsync(Guid id);
}
