using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.BookDtos
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(256, ErrorMessage = "Title must not exceed 256 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        public string Description { get; set; } = null!;
        public IFormFile? Picture { get; set; }

        [Required(ErrorMessage = "Release year is required.")]
        [Range(1, 2100, ErrorMessage = "Release year must be valid.")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Page number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be exactly 13 characters.")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or positive.")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "At least one genre must be specified.")]
        [MinLength(1, ErrorMessage = "At least one genre must be specified.")]
        public List<int> GenreIds { get; set; } = [];

        [Required(ErrorMessage = "At least one author must be specified.")]
        [MinLength(1, ErrorMessage = "At least one author must be specified.")]
        public List<int> AuthorIds { get; set; } = [];

        [Required(ErrorMessage = "Publisher is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PublisherId must be greater than 0.")]
        public int PublisherId { get; set; }
    }
}
