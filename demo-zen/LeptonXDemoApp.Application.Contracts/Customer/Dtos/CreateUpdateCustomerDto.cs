using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Customer.Dtos;

[Serializable]
public class CreateUpdateCustomerDto
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
