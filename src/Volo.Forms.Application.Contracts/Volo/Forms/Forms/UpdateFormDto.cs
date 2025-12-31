using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Volo.Forms.Forms;

public class UpdateFormDto : IHasConcurrencyStamp
{
    [Required]
    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxTitleLength))]
    public string Title { get; set; }

    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxDescriptionLength))]
    public string Description { get; set; }


    public string ConcurrencyStamp { get; set; }
}
