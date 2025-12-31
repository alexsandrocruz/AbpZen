using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account;

public class DelegateNewUserInput
{
    public Guid TargetUserId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
}
