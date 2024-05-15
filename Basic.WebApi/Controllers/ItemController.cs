using Basic.Domain.Interfaces;
using Basic.Domain.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Item> GetAllPersons()
        {
            return unitOfWork.Item.GetAll();
        }

        [HttpGet("{id}")]
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

        [HttpPut]
        public async Task<IActionResult> UpdateItem(Item item)
        {
            unitOfWork.Item.Update(item);
            unitOfWork.Save();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveItem(Item item)
        {
            unitOfWork.Item.Remove(item);
            unitOfWork.Save();
            return Ok();
        }
    }
}
