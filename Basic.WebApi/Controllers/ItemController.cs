﻿using Basic.Domain.Interfaces;
using Basic.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Basic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public ItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<Item> GetAllPersons()
        {
            var currentUser = GetCurrentUser();
            return unitOfWork.Item.GetAll();
        }
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"{currentUser.Username} , is {currentUser.Role}");
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetItemById(int id)
        {

            var item = unitOfWork.Item.GetById(id);
            if (item == null)
            {
                return NotFound(); // Returns a 404 Not Found response if the item with the specified ID is not found
            }
            return Ok(item); // Returns a 200 OK response with the item data if found
        }

        [HttpPost]
        public async Task<IActionResult> EnterItem(Item item)
        {
            unitOfWork.Item.Add(item);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPost("AddItem")]
        [Authorize]
        public async Task<IActionResult> AddItem(ItemCategory item)
        {
            Item getItem = new Item();
            Category category = new Category();
            category.CategoryName = item.CategoryName;
            getItem.Category = category;
            getItem.ItemName = item.ItemName;
            getItem.ItemQuantity = item.ItemQuantity;
            getItem.ItemUnit = item.ItemUnit;
            unitOfWork.Item.Add(getItem);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateItem(Item item)
        {
            unitOfWork.Item.Update(item);
            unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("RemoveItem")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            Item getItem = new Item();
            getItem.Id = itemId;
            unitOfWork.Item.Remove(getItem);
            unitOfWork.Save();
            return Ok();
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    //GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    //Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
