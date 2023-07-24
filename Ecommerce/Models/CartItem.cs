using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
  public class CartItem
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product Id is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Name is required")]
    public string ProductName { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    public string ImagePath { get; set; }

    [Required(ErrorMessage = "User Id is required")]
    public string UserId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; }

  }
}
