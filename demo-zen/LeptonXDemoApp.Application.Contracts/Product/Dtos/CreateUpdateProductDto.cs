using System;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Product.Dtos;

[Serializable]
public class CreateUpdateProductDto
{
    public string Name { get; set; }
    public string Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CategoryId { get; set; }
}
