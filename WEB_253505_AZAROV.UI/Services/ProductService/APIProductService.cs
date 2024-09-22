using System.Text;
using System.Text.Json;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.UI.Services;
public class APIProductService : IProductService
{
    private HttpClient _httpClient;
    private string _pageSize;
    private JsonSerializerOptions _serializerOptions;
    private ILogger<APIProductService> _logger;

    public APIProductService(HttpClient httpClient,
        IConfiguration configuration,
        ILogger<APIProductService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value!;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    public async Task<ResponseData<Item>> CreateProductAsync(Item product, IFormFile? formFile)
    {
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

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Item>> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<ListModel<Item>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        // подготовка URL запроса
        var urlString
            = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}items/");
        // добавить категорию в маршрут
        if (categoryNormalizedName != null)
        {
            urlString.Append($"{categoryNormalizedName}/");
        };
        // добавить номер страницы в маршрут
        if (pageNo > 1)
        {
            urlString.Append($"page{pageNo}");
        };
        // добавить размер страницы в строку запроса
        if (!_pageSize!.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize));
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

    public Task UpdateProductAsync(int id, Item product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}