using Ecommerce.Models;
using Ecommerce.ViewModels;

namespace Ecommerce.Repositories
{
  public interface IAdminRepository
  {
    Task<List<User>> AdminGetListUsersAsync();
    Task<object> AdminAddUser(RegistrationViewModel model);
    Task<object> AdminDeleteUser(string id);
    Task<object> AdminGetUserById(string id);
    Task<object> AdminUpdateUser(string id, string firstName, string lastName, string email, string username);
    Task<object> AdminSearchUserByUsername(string username);
    
    Task<object> AdminGetAllProduct();
    Task<object> AdminAddProduct(Product product, IFormFile image);
    Task<object> AdminGetProductById(int id);
    Task<object> AdminUpdateProduct(int id, string name, string description, int price, string author, string category, int quantity, IFormFile imageFile);
    Task<object> AdminDeleteProduct(int id);

  }
}
