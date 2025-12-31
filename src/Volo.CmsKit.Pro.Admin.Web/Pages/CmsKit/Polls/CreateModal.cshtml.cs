using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Polls;
using Volo.CmsKit.Polls;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class CreateModalModel : AdminPageModel
{
    protected IPollAdminAppService PollAdminAppService { get; }

    [BindProperty]
    public CreatePollViewModel ViewModel { get; set; }

    public string NewOption { get; set; }

    public List<SelectListItem> Widgets { get; set; } = new();

    public CreateModalModel(IPollAdminAppService pollAdminAppService)
    {
        PollAdminAppService = pollAdminAppService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var pollDto = ObjectMapper.Map<CreatePollViewModel, CreatePollDto>(ViewModel);
        var pollOptions = pollDto.PollOptions.Where(o => o.Text != null).ToList();
        pollDto.PollOptions = new Collection<PollOptionDto>(pollOptions);

        var created = await PollAdminAppService.CreateAsync(pollDto);

        return new OkObjectResult(created);
    }

    public async Task OnGetAsync()
    {
        ViewModel = new CreatePollViewModel();

        Widgets = new List<SelectListItem>() { new("", "") };
        Widgets.AddRange((await PollAdminAppService.GetWidgetsAsync())
            .Items
            .Select(w => new SelectListItem(L[$"DisplayName:{w.Name}"].Value, w.Name))
            .ToList());
    }
    
    public class CreatePollViewModel : ExtensibleObject
    {
        [Required]
        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxQuestionLength))]
        public string Question { get; set; }

        [ReadOnlyInput]
        [Required]
        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxCodeLength))]
        [InputInfoText("Poll:CodeIsAUniqueKey")]
        public string Code { get; set; } = Path.GetRandomFileName().Replace(".", string.Empty).Substring(0, 8);

        [DynamicMaxLength(typeof(PollConst), nameof(PollConst.MaxNameLength))]
        [InputInfoText("Poll:NameIsUsedToFilterInAdminSide")]
        public string Name { get; set; }

        [SelectItems(nameof(Widgets))]
        public string Widget { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        public DateTime? EndDate { get; set; }

        public DateTime? ResultShowingEndDate { get; set; }

        public bool AllowMultipleVote { get; set; }

        public bool ShowVoteCount { get; set; } = true;

        public bool ShowResultWithoutGivingVote { get; set; }

        public bool ShowHoursLeft { get; set; }


        public PollOptionDto[] PollOptions { get; set; } = Array.Empty<PollOptionDto>();
    }
}
