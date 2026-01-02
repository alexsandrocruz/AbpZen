using System;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Order.Dtos;

[Serializable]
public class CreateUpdateOrderDto
{
    public string Number { get; set; }
    public DateTime? Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CustomerId { get; set; }
}
