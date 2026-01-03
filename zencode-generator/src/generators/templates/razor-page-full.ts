
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
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.{{ rel.displayField }}, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
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
        var {{ rel.parentEntityName | camelCase }}List = await _{{ rel.parentEntityName | camelCase }}AppService.GetListAsync(new {{ rel.parentEntityName }}GetListInput { MaxResultCount = 1000 });
        {{ rel.parentEntityName }}List = {{ rel.parentEntityName | camelCase }}List.Items
            .Select(x => new SelectListItem(x.{{ rel.displayField }}, x.Id.ToString()))
            .ToList();
        ViewModel.{{ rel.parentEntityName }}List = {{ rel.parentEntityName }}List;
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

<form method="post" id="Create{{ entity.name }}Form">
    <abp-card>
        <abp-card-header>
            <abp-row>
                <abp-column size-md="_6">
                    <abp-card-title>@L["New{{ entity.name }}"].Value</abp-card-title>
                </abp-column>
                <abp-column size-md="_6" class="text-end">
                </abp-column>
            </abp-row>
        </abp-card-header>
        <abp-card-body>
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
                    <div class="mb-3">
                        <label class="form-label" for="ViewModel_{{ field.name }}">
                            @L["{{ entity.name }}:{{ field.name }}"]
                            {%- if field.isRequired %}<span class="text-danger">*</span>{%- endif %}
                        </label>
                        {%- if field.type == 'string' and field.isTextArea %}
                        <textarea asp-for="ViewModel.{{ field.name }}" 
                                  class="form-control" 
                                  rows="3"
                                  placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}"></textarea>
                        {%- elsif field.type == 'string' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="text"
                               placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}" />
                        {%- elsif field.type == 'int' or field.type == 'long' or field.type == 'decimal' or field.type == 'double' or field.type == 'float' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="number"
                               placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}" />
                        {%- elsif field.type == 'bool' %}
                        <div class="form-check">
                            <input asp-for="ViewModel.{{ field.name }}" class="form-check-input" type="checkbox" />
                        </div>
                        {%- elsif field.type == 'datetime' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="datetime-local" />
                        {%- elsif field.type == 'enum' and field.enumConfig %}
                        <select asp-for="ViewModel.{{ field.name }}" class="form-control">
                            <option value="">@L["Select"]</option>
                            @foreach (var val in Enum.GetValues<{{ field.enumConfig.enumName }}>())
                            {
                                <option value="@((int)val)">@L["Enum:{{ field.enumConfig.enumName }}:" + val.ToString()]</option>
                            }
                        </select>
                        {%- else %}
                        <input asp-for="ViewModel.{{ field.name }}" class="form-control" />
                        {%- endif %}
                        <span asp-validation-for="ViewModel.{{ field.name }}" class="text-danger"></span>
                    </div>
                </abp-column>
            {%- endunless %}
            {%- endunless %}
            {%- endfor %}

            {%- for rel in relationships.asChild %}
                <abp-column size-md="_12">
                    <div class="mb-3">
                        <label class="form-label" for="ViewModel_{{ rel.fkFieldName }}">
                            @L["{{ entity.name }}:{{ rel.fkFieldName }}"]
                            {%- if rel.isRequired %}<span class="text-danger">*</span>{%- endif %}
                        </label>
                        {%- if rel.lookupMode == 'modal' %}
                        <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" 
                                          lookup-entity="{{ rel.parentEntityName }}" 
                                          display-field="{{ rel.displayField }}" 
                                          allow-create="true"
                                          lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                                          display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
                        {%- else %}
                        <select asp-for="ViewModel.{{ rel.fkFieldName }}" asp-items="Model.ViewModel.{{ rel.parentEntityName }}List" class="form-control">
                            <option value="">@L["Select"]</option>
                        </select>
                        {%- endif %}
                        <span asp-validation-for="ViewModel.{{ rel.fkFieldName }}" class="text-danger"></span>
                    </div>
                </abp-column>
            {%- endfor %}
            </abp-row>

            {%- for rel in relationships.asParent %}
            {%- if rel.isChildGrid %}
            <hr class="my-4" />
            <div class="mt-2">
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <h5 class="mb-0">@L["{{ rel.targetPluralName }}"]</h5>
                    <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                        <i class="fa fa-plus"></i> @L["Add{{ rel.targetEntityName }}"]
                    </button>
                </div>
                <table class="table table-striped table-hover table-bordered" id="{{ rel.targetPluralName }}Table">
                    <thead class="table-light">
                        <tr>
                            <th>Item</th>
                            <th>Quantity</th>
                            <th>Total</th>
                            <th style="width: 60px">@L["Actions"]</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            {%- endif %}
            {%- endfor %}
        </abp-card-body>
        <abp-card-footer>
            <a asp-page="Index" class="btn btn-secondary">@L["Cancel"]</a>
            <button type="submit" class="btn btn-primary">@L["Save"]</button>
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
        var {{ rel.targetEntityName | camelCase }}InitialData = @Html.Raw(Json.Serialize(Model.ViewModel.{{ rel.navigationName }}));
    {%- endif %}
    {%- endfor %}
    </script>
    <abp-script src="/Pages/{{ entity.name }}/Edit.js" />
}

