using System.Text;
using System.Text.Json;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
using WEB_253505_AZAROV.UI.Services.FileService;

namespace WEB_253505_AZAROV.UI.Services;
public class APIProductService : IProductService
{
    private HttpClient _httpClient;
    private string _pageSize;
    private JsonSerializerOptions _serializerOptions;
    private ILogger<APIProductService> _logger;
    private readonly IFileService _fileService;

    public APIProductService(HttpClient httpClient,
        IConfiguration configuration,
        ILogger<APIProductService> logger, 
        IFileService fileService)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value!;
        _fileService = fileService;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    public async Task<ResponseData<Item>> CreateProductAsync(Item product, IFormFile? formFile)
    {
        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                product.ImageURI = imageUrl;
            }
        }
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "items/");
        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response
            .Content
            .ReadFromJsonAsync<ResponseData<Item>>
            (_serializerOptions);
            return data!;
        }
        _logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
        return ResponseData<Item>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
    }

    public async Task DeleteProductAsync(int id)
    {
        var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + $"items/{id}");
        var response = await _httpClient.DeleteAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Object not deleted. Error:{response.StatusCode}");
        }
    }

    public async Task<ResponseData<Item>> GetProductByIdAsync(int id)
    {
        var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + $"items/{id}");
        var response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                var responseData = await response.Content
                                     .ReadFromJsonAsync<ResponseData<Item>>(_serializerOptions);
                return responseData!;

            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Error: {ex.Message}");
                return ResponseData<Item>.Error($"Error: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Can't get device with id={id}. Error:{response.StatusCode}");
        return ResponseData<Item>.Error($"Can't get device with id={id}. Error:{response.StatusCode}");
    }

    public async Task<ResponseData<ListModel<Item>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        // подготовка URL запроса
        var urlString
            = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}items/");
        // Добавить категорию в маршрут, если она указана
        if (!string.IsNullOrEmpty(categoryNormalizedName))
        {
            urlString.Append($"{categoryNormalizedName}/");
        }

        // Добавить параметры страницы в строку запроса
        var queryParams = new List<string>();
        
        if (pageNo > 1)
        {
            queryParams.Add($"pageNo={pageNo}");
        }
        
        if (_pageSize != "3") // Используем значение по умолчанию
        {
            queryParams.Add($"pageSize={_pageSize}");
        }
        
        // Если есть параметры, добавляем их к URL
        if (queryParams.Count > 0)
        {
            urlString.Append("?" + string.Join("&", queryParams));
        }
        
        // отправить запрос к API
        var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
        if(response.IsSuccessStatusCode)
        {
            try
            {
                return (await response.Content.
                            ReadFromJsonAsync
                                <ResponseData<ListModel<Item>>>(
                                    _serializerOptions))!;
            }
            catch(JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return ResponseData<ListModel<Item>>
                        .Error($"Ошибка: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        return ResponseData<ListModel<Item>>
            .Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
    }

    public async Task UpdateProductAsync(int id, Item item, IFormFile? formFile)
    {
        if (formFile != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                await _fileService.DeleteFileAsync(item.ImageURI!);
                item.ImageURI = imageUrl;
            }
        }
        var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + $"Items/{id}");
        var response = await _httpClient.PutAsJsonAsync(uri, item);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> Object not updated. Error:{response.StatusCode}");
        }
    }
}