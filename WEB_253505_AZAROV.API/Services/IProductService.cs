using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
namespace WEB_253505_AZAROV.API.Services;
public interface IProductService
{
    /// <summary>
    /// Получение списка всех объектов
    /// </summary>
    /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
    /// <param name="pageNo">номер страницы списка</param>
    /// <param name="pageSize">количество объектов на странице</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Item>>> GetProductListAsync(
    string? categoryNormalizedName,int pageNo=1,
    int pageSize=3);
    /// <summary>
    /// Поиск объекта по Id
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns></returns>
    public Task<ResponseData<Item>> GetProductByIdAsync(int id);
    /// <summary>
    /// Обновление объекта
    /// </summary>
    /// <param name="id">Id изменяемомго объекта</param>
    /// <param name="product">объект с новыми параметрами</param>
    /// <returns></returns>
    public Task UpdateProductAsync(int id, Item product);
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
    /// <returns>Созданный объект</returns>
    public Task<ResponseData<Item>> CreateProductAsync(Item product);
    /// <summary>
    /// Сохранить файл изображения для объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="formFile">файл изображения</param>
    /// <returns>Url к файлу изображения</returns
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}