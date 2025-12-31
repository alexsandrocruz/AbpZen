using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.LanguageManagement.Pages.LanguageManagement;

public class CreateModel : LanguageManagementPageModel
{
    public List<SelectListItem> CultureSelectList { get; set; } = new List<SelectListItem>();

    public List<SelectListItem> UiCultureSelectList { get; set; } = new List<SelectListItem>();

    protected ILanguageAppService LanguageAppService { get; }

    [BindProperty]
    public LanguageCreateModalView Language { get; set; }

    public CreateModel(ILanguageAppService languageAppService)
    {
        LanguageAppService = languageAppService;
    }

    public virtual Task OnGetAsync()
    {
        Language = new LanguageCreateModalView();

        var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => c.DisplayName != CultureInfo.InvariantCulture.DisplayName);

        CultureSelectList = allCultures.Select(c => new SelectListItem(c.EnglishName, c.Name)).ToList();

        if (CultureSelectList.Any())
        {
            CultureSelectList.FirstOrDefault().Text = L["NotAssigned"].Value;
            CultureSelectList.FirstOrDefault().Value = "";
        }

        UiCultureSelectList = CultureSelectList;

        return Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var language = ObjectMapper.Map<LanguageCreateModalView, CreateLanguageDto>(Language);

        await LanguageAppService.CreateAsync(language);

        return NoContent();
    }

    public class LanguageCreateModalView : ExtensibleObject
    {
        [Required]
        [SelectItems(nameof(CultureSelectList))]
        public string CultureName { get; set; }

        [Required]
        [SelectItems(nameof(UiCultureSelectList))]
        public string UiCultureName { get; set; }
        
        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; } = true;
    }
}
