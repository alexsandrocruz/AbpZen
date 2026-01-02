/**
 * Razor Index Page template for ABP Web layer
 */
export function getRazorIndexTemplate(): string {
    return `@page
@using {{ project.namespace }}.Permissions
@using Microsoft.AspNetCore.Authorization
@using Microsoft.AspNetCore.Mvc.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Layout
@using {{ project.namespace }}.Web.Pages.{{ entity.name }}
@using {{ project.namespace }}.Localization
@using {{ project.namespace }}.Web.Menus
@model IndexModel
@inject IPageLayout PageLayout
@inject IHtmlLocalizer<{{ project.name }}Resource> L
@inject IAuthorizationService Authorization

@{
    PageLayout.Content.Title = L["{{ entity.pluralName }}"].Value;
    PageLayout.Content.MenuItemName = {{ project.name }}Menus.{{ entity.name }};
}

@section scripts
{
    <abp-script src="/Pages/{{ entity.name }}/index.js" />
}
@section styles
{
    <abp-style src="/Pages/{{ entity.name }}/index.css"/>
}
@section content_toolbar
{
    @if (await Authorization.IsGrantedAsync({{ project.name }}Permissions.{{ entity.name }}.Create))
    {
        <abp-button id="New{{ entity.name }}Button"
            text="@L["New{{ entity.name }}"].Value"
            icon="plus" size="Small"
            button-type="Primary" />
    }
}

<abp-card>
    <abp-card-body>
        <abp-row class="mb-3">
            <a abp-collapse-id="{{ entity.name }}Collapse" class="text-secondary">@L["TableFilter"]</a>
        </abp-row>
        <abp-dynamic-form abp-model="{{ entity.name }}Filter" id="{{ entity.name }}Filter" required-symbols="false" column-size="_3">
            <abp-collapse-body id="{{ entity.name }}Collapse">
                <abp-form-content />
            </abp-collapse-body>
        </abp-dynamic-form>
        <hr />
        <abp-table striped-rows="true" id="{{ entity.name }}Table" class="nowrap"/>
    </abp-card-body>
</abp-card>
`;
}

/**
 * Razor Index PageModel template
 */
export function getRazorIndexModelTemplate(): string {
    return `using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using {{ project.namespace }}.{{ entity.name }};
using {{ project.namespace }}.{{ entity.name }}.Dtos;

namespace {{ project.namespace }}.Web.Pages.{{ entity.name }};

public class IndexModel : {{ project.name }}PageModel
{
    public {{ entity.name }}FilterInput {{ entity.name }}Filter { get; set; }
    
    private readonly I{{ entity.name }}AppService _{{ entity.name | camelCase }}AppService;

    public IndexModel(I{{ entity.name }}AppService {{ entity.name | camelCase }}AppService)
    {
        _{{ entity.name | camelCase }}AppService = {{ entity.name | camelCase }}AppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync({{ entity.name }}GetListInput input)
    {
        var result = await _{{ entity.name | camelCase }}AppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _{{ entity.name | camelCase }}AppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class {{ entity.name }}FilterInput
{
    {%- for field in entity.fields %}
    {%- if field.isFilterable %}
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "{{ entity.name }}:{{ field.name }}")]
    {%- if field.type == 'string' %}
    public string? {{ field.name }} { get; set; }
    {%- elsif field.type == 'int' %}
    public int? {{ field.name }} { get; set; }
    {%- elsif field.type == 'guid' %}
    public Guid? {{ field.name }} { get; set; }
    {%- elsif field.type == 'datetime' %}
    public DateTime? {{ field.name }} { get; set; }
    {%- elsif field.type == 'bool' %}
    public bool? {{ field.name }} { get; set; }
    {%- elsif field.type == 'enum' and field.enumConfig %}
    public {{ field.enumConfig.enumName }}? {{ field.name }} { get; set; }
    {%- else %}
    public {{ field.type | csharpType: true }} {{ field.name }} { get; set; }
    {%- endif %}
    {%- endif %}
    {%- endfor %}
}
`;
}

/**
 * Razor Index JavaScript template
 */
export function getRazorIndexJsTemplate(): string {
    return `$(function () {

    function debounce(func, delay) {
        let timerId;
        return function(...args) {
            clearTimeout(timerId);
            timerId = setTimeout(() => {
                func.apply(this, args);
            }, delay);
        };
    }

    $("#{{ entity.name }}Filter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#{{ entity.name }}Filter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/{{ entity.name }}Filter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('{{ project.name }}');
    var createModal = new abp.ModalManager(abp.appPath + '{{ entity.name }}/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + '{{ entity.name }}/EditModal');

    var dataTable = $('#{{ entity.name }}Table').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(function (input) {
            return abp.ajax({
                url: '?handler=List',
                type: 'GET',
                data: input
            });
        }, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('{{ project.name }}.{{ entity.name }}.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('{{ project.name }}.{{ entity.name }}.Delete'),
                            confirmMessage: function (data) {
                                return l('{{ entity.name }}DeletionConfirmationMessage', data.record.id);
                            },
                            action: function (data) {
                                abp.ajax({
                                    url: '?handler=Delete&id=' + data.record.id,
                                    type: 'POST'
                                })
                                .then(function () {
                                    abp.notify.info(l('SuccessfullyDeleted'));
                                    dataTable.ajax.reload(null, false);
                                });
                            }
                        }
                    ]
                }
            },
            {%- for field in entity.fields %}
            {%- unless field.isLookup %}
            {
                title: l('{{ entity.name }}:{{ field.name }}'),
                data: "{{ field.name | camelCase }}",
                {%- if field.type == 'datetime' %}
                dataFormat: 'datetime'
                {%- endif %}
                {%- if field.type == 'bool' %}
                render: function (data) { return data ? l('Yes') : l('No'); }
                {%- endif %}
                {%- if field.type == 'enum' and field.enumConfig %}
                render: function (data) { return l('Enum:{{ field.enumConfig.enumName }}.' + data); }
                {%- endif %}
            },
            {%- endunless %}
            {%- endfor %}
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#New{{ entity.name }}Button').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
`;
}

/**
 * Razor Index CSS template (empty base)
 */
export function getRazorIndexCssTemplate(): string {
    return `/* {{ entity.name }} page styles */
`;
}
