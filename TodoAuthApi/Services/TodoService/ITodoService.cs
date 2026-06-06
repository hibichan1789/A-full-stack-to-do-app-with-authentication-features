using TodoAuthApi.Models;

namespace TodoAuthApi.Services.TodoService
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodoAsync(int userId);
        Task<Todo?> GetTodoByIdAsync(int userId,int id);
        Task<Todo> CreateTodoAsync(int userId, Todo todo);
        Task<Todo> UpdateTodoAsync(int userId, int id, Todo todo);
        Task DeleteTodoAsync(int userId, int id);
    }
}
