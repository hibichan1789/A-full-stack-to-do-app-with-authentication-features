namespace TodoAuthApi.DTOs.TodoDto
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public string? Summary { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
