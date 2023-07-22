using Ecommerce.Repositories;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace Ecommerce.Controllers
{

  //[Authorize]
  [Authorize(Roles = "admin")]
  public class DashboardController : Controller
  {
    public IActionResult Display()
    {
      return View();
    }

    public IActionResult Products()
    {
      return View();
    }
  }

}
