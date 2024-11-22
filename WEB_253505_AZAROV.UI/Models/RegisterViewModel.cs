using System.ComponentModel.DataAnnotations;
namespace WEB_253505_AZAROV.UI.Models;
public class RegisterUserViewModel
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; }= null!;
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }= null!;
    public IFormFile? Avatar { get; set; }
}