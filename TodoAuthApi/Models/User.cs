using System.ComponentModel.DataAnnotations;

namespace TodoAuthApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public String UserName { get; set; } = String.Empty;

        [Required]
        [StringLength(maximumLength: 200)]
        public String Email { get; set; } = String.Empty;

        [Required]
        [StringLength(maximumLength: 500)]
        public String PasswordHash { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
