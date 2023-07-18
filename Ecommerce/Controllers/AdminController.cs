using Ecommerce.Repositories;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
  public class AdminController : Controller
  {

    private readonly IAdminRepository _adminRepository;
    public AdminController(IAdminRepository adminRepository)
    {
      _adminRepository = adminRepository;
    }

    [Authorize(Roles = "admin")]
    public IActionResult Display()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
      var users = await _adminRepository.AdminGetListUsersAsync();
      return Ok(users);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddUser(RegistrationViewModel model)
    {
     
      if (!ModelState.IsValid) { return View(model); }
      model.Role = "user";
      var result = await _adminRepository.AdminAddUser(model);
      //TempData["msg"] = result.Message;
      //return Ok(result);
      return RedirectToAction("Display", "Dashboard");
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteUser(string id)
    {
      var result = await _adminRepository.AdminDeleteUser(id);
      return Ok(result);
    }

  }
}
