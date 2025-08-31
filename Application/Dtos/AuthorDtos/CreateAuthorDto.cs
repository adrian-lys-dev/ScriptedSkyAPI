using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.AuthorDtos
{
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }
    }
}
