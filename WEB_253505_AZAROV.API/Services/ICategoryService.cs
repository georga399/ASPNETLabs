using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
namespace WEB_253505_AZAROV.API.Services;

public interface ICategoryService
{
/// <summary>
/// Получение списка всех категорий
/// </summary>
/// <returns></returns>
    public Task<ResponseData<List<Category>>> GetCategoryListAsync();
}