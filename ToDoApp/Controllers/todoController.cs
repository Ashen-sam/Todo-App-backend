using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public ToDoController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

       
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<List<Todo>>> AddTitle(Todo newTodo)
        {
            if (newTodo != null)
            {
                newTodo.IsCompleted = false;

                appDbContext.TodoApp.Add(newTodo);
                await appDbContext.SaveChangesAsync();
                return Ok(newTodo);
            }
            return BadRequest("Todo object is null.");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<Todo>>> GetTitle()
        {
            var titles = await appDbContext.TodoApp.ToListAsync();
            return Ok(titles);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        public async Task<ActionResult<Todo>> GetTitle(int id)
        {
            var todo = await appDbContext.TodoApp.FirstOrDefaultAsync(e => e.Id == id);
            if (todo != null)
            {
                return Ok(todo);
            }
            return NotFound();
        }

        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<Todo>> UpdateTitle(Todo updatedTodo)
        {
            if (updatedTodo != null)
            {
                var todoToUpdate = await appDbContext.TodoApp.FirstOrDefaultAsync(e => e.Id == updatedTodo.Id);
                if (todoToUpdate != null)
                {
                    
                    todoToUpdate.Title = updatedTodo.Title;
                    todoToUpdate.Description = updatedTodo.Description; 
                    todoToUpdate.IsCompleted = updatedTodo.IsCompleted;

                    await appDbContext.SaveChangesAsync();

                    return Ok(todoToUpdate);
                }
                return NotFound(); 
            }
            return BadRequest(); 
        }

        [HttpDelete]
        [Produces("application/json")]
        public async Task<ActionResult<List<Todo>>> DeleteTitle(int id)
        {
            var todoToDelete = await appDbContext.TodoApp.FirstOrDefaultAsync(e => e.Id == id);
            if (todoToDelete != null)
            {
                appDbContext.TodoApp.Remove(todoToDelete);
                await appDbContext.SaveChangesAsync();

                var remainingTodos = await appDbContext.TodoApp.ToListAsync();
                return Ok(remainingTodos);
            }
            return BadRequest("The specified Todo item does not exist.");
        }


    }
}
