using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class EditModalModel : AdminPageModel
{
    protected IPollAdminAppService PollAdminAppService { get; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public PollEditViewModel ViewModel { get; set; }

    public string NewOption { get; set; }

    public List<SelectListItem> Widgets { get; set; } = new();

    public EditModalModel(IPollAdminAppService pollAdminAppService)
    {
        PollAdminAppService = pollAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var dto = await PollAdminAppService.GetAsync(Id);

        Widgets = new List<SelectListItem>() { new("", "") };
        Widgets.AddRange((await PollAdminAppService.GetWidgetsAsync())
            .Items
            .Select(w => new SelectListItem(L[$"DisplayName:{w.Name}"].Value, w.Name))
            .ToList());

        ViewModel = ObjectMapper.Map<PollWithDetailsDto, PollEditViewModel>(dto);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var pollDto = ObjectMapper.Map<PollEditViewModel, UpdatePollDto>(ViewModel);
        var pollOptions = pollDto.PollOptions.Where(o => o.Text != null).ToList();
        pollDto.PollOptions = new Collection<PollOptionDto>(pollOptions);
        await PollAdminAppService.UpdateAsync(Id, pollDto);
        return NoContent();
    }

    public class PollEditViewModel : ExtensibleObject
    {
        [Required]
        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxQuestionLength))]
        public string Question { get; set; }

        [ReadOnlyInput]
        [Required]
        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxCodeLength))]
        [InputInfoText("Poll:CodeIsAUniqueKey")]
        public string Code { get; set; }

        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxNameLength))]
        [InputInfoText("Poll:NameIsUsedToFilterInAdminSide")]
        public string Name { get; set; }

        [SelectItems(nameof(Widgets))]
        public string Widget { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ResultShowingEndDate { get; set; }

        public bool ShowVoteCount { get; set; }

        public bool ShowResultWithoutGivingVote { get; set; }

        public bool ShowHoursLeft { get; set; }


        public PollOptionDto[] PollOptions { get; set; } = Array.Empty<PollOptionDto>();
    }
}
