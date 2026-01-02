using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Product.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Product;

public interface IProductAppService :
    ICrudAppService<
        ProductDto,
        Guid,
        ProductGetListInput,
        CreateUpdateProductDto,
        CreateUpdateProductDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetProductLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
