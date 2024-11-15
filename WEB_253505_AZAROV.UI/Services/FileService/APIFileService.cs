using System;

namespace WEB_253505_AZAROV.UI.Services.FileService;

public class APIFileService: IFileService
{
  private readonly HttpClient _httpClient;
    private readonly ILogger<APIFileService> _logger;

    public APIFileService(HttpClient httpClient, ILogger<APIFileService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(IFormFile formFile)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post
        };

        var extension = Path.GetExtension(formFile.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(formFile.OpenReadStream());
        content.Add(streamContent, "file", newName);

        request.Content = content;

        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        _logger.LogError($"-----> File not saved. Error:{response.StatusCode.ToString()}");
        return string.Empty;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var uri = new Uri(_httpClient.BaseAddress?.AbsoluteUri + $"/{fileName.Split('/').Last()}");
        var response = await _httpClient.DeleteAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"-----> File not deleted. Error:{response.StatusCode.ToString()}");
        }
    }
}
