using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
namespace WEB_253505_AZAROV.BlazorWASM.Services;
internal class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly string _pageSize;
    public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _accessTokenProvider = accessTokenProvider;
    }
    private async Task<string> GetJwtTokenAsync()
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out var token))
        {
            return token.Value;
        }
        throw new Exception("Не удалось получить токен");
    }
    public List<Category> Categories { get; set; }
    public List<Item> Items { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public Category SelectedCategory { get; set; } = null;
    public event Action DataLoaded;
    public async Task GetCategoryListAsync()
    {
        var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}Categories";
        try
        {
            var response = await _httpClient.GetAsync(new Uri(urlString));
            if (!response.IsSuccessStatusCode)
            {
                Success = false;
                ErrorMessage = $"Error occured in fetching data: {response.StatusCode.ToString()}";
            }
            var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_jsonSerializerOptions);
            if (!data.Successfull)
            {
                Success = false;
                ErrorMessage = data.ErrorMessage;
            }
            Success = true;
            Categories = data.Data;
            DataLoaded.Invoke();
        }
        catch (Exception ex)
        {
            Success = false;
            ErrorMessage = $"Error occured in http client: {ex.Message}";
        }
    }
    public async Task GetProductListAsync(int pageNo = 1)
    {
        try
        {
            var route = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}items/");
            if (SelectedCategory is not null)
            {
                route.Append($"{SelectedCategory.NormalizedName}/");
            }
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
                route.Append("?" + string.Join("&", queryParams));
            }

            var token = await GetJwtTokenAsync();
        
            var request = new HttpRequestMessage(HttpMethod.Get, route.ToString());
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Success = false;
                ErrorMessage = $"Error occured in fetching data: {response.StatusCode.ToString()}";
            }
            var data = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Item>>>(_jsonSerializerOptions);
            if (!data.Successfull)
            {
                Success = false;
                ErrorMessage = data.ErrorMessage;
            }
            Success = true;
            Items = data.Data.Items;
            CurrentPage = data.Data.CurrentPage;
            TotalPages = data.Data.TotalPages;
            DataLoaded.Invoke();
        }
        catch (Exception ex) {
            Success = false;
            ErrorMessage = $"Error occured in http client: {ex.Message}";
        }
    }
}