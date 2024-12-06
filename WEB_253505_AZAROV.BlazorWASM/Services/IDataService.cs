using WEB_253505_AZAROV.Domain.Entities;
namespace WEB_253505_AZAROV.BlazorWASM.Services;
public interface IDataService
{
    event Action DataLoaded;
    List<Category> Categories { get; set; }
    List<Item> Items { get; set; }
    bool Success { get; set; }
    string ErrorMessage { get; set; }
    int TotalPages { get; set; }
    int CurrentPage { get; set; }
    Category SelectedCategory { get; set; }
    public Task GetProductListAsync(int pageNo = 1);
    public Task GetCategoryListAsync();
}