<form method="post" id="Edit{{ entity.name }}Form">
    <abp-card>
        <abp-card-header>
            <abp-row>
                <abp-column size-md="_6">
                    <abp-card-title>@L["Edit{{ entity.name }}"].Value</abp-card-title>
                </abp-column>
                <abp-column size-md="_6" class="text-end">
                </abp-column>
            </abp-row>
        </abp-card-header>
        <abp-card-body>
            <input type="hidden" name="Id" value="@Model.Id" />
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
                    <div class="mb-3">
                        <label class="form-label" for="ViewModel_{{ field.name }}">
                            @L["{{ entity.name }}:{{ field.name }}"]
                            {%- if field.isRequired %}<span class="text-danger">*</span>{%- endif %}
                        </label>
                        {%- if field.type == 'string' and field.isTextArea %}
                        <textarea asp-for="ViewModel.{{ field.name }}" 
                                  class="form-control" 
                                  rows="3"
                                  {%- if field.readOnly %} readonly{%- endif %}
                                  placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}"></textarea>
                        {%- elsif field.type == 'string' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="text"
                               {%- if field.readOnly %} readonly{%- endif %}
                               placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}" />
                        {%- elsif field.type == 'int' or field.type == 'long' or field.type == 'decimal' or field.type == 'double' or field.type == 'float' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="number"
                               {%- if field.readOnly %} readonly{%- endif %}
                               placeholder="{%- if field.placeholder %}{{ field.placeholder }}{%- endif %}" />
                        {%- elsif field.type == 'bool' %}
                        <div class="form-check">
                            <input asp-for="ViewModel.{{ field.name }}" class="form-check-input" type="checkbox" {%- if field.readOnly %} disabled{%- endif %} />
                        </div>
                        {%- elsif field.type == 'datetime' %}
                        <input asp-for="ViewModel.{{ field.name }}" 
                               class="form-control" 
                               type="datetime-local"
                               {%- if field.readOnly %} readonly{%- endif %} />
                        {%- elsif field.type == 'enum' and field.enumConfig %}
                        <select asp-for="ViewModel.{{ field.name }}" class="form-control" {%- if field.readOnly %} disabled{%- endif %}>
                            <option value="">@L["Select"]</option>
                            @foreach (var val in Enum.GetValues<{{ field.enumConfig.enumName }}>())
                            {
                                <option value="@((int)val)">@L["Enum:{{ field.enumConfig.enumName }}:" + val.ToString()]</option>
                            }
                        </select>
                        {%- else %}
                        <input asp-for="ViewModel.{{ field.name }}" class="form-control" {%- if field.readOnly %} readonly{%- endif %} />
                        {%- endif %}
                        <span asp-validation-for="ViewModel.{{ field.name }}" class="text-danger"></span>
                    </div>
                </abp-column>
            {%- endunless %}
            {%- endunless %}
            {%- endfor %}

            {%- for rel in relationships.asChild %}
                <abp-column size-md="_12">
                    <div class="mb-3">
                        <label class="form-label" for="ViewModel_{{ rel.fkFieldName }}">
                            @L["{{ entity.name }}:{{ rel.fkFieldName }}"]
                            {%- if rel.isRequired %}<span class="text-danger">*</span>{%- endif %}
                        </label>
                        {%- if rel.lookupMode == 'modal' %}
                        <zen-lookup-input for="ViewModel.{{ rel.fkFieldName }}" 
                                          lookup-entity="{{ rel.parentEntityName }}" 
                                          display-field="{{ rel.displayField }}" 
                                          allow-create="true"
                                          lookup-modal-title="@L["{{ rel.parentEntityName }}"].Value"
                                          display-value="@Model.ViewModel.{{ rel.parentEntityName }}DisplayName" />
                        {%- else %}
                        <select asp-for="ViewModel.{{ rel.fkFieldName }}" asp-items="Model.ViewModel.{{ rel.parentEntityName }}List" class="form-control">
                            <option value="">@L["Select"]</option>
                        </select>
                        {%- endif %}
                        <span asp-validation-for="ViewModel.{{ rel.fkFieldName }}" class="text-danger"></span>
                    </div>
                </abp-column>
            {%- endfor %}
            </abp-row>

            {%- for rel in relationships.asParent %}
            {%- if rel.isChildGrid %}
            <hr class="my-4" />
            <div class="mt-2">
                <div class="d-flex justify-content-between align-items-center mb-3">
                    <h5 class="mb-0">@L["{{ rel.targetPluralName }}"]</h5>
                    <button type="button" class="btn btn-sm btn-outline-primary" id="Add{{ rel.targetEntityName }}Btn">
                        <i class="fa fa-plus"></i> @L["Add{{ rel.targetEntityName }}"]
                    </button>
                </div>
                <table class="table table-striped table-hover table-bordered" id="{{ rel.targetPluralName }}Table">
                    <thead class="table-light">
                        <tr>
                            <th>Item</th>
                            <th>Quantity</th>
                            <th>Total</th>
                            <th style="width: 60px">@L["Actions"]</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            {%- endif %}
            {%- endfor %}
        </abp-card-body>
        <abp-card-footer>
            <a asp-page="Index" class="btn btn-secondary">@L["Cancel"]</a>
            <button type="submit" class="btn btn-primary">@L["Save"]</button>
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
    var _{{ rel.targetEntityName | camelCase }}List = [];
    
    function render{{ rel.targetPluralName }}Table() {
        var tbody = $('#{{ rel.targetPluralName }}Table tbody');
        tbody.empty();

        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            var row = '<tr>';
            // TODO: Dynamic columns based on Child Entity Fields
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-{{ rel.targetEntityName | camelCase }}-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });

        $('.delete-{{ rel.targetEntityName | camelCase }}-btn').click(function () {
            var index = $(this).data('index');
            _{{ rel.targetEntityName | camelCase }}List.splice(index, 1);
            render{{ rel.targetPluralName }}Table();
        });
    }

    $('#Add{{ rel.targetEntityName }}Btn').click(function () {
        // Mock Add Logic
        var newItem = {
            id: abp.utils.createGuid(),
            quantity: 1,
            total: 100
        };
        _{{ rel.targetEntityName | camelCase }}List.push(newItem);
        render{{ rel.targetPluralName }}Table();
    });
    
    // Initial Render
    render{{ rel.targetPluralName }}Table();
    
    // Form Submit Injection
    $('#Create{{ entity.name }}Form').submit(function (e) {
        var $form = $(this);
        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            // In C#, Binding a List<T> requires indexers: OrderItems[0].Id
            // We iterate properties of the item
            
            // Core ID (if exists, though create usually doesn't have valid ID for child yet, but FKs do)
            if(item.id) {
                $form.append('<input type="hidden" name="{{ rel.navigationName }}[' + index + '].id" value="' + item.id + '" />');
            }

            // Iterate other props (Mocked for now, assumes 'item' is flat object matching DTO)
            for (var prop in item) {
                if(prop !== 'id' && typeof item[prop] !== 'function' && typeof item[prop] !== 'object') {
                     $form.append('<input type="hidden" name="{{ rel.navigationName }}[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                }
            }
        });
        // Allow form to continue
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
    var _{{ rel.targetEntityName | camelCase }}List = [];

    // Load initial data
    if (typeof {{ rel.targetEntityName | camelCase }}InitialData !== 'undefined') {
        _{{ rel.targetEntityName | camelCase }}List = {{ rel.targetEntityName | camelCase }}InitialData;
    }

    function render{{ rel.targetPluralName }}Table() {
        var tbody = $('#{{ rel.targetPluralName }}Table tbody');
        tbody.empty();

        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            var row = '<tr>';
            row += '<td>' + (item.productDisplayName || item.name || 'Item ' + (index + 1)) + '</td>';
            row += '<td>' + (item.quantity || 1) + '</td>';
            row += '<td>' + (item.total || 0) + '</td>';
            row += '<td><button type="button" class="btn btn-danger btn-sm delete-{{ rel.targetEntityName | camelCase }}-btn" data-index="' + index + '"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            tbody.append(row);
        });

        $('.delete-{{ rel.targetEntityName | camelCase }}-btn').click(function () {
            var index = $(this).data('index');
            _{{ rel.targetEntityName | camelCase }}List.splice(index, 1);
            render{{ rel.targetPluralName }}Table();
        });
    }

    $('#Add{{ rel.targetEntityName }}Btn').click(function () {
        var newItem = {
            id: abp.utils.createGuid(),
            quantity: 1,
            total: 100
        };
        _{{ rel.targetEntityName | camelCase }}List.push(newItem);
        render{{ rel.targetPluralName }}Table();
    });
    
    render{{ rel.targetPluralName }}Table();

    $('#Edit{{ entity.name }}Form').submit(function (e) {
        var $form = $(this);
        _{{ rel.targetEntityName | camelCase }}List.forEach(function (item, index) {
            if(item.id) {
                $form.append('<input type="hidden" name="{{ rel.navigationName }}[' + index + '].id" value="' + item.id + '" />');
            }
            for (var prop in item) {
                if(prop !== 'id' && typeof item[prop] !== 'function' && typeof item[prop] !== 'object') {
                     $form.append('<input type="hidden" name="{{ rel.navigationName }}[' + index + '].' + prop + '" value="' + item[prop] + '" />');
                }
            }
        });
        return true;
    });

    {%- endif %}
    {%- endfor %}
});
`;
}
