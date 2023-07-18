using Ecommerce.Repositories;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
  public class UserController : Controller
  {
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public IActionResult Display()
    {
      return View();
    }

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await _userRepository.LoginAsync(model);
      if (result.StatusCode == 1 || User.IsInRole("admin"))
      {
        return RedirectToAction("Index", "Home");
      }
      else
      {
        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(Login));
      }

    }

    [HttpPost]

    public IActionResult Registration()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationViewModel model)
    {
      if (!ModelState.IsValid) { return View(model); }
      model.Role = "user";
      var result = await _userRepository.RegistrationAsync(model);
      TempData["msg"] = result.Message;
      return RedirectToAction(nameof(Registration));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
      await _userRepository.LogoutAsync();
      return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> RegisterAdmin()
    {
      RegistrationViewModel model = new RegistrationViewModel
      {
        Username = "Admin",
        Email = "admin@gmail.com",
        FirstName = "Tien",
        LastName = "Tran",
        Password = "Admin@123456"
      };

      model.Role = "admin";
      var result = await _userRepository.RegistrationAsync(model);
      return Ok(result);
    }

  }

}



