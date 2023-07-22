using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly EcommerceDataContext _context;

    public HomeController(ILogger<HomeController> logger, EcommerceDataContext context)
    {
      _logger = logger;
      _context = context;
    }

    public async Task<IActionResult> Index()
    {
      List<Product> products = await _context.Products.ToListAsync();
      var ps = new
      {
        products
      };
      ViewData["Products"] = ps;
      return View(products);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /*public async Task<IActionResult> GetProduct()
    {
      List<Product> products = await _context.Products.ToListAsync();
      var ps = new
      {
        products
      };
      ViewData["Products"] = ps;

      return View("~/Views/Home/index.cshtml");
    }*/
  }
}