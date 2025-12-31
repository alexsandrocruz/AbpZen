using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.Forms.Forms;

public class CreateFormDto
{
    [Required]
    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxTitleLength))]
    public string Title { get; set; }

    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxDescriptionLength))]
    public string Description { get; set; }
}
