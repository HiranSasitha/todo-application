using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("save-todo-item")]      // create todo items 
        public async Task<IActionResult> CreateTodoItems ([FromBody] TodoItems todoItems)
        {
            try
            {
                _context.TodoItems.Add(todoItems);
               await _context.SaveChangesAsync();
                return Ok(todoItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-all-available-todo-items")]
        public async Task<IActionResult> GetAllTodo()       // get All available-todo-items
        {
            try
            {
                var todo = await _context.TodoItems.Where(todo => todo.IsAvailable).ToListAsync();
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("get-all-available-todo-items-orderby-title")]
        public async Task<IActionResult> GetAllTodoOrderByTitel()       // get All available-todo-items-orderByTitle
        {
            try
            {
                var todo = await _context.TodoItems.Where(todo => todo.IsAvailable).OrderBy(todo => todo.Title).ToListAsync();
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("get-all-unavailable-todo-items")]
        public async Task<IActionResult> GetAllTodoUnAvailable()       // get All unavailable-todo-items
        {
            try
            {

                var todo = await _context.TodoItems.Where(todo => todo.IsAvailable == false).ToListAsync();
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("get-todo-item-by-id/{id}")]

        public async Task<IActionResult> GetTodoById(int id)       // get-todo-items-by-id
        {
            try
            {

                var todo = _context.TodoItems.Find(id);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        [Route("update-todo-item/{id}")]

        public async Task<IActionResult> UpdateTodoItem(int id, [FromBody] TodoItems todoItems)       // update-todo-item
        {
            try
            {

                var todo = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

                if (todo != null) { 

                    
                    todo.Title = todoItems.Title;
                    todo.Description = todoItems.Description;
                    todo.CreateDate = DateTime.Now;


                    _context.TodoItems.Update(todo);
                    await _context.SaveChangesAsync();
                    return Ok(todo);

                }
                else
                {
                    throw new Exception("Invalid id.");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("update-todo-item-completed/{id}")]

        public async Task<IActionResult> UpdateTodoItemToCompleted(int id)       // update-todo-item-completed
        {
            try
            {

                var todo = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

                if (todo != null)
                {

                    todo.IsAvailable = true;
                   
                    todo.CreateDate = DateTime.Now;


                    _context.TodoItems.Update(todo);
                    await _context.SaveChangesAsync();
                    return Ok(todo);

                }
                else
                {
                    throw new Exception("Invalid id.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("update-todo-item-activate/{id}")]

        public async Task<IActionResult> UpdateTodoItemToActive(int id)       // update-todo-item-completed
        {
            try
            {

                var todo = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id == id);

                if (todo != null)
                {

                    todo.IsAvailable = false;

                    todo.CreateDate = DateTime.Now;


                    _context.TodoItems.Update(todo);
                    await _context.SaveChangesAsync();
                    return Ok(todo);

                }
                else
                {
                    throw new Exception("Invalid id.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Route("delete-todo-{id}")]

        public async Task<IActionResult> DeleteTodo(int id)       // delete-todo
        {
            try
            {

                var todo = _context.TodoItems.Find(id);
               
                if (todo != null)
                {
                    _context.TodoItems.Remove(todo);
                    _context.SaveChanges();
                    return Ok(todo);
                }
                else
                {
                    throw new Exception("Invalid id.");
                }
              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("get-all-unavailable-todo-items-orderby-title")]
        public async Task<IActionResult> GetAllUnavailableTodoOrderByTitel()       // get All unavailable-todo-items-orderByTitle
        {
            try
            {
                var todo = await _context.TodoItems.Where(todo => todo.IsAvailable==false).OrderBy(todo => todo.Title).ToListAsync();
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
