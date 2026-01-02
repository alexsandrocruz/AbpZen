using System;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Customer.Dtos;

[Serializable]
public class CreateUpdateCustomerDto
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
}
