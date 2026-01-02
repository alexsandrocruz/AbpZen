
/**
 * Create Modal PageModel template
 */
export function getRazorCreateModalModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;
{%- for rel in relationships.asChild %}
using {{ project.namespace }}.{{ rel.parentEntityName }};
using {{ project.namespace }}.{{ rel.parentEntityName }}.Dtos;
{%- endfor %}

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class CreateModalModel : {{ project.name }}PageModel
{
    [BindProperty]
    public Create{{ entity.name }}ViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    {%- for rel in relationships.asChild %}
    public List<SelectListItem> {{ rel.parentEntityName }}List { get; set; } = new();
    {%- endfor %}

    private readonly I{{ entity.name }}AppService _{{ entity.name | camelCase }}AppService;
    {%- for rel in relationships.asChild %}
    private readonly I{{ rel.parentEntityName }}AppService _{{ rel.parentEntityName | camelCase }}AppService;
    {%- endfor %}

    public CreateModalModel(
        I{{ entity.name }}AppService {{ entity.name | camelCase }}AppService{% if relationships.asChild.size > 0 %},{% endif %}
        {%- for rel in relationships.asChild %}
        I{{ rel.parentEntityName }}AppService {{ rel.parentEntityName | camelCase }}AppService{% unless forloop.last %},{% endunless %}
        {%- endfor %}
    )
    {
        _{{ entity.name | camelCase }}AppService = {{ entity.name | camelCase }}AppService;
        {%- for rel in relationships.asChild %}
        _{{ rel.parentEntityName | camelCase }}AppService = {{ rel.parentEntityName | camelCase }}AppService;
        {%- endfor %}
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new Create{{ entity.name }}ViewModel();

        // Load lookup data for FK dropdowns
        {%- for rel in relationships.asChild %}
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
        {%- endfor %}
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Create{{ entity.name }}ViewModel, CreateUpdate{{ entity.name }}Dto>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.CreateAsync(dto);
        return NoContent();
    }
}
`;
}

/**
 * Edit Modal PageModel template
 */
export function getRazorEditModalModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;
{%- for rel in relationships.asChild %}
using {{ project.namespace }}.{{ rel.parentEntityName }};
using {{ project.namespace }}.{{ rel.parentEntityName }}.Dtos;
{%- endfor %}

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class EditModalModel : {{ project.name }}PageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public {{ entity.primaryKey }} Id { get; set; }

    [BindProperty]
    public Edit{{ entity.name }}ViewModel ViewModel { get; set; }

    // ========== Lookup Lists for FK Dropdowns ==========
    {%- for rel in relationships.asChild %}
    public List<SelectListItem> {{ rel.parentEntityName }}List { get; set; } = new();
    {%- endfor %}

    private readonly I{{ entity.name }}AppService _{{ entity.name | camelCase }}AppService;
    {%- for rel in relationships.asChild %}
    private readonly I{{ rel.parentEntityName }}AppService _{{ rel.parentEntityName | camelCase }}AppService;
    {%- endfor %}

    public EditModalModel(
        I{{ entity.name }}AppService {{ entity.name | camelCase }}AppService{% if relationships.asChild.size > 0 %},{% endif %}
        {%- for rel in relationships.asChild %}
        I{{ rel.parentEntityName }}AppService {{ rel.parentEntityName | camelCase }}AppService{% unless forloop.last %},{% endunless %}
        {%- endfor %}
    )
    {
        _{{ entity.name | camelCase }}AppService = {{ entity.name | camelCase }}AppService;
        {%- for rel in relationships.asChild %}
        _{{ rel.parentEntityName | camelCase }}AppService = {{ rel.parentEntityName | camelCase }}AppService;
        {%- endfor %}
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _{{ entity.name | camelCase }}AppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<{{ entity.name }}Dto, Edit{{ entity.name }}ViewModel>(dto);

        // Load lookup data for FK dropdowns
        {%- for rel in relationships.asChild %}
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
        {%- endfor %}
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Edit{{ entity.name }}ViewModel, CreateUpdate{{ entity.name }}Dto>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.UpdateAsync(Id, dto);
        return NoContent();
    }
}
`;
}

/**
 * Razor View template for Create Modal
 */
export function getRazorCreateModalViewTemplate(): string {
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
        <abp-modal-header title="@L["New{{ entity.name }}"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
`;
}

/**
 * Razor View template for Edit Modal
 */
export function getRazorEditModalViewTemplate(): string {
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
        <abp-modal-header title="@L["Edit{{ entity.name }}"].Value"></abp-modal-header>
        <abp-modal-body>
            <input type="hidden" name="Id" value="@Model.Id" />
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
`;
}
