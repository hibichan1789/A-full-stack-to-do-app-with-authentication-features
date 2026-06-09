using Microsoft.EntityFrameworkCore;
using TodoAuthApi.Context;
using TodoAuthApi.Models;
using TodoAuthApi.Services.SummaryService;

namespace TodoAuthApi.Services.TodoService
{
    public class TodoService: ITodoService
    {
        private readonly MyContext _db;
        private readonly ISummaryService _summaryService;
        public TodoService(MyContext db, ISummaryService summaryService)
        {
            _db = db;
            _summaryService = summaryService;
        }

        public async Task<IEnumerable<Todo>> GetAllTodoAsync(int userId)
        {
            return await _db.Todos
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Todo?> GetTodoByIdAsync(int userId, int id)
        {
            return await _db.Todos
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);
        }

        public async Task<Todo> CreateTodoAsync(int userId,Todo todo)
        {
            todo.IsCompleted = false;
            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = DateTime.UtcNow;
            todo.UserId = userId;

            
            if(String.IsNullOrWhiteSpace(todo.Description))
            {
                todo.Summary = string.Empty;
            }
            else
            {
                todo.Summary = await _summaryService.GenerateSummaryAsync(todo.Description);
            }
            
            
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> UpdateTodoAsync(int userId, int id, Todo todo)
        {
            var targetTodo = await GetTodoByIdAsync(userId, id);

            if(targetTodo == null)
            {
                throw new InvalidOperationException("Todo Not Found");
            }

            targetTodo.Title = todo.Title;
            targetTodo.Description = todo.Description;
            targetTodo.IsCompleted = todo.IsCompleted;
            targetTodo.UpdatedAt = DateTime.UtcNow;

            if (String.IsNullOrWhiteSpace(targetTodo.Description))
            {
                targetTodo.Summary = string.Empty;
            }
            else
            {
                targetTodo.Summary = await _summaryService.GenerateSummaryAsync(targetTodo.Description);
            }
            await _db.SaveChangesAsync();
            return targetTodo;
        }

        public async Task DeleteTodoAsync(int userId, int id)
        {
            var targetTodo = await GetTodoByIdAsync(userId, id);

            if(targetTodo == null)
            {
                throw new InvalidOperationException("Todo Not Found");
            }

            _db.Todos.Remove(targetTodo);
            await _db.SaveChangesAsync();
        }


    }
}
