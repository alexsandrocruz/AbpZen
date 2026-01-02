/**
 * ZenLookup TagHelper template for ABP Razor Pages
 * Generates a custom lookup input with modal selection
 */
export function getZenLookupTagHelperTemplate(): string {
    return `using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace {{ project.namespace }}.Web.TagHelpers;

/// <summary>
/// Custom Tag Helper for modal-based entity lookup
/// Usage: <zen-lookup-input for="CategoryId" lookup-entity="Category" display-field="Name" />
/// </summary>
[HtmlTargetElement("zen-lookup-input", Attributes = "for")]
public class ZenLookupInputTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; } = default!;

    [HtmlAttributeName("lookup-entity")]
    public string LookupEntity { get; set; } = string.Empty;

    [HtmlAttributeName("display-field")]
    public string DisplayField { get; set; } = "Name";

    [HtmlAttributeName("display-value")]
    public string? DisplayValue { get; set; }

    [HtmlAttributeName("label")]
    public string? Label { get; set; }

    [HtmlAttributeName("placeholder")]
    public string Placeholder { get; set; } = "Select...";

    [HtmlAttributeName("allow-create")]
    public bool AllowCreate { get; set; } = false;

    [HtmlAttributeName("lookup-url")]
    public string? LookupUrl { get; set; }

    [HtmlAttributeName("required")]
    public bool Required { get; set; } = false;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public ZenLookupInputTagHelper(IHtmlGenerator generator)
    {
        _generator = generator;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "zen-lookup-container mb-3");

        var fieldName = For.Name;
        var fieldId = fieldName.Replace(".", "_");
        var currentValue = For.Model?.ToString() ?? string.Empty;
        var displayText = DisplayValue ?? string.Empty;

        // Build the lookup URL if not provided
        var lookupUrl = LookupUrl ?? $"/api/app/{LookupEntity.ToLower()}/lookup";

        var html = $@"
            {(string.IsNullOrEmpty(Label) ? "" : $"<label class='form-label' for='{fieldId}'>{Label}{(Required ? " <span class='text-danger'>*</span>" : "")}</label>")}
            <div class='input-group'>
                <input type='hidden' 
                       id='{fieldId}' 
                       name='{fieldName}' 
                       value='{currentValue}'
                       data-zen-lookup='true'
                       data-lookup-entity='{LookupEntity}'
                       data-display-field='{DisplayField}'
                       data-lookup-url='{lookupUrl}' />
                <input type='text' 
                       class='form-control zen-lookup-display' 
                       id='{fieldId}_Display' 
                       value='{HtmlEncoder.Default.Encode(displayText)}'
                       placeholder='{Placeholder}'
                       readonly />
                <button type='button' 
                        class='btn btn-outline-primary zen-lookup-btn' 
                        data-target='{fieldId}'
                        title='Search'>
                    <i class='fa fa-search'></i>
                </button>
                {(AllowCreate ? $@"
                <button type='button' 
                        class='btn btn-outline-success zen-lookup-create-btn' 
                        data-target='{fieldId}'
                        data-entity='{LookupEntity}'
                        title='Create New'>
                    <i class='fa fa-plus'></i>
                </button>" : "")}
                <button type='button' 
                        class='btn btn-outline-secondary zen-lookup-clear-btn' 
                        data-target='{fieldId}'
                        title='Clear'
                        style='{(string.IsNullOrEmpty(currentValue) ? "display:none;" : "")}'>
                    <i class='fa fa-times'></i>
                </button>
            </div>
        ";

        output.Content.SetHtmlContent(html);
        await Task.CompletedTask;
    }
}
`;
}

/**
 * Template for registering the TagHelper in the project
 */
export function getZenLookupTagHelperRegistrationSnippet(): string {
    return `@addTagHelper *, {{ project.namespace }}.Web`;
}
