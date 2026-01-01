/**
 * Razor CreateModal template for ABP Web layer
 */
export function getRazorCreateModalTemplate(): string {
    return `@page
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;
@using {{ project.namespace }}.Localization
@inject IHtmlLocalizer<{{ project.name }}Resource> L
@model {{ project.namespace }}.Web.Pages.{{ entity.name }}.CreateModalModel
@{
    Layout = null;
}
<abp-dynamic-form abp-model="ViewModel" data-ajaxForm="true" asp-page="CreateModal">
    <abp-modal>
        <abp-modal-header title="@L[\\"New{{ entity.name }}\\"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
`;
}

/**
 * Razor CreateModal PageModel template
 */
export function getRazorCreateModalModelTemplate(): string {
    return `using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class CreateModalModel : {{ project.name }}PageModel
{
    [BindProperty]
    public Create{{ entity.name }}ViewModel ViewModel { get; set; }

    private readonly I{{ entity.name }}AppService _{{ entity.name | camelCase }}AppService;

    public CreateModalModel(I{{ entity.name }}AppService {{ entity.name | camelCase }}AppService)
    {
        _{{ entity.name | camelCase }}AppService = {{ entity.name | camelCase }}AppService;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new Create{{ entity.name }}ViewModel();
        await Task.CompletedTask;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Create{{ entity.name }}ViewModel, {{ dto.createTypeName }}>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.CreateAsync(dto);
        return NoContent();
    }
}
`;
}

/**
 * Razor EditModal template for ABP Web layer
 */
export function getRazorEditModalTemplate(): string {
    return `@page
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;
@using {{ project.namespace }}.Localization
@inject IHtmlLocalizer<{{ project.name }}Resource> L
@model {{ project.namespace }}.Web.Pages.{{ entity.name }}.EditModalModel
@{
    Layout = null;
}
<abp-dynamic-form abp-model="ViewModel" data-ajaxForm="true" asp-page="EditModal">
    <abp-modal>
        <abp-modal-header title="@L[\\"Edit{{ entity.name }}\\"].Value"></abp-modal-header>
        <abp-modal-body>
            <input type="hidden" name="Id" value="@Model.Id" />
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
`;
}

/**
 * Razor EditModal PageModel template
 */
export function getRazorEditModalModelTemplate(): string {
    return `using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class EditModalModel : {{ project.name }}PageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public Edit{{ entity.name }}ViewModel ViewModel { get; set; }

    private readonly I{{ entity.name }}AppService _{{ entity.name | camelCase }}AppService;

    public EditModalModel(I{{ entity.name }}AppService {{ entity.name | camelCase }}AppService)
    {
        _{{ entity.name | camelCase }}AppService = {{ entity.name | camelCase }}AppService;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _{{ entity.name | camelCase }}AppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<{{ dto.readTypeName }}, Edit{{ entity.name }}ViewModel>(dto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Edit{{ entity.name }}ViewModel, {{ dto.updateTypeName }}>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
`;
}
