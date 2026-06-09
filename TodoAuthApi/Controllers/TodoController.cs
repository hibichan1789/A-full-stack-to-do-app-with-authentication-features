using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAuthApi.DTOs.TodoDto;
using TodoAuthApi.Models;
using TodoAuthApi.Services.TodoService;

namespace TodoAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult> GetAllTodo()
        {
            var userId = GetUserId();

            var todos = await _todoService.GetAllTodoAsync(userId);

            var todoResponses = todos.Select(t => new TodoResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Summary = t.Summary,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });

            return Ok(todoResponses);
        }

        // GET: api/Todo/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTodoById(int id)
        {
            var userId = GetUserId();
            var todo = await _todoService.GetTodoByIdAsync(userId, id);

            if(todo == null)
            {
                return NotFound();
            }

            return Ok(new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Summary = todo.Summary,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                UpdatedAt = todo.UpdatedAt
            });
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult> CreateTodo(TodoCreateRequest todoCreateRequest)
        {
            var userId = GetUserId();

            var todo = new Todo
            {
                Title = todoCreateRequest.Title,
                Description = todoCreateRequest.Description,
                UserId = userId
            };

            var newTodo = await _todoService.CreateTodoAsync(userId, todo);
            return Ok(new TodoResponse
            {
                Id = newTodo.Id,
                Title = newTodo.Title,
                Description = newTodo.Description,
                Summary= newTodo.Summary,
                IsCompleted = newTodo.IsCompleted,
                CreatedAt = newTodo.CreatedAt,
                UpdatedAt = newTodo.UpdatedAt
            });
        }

        // PUT: api/Todo/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(int id, TodoUpdateRequest todoUpdateRequest)
        {
            var userId = GetUserId();

            var todo = new Todo
            {
                Title = todoUpdateRequest.Title,
                Description = todoUpdateRequest.Description,
                IsCompleted = todoUpdateRequest.IsCompleted
            };

            Todo updatedTodo;
            try
            {
                updatedTodo = await _todoService.UpdateTodoAsync(userId, id, todo);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Problem("サーバーエラー");
            }

            return Ok(new TodoResponse 
            { 
                Id = updatedTodo.Id,
                Title = updatedTodo.Title,
                Description = updatedTodo.Description,
                Summary = updatedTodo.Summary,
                IsCompleted = updatedTodo.IsCompleted,
                CreatedAt = updatedTodo.CreatedAt,
                UpdatedAt = updatedTodo.UpdatedAt
            });
        }

        // DELETE: api/Todo/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var userId = GetUserId();

            try
            {
                await _todoService.DeleteTodoAsync(userId, id);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Problem("サーバーエラー");
            }

            return NoContent();
        }
    }
}
