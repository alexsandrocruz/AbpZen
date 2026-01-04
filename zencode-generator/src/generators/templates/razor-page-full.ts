
/**
 * Create Full Page PageModel template
 */
export function getRazorCreatePageModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;
{%- for rel in relationships.asChild %}
using {{ project.namespace }}.{{ rel.parentEntityName }};
using {{ project.namespace }}.{{ rel.parentEntityName }}.Dtos;
{%- endfor %}

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class CreateModel : {{ project.name }}PageModel
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

    public CreateModel(
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
        var {{ rel.parentEntityName | camelCase }}Result = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}Result.Items
            .Select(x => new SelectListItem(x.{{ rel.displayField }}, x.Id.ToString()))
            .ToList();
        {%- endif %}
        {%- endfor %}
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Create{{ entity.name }}ViewModel, CreateUpdate{{ entity.name }}Dto>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.CreateAsync(dto);
        return RedirectToPage("Index");
    }
}
`;
}

/**
 * Edit Full Page PageModel template
 */
export function getRazorEditPageModelTemplate(): string {
    return `using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;
using {{ project.namespace }}.Web.Pages.{{ entity.name }}.ViewModels;
{%- for rel in relationships.asChild %}
using {{ project.namespace }}.{{ rel.parentEntityName }};
using {{ project.namespace }}.{{ rel.parentEntityName }}.Dtos;
{%- endfor %}

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class EditModel : {{ project.name }}PageModel
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

    public EditModel(
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
        var {{ rel.parentEntityName | camelCase }}Result = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}Result.Items
            .Select(x => new SelectListItem(x.{{ rel.displayField }}, x.Id.ToString()))
            .ToList();
        {%- endif %}
        {%- endfor %}
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var dto = ObjectMapper.Map<Edit{{ entity.name }}ViewModel, CreateUpdate{{ entity.name }}Dto>(ViewModel);
        await _{{ entity.name | camelCase }}AppService.UpdateAsync(Id, dto);
        return RedirectToPage("Index");
    }
}
`;
}

/**
 * Razor View template for Create Full Page
 */
export function getRazorCreatePageViewTemplate(): string {
    return `@page
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;
@using {{ project.namespace }}
@using {{ project.namespace }}.Localization
@inject IHtmlLocalizer<{{ project.name }}Resource> L
@model {{ project.namespace }}.Web.Pages.{{ entity.name }}.CreateModel

@section scripts {
    <abp-script src="/Pages/{{ entity.name }}/Create.js" />
}

{% capture form_content %}
    <abp-row>
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- assign isFk = false %}
    {%- for rel in relationships.asChild %}
        {%- if rel.fkFieldName == field.name %}{% assign isFk = true %}{% endif %}
    {%- endfor %}
    {%- unless isFk %}
    {%- assign colSize = "_12" %}
    {%- if field.formWidth == 'half' %}{% assign colSize = "_6" %}{% endif %}
    {%- if field.formWidth == 'third' %}{% assign colSize = "_4" %}{% endif %}
        <abp-column size-md="{{ colSize }}">
            <abp-input asp-for="ViewModel.{{ field.name }}" label="@L["{{ entity.name }}:{{ field.name }}"].Value" />
        </abp-column>
    {%- endunless %}
    {%- endunless %}
    {%- endfor %}

    {%- for rel in relationships.asChild %}
        <abp-column size-md="_12">
            {%- if rel.lookupMode == 'modal' %}
            <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" 
                              lookup-entity="{{ rel.parentEntityName }}" 
                              display-field="{{ rel.displayField }}" 
                              allow-create="true"
                              lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                              display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
            {%- else %}
            <abp-select asp-for="ViewModel.{{ rel.fkFieldName }}" asp-items="Model.{{ rel.parentEntityName }}List" label="@L["{{ entity.name }}:{{ rel.fkFieldName }}"].Value">
                <option value="">@L["Select"]</option>
            </abp-select>
            {%- endif %}
        </abp-column>
    {%- endfor %}
    </abp-row>
{% endcapture %}

