using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Repositories
{
  public interface IUserRepository
  {
    Task<Status> LoginAsync(LoginViewModel model);
    Task<Status> RegistrationAsync(RegistrationViewModel model);
    Task LogoutAsync();
  }
}
