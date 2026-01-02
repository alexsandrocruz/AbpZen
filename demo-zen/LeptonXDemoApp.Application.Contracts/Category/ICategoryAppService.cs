using System;
using LeptonXDemoApp.Category.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Category;

public interface ICategoryAppService :
    ICrudAppService<
        CategoryDto,
        Guid,
        CategoryGetListInput,
        CreateUpdateCategoryDto,
        CreateUpdateCategoryDto>
{
}
