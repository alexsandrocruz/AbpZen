using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Entities;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.LanguageManagement.Pages.LanguageManagement;

public class EditModel : LanguageManagementPageModel
{
    protected ILanguageAppService LanguageAppService { get; }

    [BindProperty]
    public LanguageEditModalView Language { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public EditModel(ILanguageAppService languageAppService)
    {
        LanguageAppService = languageAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var language = await LanguageAppService.GetAsync(Id);
        Language = ObjectMapper.Map<LanguageDto, LanguageEditModalView>(language);
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        var language = ObjectMapper.Map<LanguageEditModalView, UpdateLanguageDto>(Language);

        await LanguageAppService.UpdateAsync(Language.Id, language);

        return NoContent();
    }

    public class LanguageEditModalView : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
