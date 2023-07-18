using Ecommerce.Models;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<Status> LoginAsync(LoginViewModel model)
    {
      var status = new Status();
      var user = await _userManager.FindByNameAsync(model.Username);
      if (user == null)
      {
        status.StatusCode = 0;
        status.Message = "Invalid username";
        return status;
      }

      if (!await _userManager.CheckPasswordAsync(user, model.Password))
      {
        status.StatusCode = 0;
        status.Message = "Invalid Password";
        return status;
      }

      var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
      if (signInResult.Succeeded)
      {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

        foreach (var userRole in userRoles)
        {
          authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        status.StatusCode = 1;
        status.Message = "Logged in successfully";
      }
      else if (signInResult.IsLockedOut)
      {
        status.StatusCode = 0;
        status.Message = "User is locked out";
      }
      else
      {
        status.StatusCode = 0;
        status.Message = "Error on logging in";
      }

      return status;
    }

    public async Task LogoutAsync()
    {
      await _signInManager.SignOutAsync();
    }

    public async Task<Status> RegistrationAsync(RegistrationViewModel model)
    {
      var status = new Status();
      var userExits = await _userManager.FindByNameAsync(model.Username);
      if (userExits != null)
      {
        status.StatusCode = 0;
        status.Message = "User already exists";
        return status;
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
        status.StatusCode = 0;
        status.Message = "User creation failed";
        return status;
      }

      if (!await _roleManager.RoleExistsAsync(model.Role))
        await _roleManager.CreateAsync(new IdentityRole(model.Role));


      if (await _roleManager.RoleExistsAsync(model.Role))
      {
        await _userManager.AddToRoleAsync(user, model.Role);
      }

      status.StatusCode = 1;
      status.Message = "You have registered successfully";
      return status;
    }


  }
}
