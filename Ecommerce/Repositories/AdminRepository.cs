﻿using Ecommerce.Models;
using Ecommerce.ViewModels;
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
  }
}