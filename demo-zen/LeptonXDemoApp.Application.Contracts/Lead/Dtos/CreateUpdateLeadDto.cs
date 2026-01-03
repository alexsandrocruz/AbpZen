using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Lead.Dtos;

[Serializable]
public class CreateUpdateLeadDto
{
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
