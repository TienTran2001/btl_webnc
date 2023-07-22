using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
  [Table("Products")]
  public class Product
  {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
    [MaxLength(255, ErrorMessage = "Tên sản phẩm không vượt quá 255 ký tự.")]
    public string Name { get; set; }

    [MaxLength]
    public string Description { get; set; }

    [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
    public int Price { get; set; }

    [MaxLength(255)]
    public string Author { get; set; }

    [MaxLength(255)]
    public string Category { get; set; }

    [Required(ErrorMessage = "Số lượng sản phẩm không được để trống.")]
    public int Quantity { get; set; }

    [MaxLength(255)]
    public string ImagePath { get; set; }
  }
}
