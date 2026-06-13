using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTOs
{
    public class AddBloggerDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
