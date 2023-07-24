using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
  public class CartItemController : Controller
  {

    private readonly EcommerceDataContext _context;

    public CartItemController(EcommerceDataContext context)
    {
      _context = context;
     
    }

    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public async Task<IActionResult> GetAllCart() 
    {
      var currentUser = HttpContext.User;
      // Lấy ID của người dùng từ đối tượng ClaimsPrincipal
      var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      //List<CartItem> carts = await _context.CartItems.ToListAsync();
      var carts = _context.CartItems.Where(item => item.UserId == userId).ToList();
      int count = carts.Count;
      return Ok(new
      {
         count,
         carts
      });
    
    }

    [Authorize]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
      var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
      var productCart = await _context.CartItems.FirstOrDefaultAsync(p => p.ProductId == productId);
      var currentUser = HttpContext.User;
      // Lấy ID của người dùng từ đối tượng ClaimsPrincipal
      var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (productCart == null)
      {
        var cartItem = new CartItem
        {
          ProductId = productId,
          ProductName = product.Name,
          Price = product.Price,
          Quantity = quantity,
          ImagePath = product.ImagePath,
          UserId = userId
        };
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
        return Ok(new
        {
          success = true,
          message = "Add successfully"
        });
      }

      productCart.Quantity += quantity;
      await _context.SaveChangesAsync();

      return Ok(new
      {
        success = true,
        message = "Add successfully"
      });
    }

    [Authorize]
    public async Task<IActionResult> UpdateToCart(int cartId, int quantity)
    {
      var cartItem = await _context.CartItems.FirstOrDefaultAsync(p => p.Id == cartId);
      if (cartItem == null)
      {
        return Ok(new { success = false, message = "not found" });
      }

      cartItem.Quantity = quantity;
      await _context.SaveChangesAsync();
      return Ok(new
      {
        success= true,
        cartItem
      });
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteCart(int cartId)
    {
      var cartItem = await _context.CartItems.FirstOrDefaultAsync(p => p.Id == cartId);
      if (cartItem == null)
      {
        return Ok(new { success = false, message = "not found" });
      }

      _context.CartItems.Remove(cartItem);
      await _context.SaveChangesAsync();
      return Ok(new
      {
        success = true,
        message = "Delete successfull"
      }); ;
    }
  }
}
