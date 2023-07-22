using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
  public class ProductController : Controller
  {
    private readonly EcommerceDataContext _context;

    public ProductController(EcommerceDataContext context)
    {
      _context = context;
    }
    /* public IActionResult ()
     {
       return View();
     }*/
    public async Task<ActionResult> Detail(int id)
    {
      var product = await _context.Products.FindAsync(id);
      return View(product);
    }
  }
}
