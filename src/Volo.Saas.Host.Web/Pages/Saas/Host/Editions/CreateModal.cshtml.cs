using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Payment.Plans;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Pages.Saas.Host.Editions;

public class CreateModalModel : SaasHostPageModel
{
    [BindProperty]
    public EditionInfoModel Edition { get; set; }

    public List<SelectListItem> Plans { get; private set; }

    protected IEditionAppService EditionAppService { get; }

    public CreateModalModel(
        IEditionAppService editionAppService)
    {
        EditionAppService = editionAppService;
    }

    public virtual async Task OnGetAsync()
    {
        Edition = new EditionInfoModel();

        var plans = await EditionAppService.GetPlanLookupAsync();

        Plans = plans.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<EditionInfoModel, EditionCreateDto>(Edition);
        await EditionAppService.CreateAsync(input);

        return NoContent();
    }

    public class EditionInfoModel : ExtensibleObject
    {
        [Required]
        [Display(Name = "Display Name")]
        [DynamicStringLength(typeof(EditionConsts),nameof(EditionConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }

        [SelectItems(nameof(Plans))]
        public Guid? PlanId { get; set; }
    }
}
