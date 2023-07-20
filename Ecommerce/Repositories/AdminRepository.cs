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

    public AdminRepository(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;
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

  }
}
