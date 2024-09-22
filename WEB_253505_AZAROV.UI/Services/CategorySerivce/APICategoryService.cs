using System.Text;
using System.Text.Json;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.UI.Services;

public class APICategoryService : ICategoryService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    private ILogger<APICategoryService> _logger;

    public APICategoryService(HttpClient httpClient,
        IConfiguration configuration,
        ILogger<APICategoryService> logger)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var urlString
            = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}categories/");
        var response = await _httpClient.GetAsync(
            new Uri(urlString.ToString()));
        if(response.IsSuccessStatusCode)
        {
            try
            {
                return (await response.Content.
                            ReadFromJsonAsync
                                <ResponseData<List<Category>>>(
                                    _serializerOptions))!;
            }
            catch(JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return ResponseData<List<Category>>
                        .Error($"Ошибка: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        return ResponseData<List<Category>>
            .Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
    }
}