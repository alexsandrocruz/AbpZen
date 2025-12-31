using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Faqs;

public class CmsFaqOptionsViewModel
{
    [Required]
    public string GroupName { get; set; }
    
    public string SectionName { get; set; }
}