using Basic.Domain.Interfaces;
using Basic.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Basic.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Category> GetAllCategory()
        {
            var currentUser = GetCurrentUser();
            return unitOfWork.Category.GetAll();
        }
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"{currentUser.Username} , is {currentUser.Role}");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCategoryById(int id)
        {

            var Category = unitOfWork.Category.GetById(id);
            if (Category == null)
            {
                return NotFound(); // Returns a 404 Not Found response if the item with the specified ID is not found
            }
            return Ok(Category); // Returns a 200 OK response with the item data if found
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EnterItem(Category category)
        {
            unitOfWork.Category.Add(category);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem(Category category)
        {
            unitOfWork.Category.Update(category);
            unitOfWork.Save();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveItem(Category category)
        {
            unitOfWork.Category.Remove(category);
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
