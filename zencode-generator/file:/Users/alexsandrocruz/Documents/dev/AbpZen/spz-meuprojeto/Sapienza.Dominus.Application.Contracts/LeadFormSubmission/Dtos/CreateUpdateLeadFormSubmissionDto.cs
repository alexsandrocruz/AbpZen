using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.LeadFormSubmission.Dtos;

[Serializable]
public class CreateUpdateLeadFormSubmissionDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string IpAddress { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadFormId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
