using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
namespace WEB_253505_AZAROV.UI.Services;

public interface IProductService
{
    /// <summary>
    /// Получение списка всех объектов
    /// </summary>
    /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
    /// <param name="pageNo">номер страницы списка</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Item>>> GetProductListAsync(string? 
                categoryNormalizedName, int pageNo=1);
    /// <summary>
    /// Поиск объекта по Id
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns>Найденный объект или null, если объект не найден</returns>
    public Task<ResponseData<Item>> GetProductByIdAsync(int id);
    /// <summary>
    /// Обновление объекта
    /// </summary>
    /// <param name="id">Id изменяемомго объекта</param>
    /// <param name="product">объект с новыми параметрами</param>
    /// <param name="formFile">Файл изображения</param>
    /// <returns></returns>
    public Task UpdateProductAsync(int id, Item product, IFormFile? formFile);
    /// <summary>
    /// Удаление объекта
    /// </summary>
    /// <param name="id">Id удаляемомго объекта</param>
    /// <returns></returns>
    public Task DeleteProductAsync(int id);
    /// <summary>
    /// Создание объекта
    /// </summary>
    /// <param name="product">Новый объект</param>
    /// <param name="formFile">Файл изображения</param>
    /// <returns>Созданный объект</returns>
    public Task<ResponseData<Item>> CreateProductAsync(Item product, 
                                            IFormFile? formFile);
}