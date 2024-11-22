using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NuGet.Common;
using System;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using WEB_253505_AZAROV.UI.HelperClasses;
namespace WEB_253505_AZAROV.UI.Services.Authentication;
public class KeycloakTokenAccessor : ITokenAccessor
{
    private readonly KeycloakData _keycloakData;
    private readonly HttpContext? _httpContext;
    private readonly HttpClient _httpClient;
    public KeycloakTokenAccessor(IOptions<KeycloakData> options,
                                 IHttpContextAccessor httpContextAccessor,
                                 HttpClient httpClient)
    {
        _keycloakData = options.Value;
        _httpContext = httpContextAccessor.HttpContext;
        _httpClient = httpClient;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        if (_httpContext.User.Identity.IsAuthenticated)
        {
            return await _httpContext.GetTokenAsync("access_token");
        }
        var requestUri = $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";
        HttpContent content = new FormUrlEncodedContent([
            new KeyValuePair<string,string>("client_id",_keycloakData.ClientId),
            new KeyValuePair<string,string>("grant_type","client_credentials"),
            new KeyValuePair<string,string>("client_secret",_keycloakData.ClientSecret)
        ]);
        var response = await _httpClient.PostAsync(requestUri, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }
        // extract access token from response
        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonObject.Parse(jsonString)["access_token"].GetValue<string>();
    }
    public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
    {
        string token = await GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders
                  .Authorization = new AuthenticationHeaderValue("bearer", token);
    }
}