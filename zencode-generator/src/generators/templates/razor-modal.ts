
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
        {%- if rel.lookupMode != 'modal' %}
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
        {%- endif %}
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
        {%- if rel.lookupMode == 'modal' %}
        if (ViewModel.{{ rel.fkFieldName }} != null)
        {
            var {{ rel.parentEntityName | camelCase }} = await _{{ rel.parentEntityName | camelCase }}AppService.GetAsync(ViewModel.{{ rel.fkFieldName }}.Value);
            ViewModel.{{ rel.parentEntityName }}DisplayName = {{ rel.parentEntityName | camelCase }}.{{ rel.displayField }};
        }
        {%- else %}
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
        {%- endif %}
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
    {%- assign tabbedRelationships = relationships.asParent | where: "childGridConfig.renderMode", "tab" -%}
}

{%- capture form_content -%}
    {%- assign hasModalLookup = false -%}
    {%- for rel in relationships.asChild -%}
        {%- if rel.lookupMode == 'modal' -%}
            {%- assign hasModalLookup = true -%}
        {%- endif -%}
    {%- endfor -%}
    <abp-form-content />
    {%- if hasModalLookup -%}
    {%- for rel in relationships.asChild -%}
        {%- if rel.lookupMode == 'modal' -%}
        <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" lookup-entity="{{ rel.parentEntityName }}" display-field="{{ rel.displayField }}" allow-create="true" 
                          label="@L["{{ entity.name }}:{{ rel.fkFieldName }}"].Value"
                          lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                          display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
        {%- endif -%}
    {%- endfor -%}
    {%- endif -%}

    {%- for rel in relationships.asParent -%}
    {%- if rel.isChildGrid and rel.childGridConfig.renderMode != 'tab' %}
        {{ child_grid_html | replace: "REL_REPLACE", rel.targetEntityName | replace: "PLURAL_REPLACE", rel.targetPluralName | replace: "TITLE_REPLACE", rel.childGridConfig.title }}
    {%- endif -%}
    {%- endfor -%}
{%- endcapture -%}

{%- capture child_grid_template -%}
    <div class="mt-4">
        <div class="d-flex justify-content-between align-items-center mb-2">
             <label class="form-label mb-0">@L["TITLE_REPLACE"].Value</label>
             <button type="button" class="btn btn-sm btn-outline-primary AddChildItemBtn" 
                     data-entity="REL_REPLACE" 
                     data-plural="PLURAL_REPLACE"
                     id="AddREL_REPLACEBtn">
                <i class="fa fa-plus"></i> @L["Add"].Value
             </button>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered child-grid-table" id="PLURAL_REPLACETable" data-entity="REL_REPLACE">
                <thead>
                    <tr>
                        <th class="actions-column" style="width: 50px"></th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
{%- endcapture -%}

<abp-dynamic-form abp-model="ViewModel" data-ajaxForm="true" asp-page="CreateModal">
    <abp-modal id="{{ entity.name }}CreateModal" size="ExtraLarge">
        <abp-modal-header title="@L["New{{ entity.name }}"].Value"></abp-modal-header>
        <abp-modal-body>
            {%- if tabbedRelationships.size > 0 -%}
            <abp-tabs name="{{ entity.name }}CreateTabs">
                <abp-tab title="@L["General"].Value" active="true">
                    <div class="mt-3">
                        {{ form_content }}
                    </div>
                </abp-tab>
                {%- for rel in tabbedRelationships -%}
                <abp-tab title="@L["{{ rel.childGridConfig.title | default: rel.targetPluralName }}"].Value">
                    <div class="mt-3">
                        {{ child_grid_template | replace: "REL_REPLACE", rel.targetEntityName | replace: "PLURAL_REPLACE", rel.targetPluralName | replace: "TITLE_REPLACE", rel.childGridConfig.title | default: rel.targetPluralName }}
                    </div>
                </abp-tab>
                {%- endfor -%}
            </abp-tabs>
            {%- else -%}
                {{ form_content }}
            {%- endif -%}
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
    {%- assign tabbedRelationships = relationships.asParent | where: "childGridConfig.renderMode", "tab" -%}
}

{%- capture form_content -%}
    {%- assign hasModalLookup = false -%}
    {%- for rel in relationships.asChild -%}
        {%- if rel.lookupMode == 'modal' -%}
            {%- assign hasModalLookup = true -%}
        {%- endif -%}
    {%- endfor -%}
    <abp-form-content />
    {%- if hasModalLookup -%}
    {%- for rel in relationships.asChild -%}
        {%- if rel.lookupMode == 'modal' -%}
        <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" lookup-entity="{{ rel.parentEntityName }}" display-field="{{ rel.displayField }}" allow-create="true" 
                          label="@L["{{ entity.name }}:{{ rel.fkFieldName }}"].Value"
                          lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                          display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
        {%- endif -%}
    {%- endfor -%}
    {%- endif -%}

    {%- for rel in relationships.asParent -%}
    {%- if rel.isChildGrid and rel.childGridConfig.renderMode != 'tab' %}
        {{ child_grid_html | replace: "REL_REPLACE", rel.targetEntityName | replace: "PLURAL_REPLACE", rel.targetPluralName | replace: "TITLE_REPLACE", rel.childGridConfig.title }}
    {%- endif -%}
    {%- endfor -%}
{%- endcapture -%}

{%- capture child_grid_template -%}
    <div class="mt-4">
        <div class="d-flex justify-content-between align-items-center mb-2">
             <label class="form-label mb-0">@L["TITLE_REPLACE"].Value</label>
             <button type="button" class="btn btn-sm btn-outline-primary AddChildItemBtn" 
                     data-entity="REL_REPLACE" 
                     data-plural="PLURAL_REPLACE"
                     id="AddREL_REPLACEBtn">
                <i class="fa fa-plus"></i> @L["Add"].Value
             </button>
        </div>
        <div class="table-responsive">
            <table class="table table-striped table-hover table-bordered child-grid-table" id="PLURAL_REPLACETable" data-entity="REL_REPLACE">
                <thead>
                    <tr>
                        <th class="actions-column" style="width: 50px"></th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
{%- endcapture -%}

<abp-dynamic-form abp-model="ViewModel" data-ajaxForm="true" asp-page="EditModal">
    <abp-modal id="{{ entity.name }}EditModal" size="ExtraLarge">
        <abp-modal-header title="@L["Edit{{ entity.name }}"].Value"></abp-modal-header>
        <abp-modal-body>
            <input type="hidden" name="Id" value="@Model.Id" />
            {%- if tabbedRelationships.size > 0 -%}
            <abp-tabs name="{{ entity.name }}EditTabs">
                <abp-tab title="@L["General"].Value" active="true">
                    <div class="mt-3">
                        {{ form_content }}
                    </div>
                </abp-tab>
                {%- for rel in tabbedRelationships -%}
                <abp-tab title="@L["{{ rel.childGridConfig.title | default: rel.targetPluralName }}"].Value">
                    <div class="mt-3">
                        {{ child_grid_template | replace: "REL_REPLACE", rel.targetEntityName | replace: "PLURAL_REPLACE", rel.targetPluralName | replace: "TITLE_REPLACE", rel.childGridConfig.title | default: rel.targetPluralName }}
                    </div>
                </abp-tab>
                {%- endfor -%}
            </abp-tabs>
            {%- else -%}
                {{ form_content }}
            {%- endif -%}
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
`;
}
