using System;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.Category.Dtos;

[Serializable]
public class CreateUpdateCategoryDto
{
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
