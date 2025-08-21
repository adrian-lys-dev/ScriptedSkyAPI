﻿using Domain.Entities;

namespace Application.Dtos.CatalogDtos
{
    public class SingleBookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string PictureURL { get; set; }
        public int ReleaseYear { get; set; }
        public decimal Rating { get; set; }
        public int PageNumber { get; set; }
        public decimal Price { get; set; }
        public required string ISBN { get; set; }
        public int QuantityInStock { get; set; }
        public List<Genre> Genre { get; set; } = [];
        public List<Author> Author { get; set; } = [];
        public Publisher Publisher { get; set; } = null!;
    }
}
