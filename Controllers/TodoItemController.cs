using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo_Application.Data;
using Todo_Application.Model;

namespace Todo_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public  TodoItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("save-todo-item")]
        public async Task<IActionResult> CreateTodoItems ([FromBody] TodoItems todoItems)
        {
            try
            {
                _context.TodoItems.Add(todoItems);
                _context.SaveChanges();
                return Ok(todoItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
