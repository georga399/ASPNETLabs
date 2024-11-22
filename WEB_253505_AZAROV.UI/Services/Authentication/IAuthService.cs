namespace WEB_253505_AZAROV.UI.Services.Authentication;
public interface IAuthService
{
    Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar);
}