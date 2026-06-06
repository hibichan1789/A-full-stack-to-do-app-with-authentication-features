using System.ComponentModel.DataAnnotations;

namespace TodoAuthApi.DTOs.TodoDto
{
    public class TodoUpdateRequest
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