{% capture child_grid_template %}
    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    <div class="mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h5 class="mb-0">@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"]</h5>
            <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                <i class="fa fa-plus"></i> @L["Add"]
            </button>
        </div>
        <table class="table table-striped table-hover table-bordered" id="{{ rel.targetPluralName }}Table">
            <thead class="table-light">
                <tr>
                    <th style="width: 80px">@L["Actions"]</th>
                    {%- for field in rel.targetFields -%}
                    {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                    {%- elsif field.isLookup -%}
                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                    {%- endif -%}
                    {%- endfor %}
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    {%- endif %}
    {%- endfor %}
{% endcapture %}

<form method="post" id="Create{{ entity.name }}Form">
    <abp-card>
        <abp-card-header>
            <abp-card-title>@L["New{{ entity.name }}"].Value</abp-card-title>
        </abp-card-header>
        <abp-card-body>
            {%- assign hasTabs = false %}
            {%- for rel in relationships.asParent %}{% if rel.childGridConfig.renderMode == 'tab' %}{% assign hasTabs = true %}{% endif %}{% endfor %}

            {%- if hasTabs %}
            <abp-tabs>
                <abp-tab title="@L["General"].Value">
                    {{ form_content }}
                </abp-tab>
                {%- for rel in relationships.asParent %}
                {%- if rel.isChildGrid and rel.childGridConfig.renderMode == 'tab' %}
                <abp-tab title="@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"].Value">
                    <div class="mt-2">
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <h6 class="mb-0">@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"]</h6>
                            <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                                <i class="fa fa-plus"></i> @L["Add"]
                            </button>
                        </div>
                        <table class="table table-striped table-hover table-bordered w-100" id="{{ rel.targetPluralName }}Table">
                            <thead class="table-light">
                                <tr>
                                    <th style="width: 80px">@L["Actions"]</th>
                                    {%- for field in rel.targetFields -%}
                                    {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                                    {%- elsif field.isLookup -%}
                                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                                    {%- endif -%}
                                    {%- endfor %}
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </abp-tab>
                {%- endif %}
                {%- endfor %}
            </abp-tabs>
            {%- else %}
                {{ form_content }}
                {{ child_grid_template }}
            {%- endif %}
        </abp-card-body>
        <abp-card-footer>
            <a asp-page="Index" class="btn btn-secondary">@L["Cancel"]</a>
            <abp-button button-type="Primary" type="submit" text="@L["Save"].Value" />
        </abp-card-footer>
    </abp-card>
</form>
`;
}

/**
 * Razor View template for Edit Full Page
 */
export function getRazorEditPageViewTemplate(): string {
    return `@page "{id}"
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;
@using {{ project.namespace }}
@using {{ project.namespace }}.Localization
@inject IHtmlLocalizer<{{ project.name }}Resource> L
@model {{ project.namespace }}.Web.Pages.{{ entity.name }}.EditModel

@section scripts {
    <script>
    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
        var {{ rel.targetEntityName | camelCase }}InitialData = @Html.Raw(Json.Serialize(Model.ViewModel.{{ rel.targetPluralName }}));
    {%- endif %}
    {%- endfor %}
    </script>
    <abp-script src="/Pages/{{ entity.name }}/Edit.js" />
}

{% capture form_content %}
    <input type="hidden" asp-for="Id" />
    <abp-row>
    {%- for field in entity.fields %}
    {%- unless field.isLookup %}
    {%- assign isFk = false %}
    {%- for rel in relationships.asChild %}
        {%- if rel.fkFieldName == field.name %}{% assign isFk = true %}{% endif %}
    {%- endfor %}
    {%- unless isFk %}
    {%- assign colSize = "_12" %}
    {%- if field.formWidth == 'half' %}{% assign colSize = "_6" %}{% endif %}
    {%- if field.formWidth == 'third' %}{% assign colSize = "_4" %}{% endif %}
        <abp-column size-md="{{ colSize }}">
            <abp-input asp-for="ViewModel.{{ field.name }}" label="@L["{{ entity.name }}:{{ field.name }}"].Value" {%- if field.readOnly %} readonly{%- endif %} />
        </abp-column>
    {%- endunless %}
    {%- endunless %}
    {%- endfor %}

    {%- for rel in relationships.asChild %}
        <abp-column size-md="_12">
            {%- if rel.lookupMode == 'modal' %}
            <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" 
                              lookup-entity="{{ rel.parentEntityName }}" 
                              display-field="{{ rel.displayField }}" 
                              allow-create="true"
                              lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                              display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
            {%- else %}
            <abp-select asp-for="ViewModel.{{ rel.fkFieldName }}" asp-items="Model.{{ rel.parentEntityName }}List" label="@L["{{ entity.name }}:{{ rel.fkFieldName }}"].Value">
                <option value="">@L["Select"]</option>
            </abp-select>
            {%- endif %}
        </abp-column>
    {%- endfor %}
    </abp-row>
{% endcapture %}

{% capture child_grid_template %}
    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    <div class="mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h5 class="mb-0">@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"]</h5>
            <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                <i class="fa fa-plus"></i> @L["Add"]
            </button>
        </div>
        <table class="table table-striped table-hover table-bordered" id="{{ rel.targetPluralName }}Table">
            <thead class="table-light">
                <tr>
                    <th style="width: 80px">@L["Actions"]</th>
                    {%- for field in rel.targetFields -%}
                    {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                    {%- elsif field.isLookup -%}
                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                    {%- endif -%}
                    {%- endfor %}
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    {%- endif %}
    {%- endfor %}
{% endcapture %}

<form method="post" id="Edit{{ entity.name }}Form">
    <abp-card>
        <abp-card-header>
            <abp-card-title>@L["Edit{{ entity.name }}"].Value</abp-card-title>
        </abp-card-header>
        <abp-card-body>
            {%- assign hasTabs = false %}
            {%- for rel in relationships.asParent %}{% if rel.childGridConfig.renderMode == 'tab' %}{% assign hasTabs = true %}{% endif %}{% endfor %}

            {%- if hasTabs %}
            <abp-tabs>
                <abp-tab title="@L["General"].Value">
                    {{ form_content }}
                </abp-tab>
                {%- for rel in relationships.asParent %}
                {%- if rel.isChildGrid and rel.childGridConfig.renderMode == 'tab' %}
                <abp-tab title="@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"].Value">
                    <div class="mt-2">
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <h6 class="mb-0">@L["{{ rel.childGridConfig.title || rel.targetPluralName }}"]</h6>
                            <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                                <i class="fa fa-plus"></i> @L["Add"]
                            </button>
                        </div>
                        <table class="table table-striped table-hover table-bordered w-100" id="{{ rel.targetPluralName }}Table">
                            <thead class="table-light">
                                <tr>
                                    <th style="width: 80px">@L["Actions"]</th>
                                    {%- for field in rel.targetFields -%}
                                    {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                                    {%- elsif field.isLookup -%}
                                    <th>@L["{{ rel.targetEntityName }}:{{ field.name }}"]</th>
                                    {%- endif -%}
                                    {%- endfor %}
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </abp-tab>
                {%- endif %}
                {%- endfor %}
            </abp-tabs>
            {%- else %}
                {{ form_content }}
                {{ child_grid_template }}
            {%- endif %}
        </abp-card-body>
        <abp-card-footer>
            <a asp-page="Index" class="btn btn-secondary">@L["Cancel"]</a>
            <abp-button button-type="Primary" type="submit" text="@L["Save"].Value" />
        </abp-card-footer>
    </abp-card>
</form>
`;
}

/**
 * JS Template for Create Full Page
 */
export function getRazorCreatePageJsTemplate(): string {
    return `$(function () {
    var l = abp.localization.getResource('{{ project.name }}');

    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    // ---------- {{ rel.targetPluralName }} Child Grid ----------
    var _{{ rel.targetPluralName | camelCase }}Table = $('#{{ rel.targetPluralName }}Table').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: false,
            searching: false,
            paging: false,
            info: false,
            scrollX: true,
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('Delete'),
                                action: function (data) {
                                    _{{ rel.targetPluralName | camelCase }}Table.row(data.record.row).remove().draw();
                                }
                            }
                        ]
                    }
                },
                {%- for field in rel.targetFields -%}
                {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                {
                    title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                    data: "{{ field.name | camelCase }}"
                },
                {%- elsif field.isLookup -%}
                {
                    title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                    data: "{{ field.lookupConfig.targetEntity | camelCase }}DisplayName",
                    defaultContent: ""
                },
                {%- endif -%}
                {%- endfor %}
            ]
        })
    );

    $('#Add{{ rel.targetEntityName }}Btn').click(function () {
        {%- if rel.isManyToMany -%}
        var name = prompt("Select {{ rel.targetPluralName }} (Placeholder)");
        if (name) {
            _{{ rel.targetPluralName | camelCase }}Table.row.add({
                id: abp.utils.createGuid(),
                {{ rel.displayField | camelCase }}: name,
                {%- for field in rel.targetFields -%}
                {%- if field.isLookup == false and field.name != 'Id' and field.name != 'TenantId' -%}
                {{ field.name | camelCase }}: null,
                {%- endif -%}
                {%- endfor %}
            }).draw();
        }
        {%- else -%}
        _{{ rel.targetPluralName | camelCase }}Table.row.add({ id: abp.utils.createGuid() }).draw();
        {%- endif -%}
    });

    // Form Submit Sync
    $('#Create{{ entity.name }}Form').submit(function (e) {
        var $form = $(this);
        var data = _{{ rel.targetPluralName | camelCase }}Table.data().toArray();
        data.forEach(function (item, index) {
            {%- for field in rel.targetFields -%}
            {%- if field.name != 'TenantId' -%}
            $form.append('<input type="hidden" name="ViewModel.{{ rel.targetPluralName }}[' + index + '].{{ field.name }}" value="' + (item.{{ field.name | camelCase }} || '') + '" />');
            {%- endif -%}
            {%- endfor %}
        });
        return true;
    });
    {%- endif %}
    {%- endfor %}
});
`;
}

/**
 * JS Template for Edit Full Page
 */
export function getRazorEditPageJsTemplate(): string {
    return `$(function () {
    var l = abp.localization.getResource('{{ project.name }}');

    {%- for rel in relationships.asParent %}
    {%- if rel.isChildGrid %}
    // ---------- {{ rel.targetPluralName }} Child Grid ----------
    var _{{ rel.targetPluralName | camelCase }}Table = $('#{{ rel.targetPluralName }}Table').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: false,
            searching: false,
            paging: false,
            info: false,
            scrollX: true,
            data: typeof {{ rel.targetPluralName | camelCase }}InitialData !== 'undefined' ? {{ rel.targetPluralName | camelCase }}InitialData : [],
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('Delete'),
                                action: function (data) {
                                    _{{ rel.targetPluralName | camelCase }}Table.row(data.record.row).remove().draw();
                                }
                            }
                        ]
                    }
                },
                {%- for field in rel.targetFields -%}
                {%- if field.name != 'Id' and field.name != 'TenantId' and field.isLookup == false -%}
                {
                    title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                    data: "{{ field.name | camelCase }}"
                },
                {%- elsif field.isLookup -%}
                {
                    title: l('{{ rel.targetEntityName }}:{{ field.name }}'),
                    data: "{{ field.lookupConfig.targetEntity | camelCase }}DisplayName",
                    defaultContent: ""
                },
                {%- endif -%}
                {%- endfor %}
            ]
        })
    );

    $('#Add{{ rel.targetEntityName }}Btn').click(function () {
        {%- if rel.isManyToMany -%}
        var name = prompt("Select {{ rel.targetPluralName }} (Placeholder)");
        if (name) {
            _{{ rel.targetPluralName | camelCase }}Table.row.add({
                id: abp.utils.createGuid(),
                {{ rel.displayField | camelCase }}: name,
                {%- for field in rel.targetFields -%}
                {%- if field.isLookup == false and field.name != 'Id' and field.name != 'TenantId' -%}
                {{ field.name | camelCase }}: null,
                {%- endif -%}
                {%- endfor %}
            }).draw();
        }
        {%- else -%}
        _{{ rel.targetPluralName | camelCase }}Table.row.add({ id: abp.utils.createGuid() }).draw();
        {%- endif -%}
    });

    // Form Submit Sync
    $('#Edit{{ entity.name }}Form').submit(function (e) {
        var $form = $(this);
        var data = _{{ rel.targetPluralName | camelCase }}Table.data().toArray();
        data.forEach(function (item, index) {
            {%- for field in rel.targetFields -%}
            {%- if field.name != 'TenantId' -%}
            $form.append('<input type="hidden" name="ViewModel.{{ rel.targetPluralName }}[' + index + '].{{ field.name }}" value="' + (item.{{ field.name | camelCase }} || '') + '" />');
            {%- endif -%}
            {%- endfor %}
        });
        return true;
    });
    {%- endif %}
    {%- endfor %}
});
`;
}
