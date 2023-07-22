using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Repositories
{
  public class AdminRepository : IAdminRepository
  {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly EcommerceDataContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public AdminRepository(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, EcommerceDataContext context, IWebHostEnvironment hostingEnvironment)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;
      _context = context;
      _hostingEnvironment = hostingEnvironment;
    }

    public async Task<List<User>> AdminGetListUsersAsync()
    {
      var users = await _userManager.Users.Select(u => new User
      {
        Id = u.Id,
        LastName = u.LastName,
        FirstName = u.FirstName,
        UserName = u.UserName,
        Email = u.Email
      }).ToListAsync();
      return users;
    }

    public async Task<object> AdminAddUser(RegistrationViewModel model)
    {
      
      var userExits = await _userManager.FindByNameAsync(model.Username);
      if (userExits != null)
      {
        return new
        {
          StatusCode = 0,
          Message = "User already exists!",
        };
      }

      User user = new User
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        FirstName = model.FirstName,
        LastName = model.LastName,
        UserName = model.Username,
        Email = model.Email,
        EmailConfirmed = true
      };

      var result = await _userManager.CreateAsync(user, model.Password);
      if (!result.Succeeded)
      {        
        return new
        {
          StatusCode = 0,
          Message = "User creation failed!",
        };
      }

      if (!await _roleManager.RoleExistsAsync(model.Role))
        await _roleManager.CreateAsync(new IdentityRole(model.Role));


      if (await _roleManager.RoleExistsAsync(model.Role))
      {
        await _userManager.AddToRoleAsync(user, model.Role);
      }

      return new
      {
        StatusCode = 1,
        Message = "You have registered successfully!",
        NewUser = user
      };
    }


    public async Task<object> AdminDeleteUser (string id)
    {
      var user = await _userManager.FindByIdAsync(id);

      if (user != null)
      {
        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
          // Xóa thành công
          return new
          {
            StatusCode = 1,
            Message = "Deleted user successfuly!!"
          };
        }

        return new
        {
          StatusCode = 0,
          Message = "Deleted user fail!!"
        };
      }
      return new
      {
        StatusCode = 0,
        Message = "User does not exist!!"
      };
    }

    public async Task<object> AdminGetUserById(string id)
    {
      var user = await _userManager.FindByIdAsync(id);

      if (user == null)
      {
        return new
        {
          StatusCode = 0,
          Message = "User not found!"
        };
      }
      return new
      {
        StatusCode = 1,
        User = user
      };
    }

    public async Task<object> AdminUpdateUser(string id, string firstName, string lastName, string email, string username)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null)
      {
        return new
        {
          StatusCode = 0,
          Message = "User not found"
        };
      }

      // Cập nhật thông tin mới
      user.FirstName = firstName;
      user.LastName = lastName;
      user.Email = email;
      user.UserName = username;

      // Lưu cập nhật vào cơ sở dữ liệu
      var result = await _userManager.UpdateAsync(user);
      if (!result.Succeeded)
      {
        return new {
          StatusCode = 0,
          Message = "Upfated User fail"
        };
      }
      return new
      {
        StatusCode = 1,
        User = user
      };
    }

    /* Search User by username */
    public async Task<object> AdminSearchUserByUsername(string username)
    {
      var users = _userManager.Users
            .Where(u => u.UserName.Contains(username))
            .ToList();
      if (users == null)
      {
        return new
        {
          StatusCode = 0,
          Message = "Upfated User fail" 
        };
      }

      return new
      {
        StatusCode = 1,
        users
      };
    }


    /* Product */
    public async Task<object> AdminGetAllProduct()
    {
      var products = await _context.Products.ToListAsync();
      if (products == null)
      {
        return new
        {
          success = false,
          message = "Not found"
        };
      }
      return new
      {
        success = true,
        products 
      };
    }

    public async Task<object> AdminAddProduct(Product product, IFormFile image)
    {
      if (image != null && image.Length > 0)
      {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();


        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(imagePath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          await image.CopyToAsync(fileStream);
        }

        // Lưu đường dẫn ảnh vào database
        product.ImagePath = fileName;
        await _context.SaveChangesAsync();
      }

      return new 
      { 
        success = true,
        product
      };
    }

    public async Task<object> AdminGetProductById(int id)
    {
      var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

      if (product == null)
      {
        return new
        {
          success = false,
          message = "User not found!"
        };
      }
      return new
      {
        success = 1,
        product
      };
    }

    public async Task<object> AdminUpdateProduct(int id, string name, string description, int price, string author, string category, int quantity, IFormFile imageFile)
    {

      var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
      if (product == null)
      {
        return new
        {
          success = false,
          message = "Product not found"
        };
      }

      product.Name = name;
      product.Description = description;
      product.Price = price;
      product.Author = author;
      product.Category = category;
      product.Quantity = quantity;

      if(imageFile != null)
      {
        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
        var filePath = Path.Combine(imagePath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
          await imageFile.CopyToAsync(fileStream);
        }
        product.ImagePath = fileName;
      }

      await _context.SaveChangesAsync();

      return new
      {
        success = true,
        product
      };
    }


    public async Task<object> AdminDeleteProduct(int id)
    {
      //var user = await _userManager.FindByIdAsync(id);
      var product = _context.Products.FirstOrDefault(p => p.Id == id);

      if (product == null)
      {
          return new
        {
          success = false,
          message = "User not found!!"
        };
      }

      var result = _context.Products.Remove(product);
      await _context.SaveChangesAsync();
      // Xóa thành công
      return new
      {
        success = 1,
        message = "Deleted product successfuly!!"
      };
    }



  }
}
