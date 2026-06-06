using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAuthApi.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 200)]
        public String Title { get; set; } = String.Empty;
        public String? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
