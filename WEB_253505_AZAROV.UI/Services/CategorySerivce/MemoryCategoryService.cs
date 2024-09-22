using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;


namespace WEB_253505_AZAROV.UI.Services;
public class MemoryCategoryService : ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = new List<Category>
        {
            new Category {Id=1, Name="Phones", NormalizedName="phones"},
            new Category {Id=2, Name="Laptops", NormalizedName="laptops"},
            new Category {Id=3, Name="Watches", NormalizedName="watches"},
            new Category {Id=4, Name="Gadgets", NormalizedName="gadgets"},
            new Category {Id=5, Name="Other", NormalizedName="other"},
        };
        var result = ResponseData<List<Category>>.Success(categories);
        return Task.FromResult(result);
    }
}