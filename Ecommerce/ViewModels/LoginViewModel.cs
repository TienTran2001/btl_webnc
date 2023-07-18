using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
