using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    //[Table("users")]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        //    [Key]
        //    [Column("id")]
        //    public int Id { get; set; }

        //    [Required(ErrorMessage = "UserName is required.")]
        //    [StringLength(100)]
        //    [Column("userName")]
        //    public string UserName { get; set; } = "";

        //    [StringLength(255)]
        //    [Column("fullName")]
        //    public string FullName { get; set; } = "";

        //    [Required(ErrorMessage = "Email is required.")]
        //    [EmailAddress(ErrorMessage = "Invalid email address.")]
        //    [StringLength(100)]
        //    [Column("email")]
        //    public string Email { get; set; } = "";

        //    [Required(ErrorMessage = "Mobile is required.")]
        //    [StringLength(20)]
        //    [RegularExpression(@"^\+?\d{10}$", ErrorMessage = "Invalid phone number.")]
        //    [Column("mobile")]
        //    public string Mobile { get; set; } = "";

        //    [Required(ErrorMessage = "Password is required.")]
        //    [StringLength(50)]
        //    [Column("password")]
        //    public string Password { get; set; } = "";

        //    [StringLength(20)]
        //    [Column("role")]
        //    public string Role { get; set; } = "user";
        //}
    }
}
