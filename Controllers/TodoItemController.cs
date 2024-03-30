﻿using Microsoft.AspNetCore.Http;
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

        public async Task<IActionResult> GetTodoById(int id)       // get -todo-items-by-id
        {
            try
            {

                var todo = await _context.TodoItems.FirstOrDefaultAsync(todo => todo.Id==id);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
