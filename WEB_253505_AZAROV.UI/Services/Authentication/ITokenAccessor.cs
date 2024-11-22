namespace WEB_253505_AZAROV.UI.Services;
public interface ITokenAccessor
{
    Task<string> GetAccessTokenAsync();
    Task SetAuthorizationHeaderAsync(HttpClient httpClient);
}