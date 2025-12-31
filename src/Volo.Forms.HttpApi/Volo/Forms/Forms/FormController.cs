using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Forms.Localization;
using Volo.Forms.Questions;
using Volo.Forms.Responses;

namespace Volo.Forms.Forms;

[RemoteService(Name = FormsRemoteServiceConsts.RemoteServiceName)]
[Area(FormsRemoteServiceConsts.ModuleName)]
[ControllerName("Form")]
[Route("api/forms")]
public class FormController : AbpControllerBase, IFormAppService
{
    protected IFormAppService FormAppService { get; }

    public FormController(IFormAppService formAppService)
    {
        FormAppService = formAppService;
        LocalizationResource = typeof(FormsResource);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FormDto>> GetListAsync(GetFormListInputDto input)
    {
        return FormAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}/responses")]
    public Task<PagedResultDto<FormResponseDetailedDto>> GetResponsesAsync(Guid id, GetResponseListInputDto input)
    {
        return FormAppService.GetResponsesAsync(id, input);
    }

    [HttpGet]
    [Route("{id}/download-responses-csv")]
    public Task<IRemoteStreamContent> GetCsvResponsesAsync(Guid id, GetResponseListInputDto input)
    {
        return FormAppService.GetCsvResponsesAsync(id, input);
    }

    [HttpGet]
    [Route("{id}/responses-count")]
    public Task<long> GetResponsesCountAsync(Guid id)
    {
        return FormAppService.GetResponsesCountAsync(id);
    }

    [HttpDelete]
    [Route("{id}/responses")]
    public Task DeleteAllResponsesOfFormAsync(Guid id)
    {
        return FormAppService.DeleteAllResponsesOfFormAsync(id);
    }

    [HttpPost]
    [Route("/invite")]
    public Task SendInviteEmailAsync(FormInviteEmailInputDto input)
    {
        return FormAppService.SendInviteEmailAsync(input);
    }

    [HttpPost]
    public virtual Task<FormDto> CreateAsync(CreateFormDto input)
    {
        return FormAppService.CreateAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<FormWithDetailsDto> GetAsync(Guid id)
    {
        return FormAppService.GetAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<FormDto> UpdateAsync(Guid id, UpdateFormDto input)
    {
        return FormAppService.UpdateAsync(id, input);
    }

    [HttpPut]
    [Route("{id}/settings")]
    public virtual Task SetSettingsAsync(Guid id, UpdateFormSettingInputDto input)
    {
        return FormAppService.SetSettingsAsync(id, input);
    }

    [HttpGet]
    [Route("{id}/settings")]
    public virtual Task<FormSettingsDto> GetSettingsAsync(Guid id)
    {
        return FormAppService.GetSettingsAsync(id);
    }

    [HttpGet]
    [Route("{id}/questions")]
    public virtual Task<List<QuestionDto>> GetQuestionsAsync(Guid id, GetQuestionListDto input)
    {
        return FormAppService.GetQuestionsAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/questions")]
    public virtual Task<QuestionDto> CreateQuestionAsync(Guid id, CreateQuestionDto input)
    {
        return FormAppService.CreateQuestionAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return FormAppService.DeleteAsync(id);
    }
}
