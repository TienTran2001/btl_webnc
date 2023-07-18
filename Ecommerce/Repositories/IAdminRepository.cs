using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Repositories
{
  public interface IAdminRepository
  {
    Task<List<User>> AdminGetListUsersAsync();
    Task<object> AdminAddUser(RegistrationViewModel model);
  }
}
