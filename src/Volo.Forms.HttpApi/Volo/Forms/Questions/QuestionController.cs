using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Forms.Localization;

namespace Volo.Forms.Questions;

[RemoteService(Name = FormsRemoteServiceConsts.RemoteServiceName)]
[Area(FormsRemoteServiceConsts.ModuleName)]
[ControllerName("Form")]
[Route("api/questions")]
public class QuestionController : AbpControllerBase, IQuestionAppService
{
    protected IQuestionAppService QuestionAppService { get; }

    public QuestionController(IQuestionAppService questionAppService)
    {
        QuestionAppService = questionAppService;
        LocalizationResource = typeof(FormsResource);
    }

    [HttpGet]
    public virtual Task<List<QuestionDto>> GetListAsync(GetQuestionListDto input)
    {
        return QuestionAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<QuestionDto> GetAsync(Guid id)
    {
        return QuestionAppService.GetAsync(id);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<QuestionDto> UpdateAsync(Guid id, UpdateQuestionDto input)
    {
        return QuestionAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return QuestionAppService.DeleteAsync(id);
    }
}
