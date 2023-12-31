﻿using Ecommerce.Models;
using Ecommerce.Repositories;
using Ecommerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Ecommerce.Controllers
{
  [Authorize(Roles = "admin")]
  public class AdminController : Controller
  {

    private readonly IAdminRepository _adminRepository;


    public AdminController(IAdminRepository adminRepository)
    {
      _adminRepository = adminRepository;
    }

    
    public IActionResult Display()
    {
      return View();
    }


    
    [HttpGet]
    public async Task<IActionResult> GetAllUser()
    {
      var users = await _adminRepository.AdminGetListUsersAsync();
      return Ok(users);
    }

   
    public async Task<IActionResult> AddUser(RegistrationViewModel model)
    {
     
      if (!ModelState.IsValid) { return View(model); }
      model.Role = "user";
      var result = await _adminRepository.AdminAddUser(model);
      //TempData["msg"] = result.Message;
      //return Ok(result);
      return RedirectToAction("Display", "Dashboard");
    }

   
    public async Task<IActionResult> DeleteUser(string id)
    {
      var result = await _adminRepository.AdminDeleteUser(id);
      return Ok(result);
    }

    
    public async Task<IActionResult> FindUserById(string id)
    {
      var result = await _adminRepository.AdminGetUserById(id);
      return Ok(result);
    }

    public async Task<IActionResult> UpdateUser(string id, string firstName, string lastName, string email, string username)
    {
      var result = await _adminRepository.AdminUpdateUser(id, firstName, lastName, email, username);
      
      return Ok(result);
    }

    public async Task<IActionResult> SearchUser(string username)
    {
      var result = await _adminRepository.AdminSearchUserByUsername(username);
      return Ok(result);
    }


    /* Product */

    public async Task<IActionResult> GetAllProducts()
    {
      var result = await _adminRepository.AdminGetAllProduct();
      return Ok(result);
    }


    public IActionResult AddProduct ()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product, IFormFile image)
    {
      //if (!ModelState.IsValid) { return View(product); }
      var result = await _adminRepository.AdminAddProduct(product, image);
      return RedirectToAction("Products", "Dashboard");
    }

    public async Task<IActionResult> GetProductById(int id)
    {
      var result = await _adminRepository.AdminGetProductById(id);

      return Ok(result);
    }

    public async Task<IActionResult> UpdateProduct(int id, string name, string description, int price, string author, string category, int quantity, IFormFile imageFile)
    {
      var result = await _adminRepository.AdminUpdateProduct(id,  name,  description,  price,  author,  category,  quantity,  imageFile);
      return Ok(result);
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
      var result = await _adminRepository.AdminDeleteProduct(id);
      return Ok(result);
    }
  }
}
