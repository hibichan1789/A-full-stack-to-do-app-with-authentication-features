using System.ComponentModel.DataAnnotations;

namespace TodoAuthApi.DTOs.TodoDto
{
    public class TodoCreateRequest
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
    }
}
