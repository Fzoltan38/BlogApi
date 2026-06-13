using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTOs
{
    public class AddPostDto
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        [Required]
        public int? BlogId { get; set; }
    }
}
