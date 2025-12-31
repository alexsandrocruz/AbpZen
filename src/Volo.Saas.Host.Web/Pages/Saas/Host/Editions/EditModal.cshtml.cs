using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Payment.Plans;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Pages.Saas.Host.Editions;

public class EditModalModel : SaasHostPageModel
{
    [BindProperty]
    public EditionInfoModel Edition { get; set; }

    public List<SelectListItem> Plans { get; set; }

    protected IEditionAppService EditionAppService { get; }

    public EditModalModel(
        IEditionAppService editionAppService)
    {
        EditionAppService = editionAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var plans = await EditionAppService.GetPlanLookupAsync();

        Plans = plans.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList();

        Edition = ObjectMapper.Map<EditionDto, EditionInfoModel>(
            await EditionAppService.GetAsync(id)
        );
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<EditionInfoModel, EditionUpdateDto>(Edition);
        await EditionAppService.UpdateAsync(Edition.Id, input);

        return NoContent();
    }

    public class EditionInfoModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Required]
        [DynamicStringLength(typeof(EditionConsts),nameof(EditionConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }

        [SelectItems(nameof(Plans))]
        public Guid? PlanId { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
