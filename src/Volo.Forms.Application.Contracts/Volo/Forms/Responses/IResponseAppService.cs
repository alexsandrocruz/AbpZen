using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Forms.Forms;
using Volo.Forms.Questions;

namespace Volo.Forms.Responses;

public interface IResponseAppService : IApplicationService
{
    public Task<FormResponseDto> GetAsync(Guid id);

    public Task<List<QuestionWithAnswersDto>> GetQuestionsWithAnswersAsync(Guid id);

    public Task<PagedResultDto<FormResponseDto>> GetListAsync(GetUserFormListInputDto input);

    public Task<FormDto> GetFormDetailsAsync(Guid formId);

    public Task<PagedResultDto<FormWithResponseDto>> GetUserFormsByUserIdAsync(Guid userId);

    public Task<FormResponseDto> SaveAnswersAsync(Guid formId, CreateResponseDto input);

    public Task<FormResponseDto> UpdateAnswersAsync(Guid id, UpdateResponseDto input);

    public Task DeleteAsync(Guid id);
}
