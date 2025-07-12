using Core.Entities;
using Infrastructure.Data.SeedData.SeedDTOs;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Infrastructure.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!userManager.Users.Any(x => x.UserName == "test"))
            {
                var user = new AppUser
                {
                    UserName = "test",
                    Email = "test@test.com"
                };

                await userManager.CreateAsync(user, "test123");
                await userManager.AddToRoleAsync(user, "User");
            }


            if (!context.Author.Any())
            {
                var authorsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);
                if (authors == null) return;

                context.Author.AddRange(authors);
                await context.SaveChangesAsync();
            }

            if (!context.Genre.Any())
            {
                var genresData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/genres.json");
                var genres = JsonSerializer.Deserialize<List<Genre>>(genresData);
                if (genres == null) return;

                context.Genre.AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!context.Publisher.Any())
            {
                var publishersData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/publishers.json");
                var publishers = JsonSerializer.Deserialize<List<Publisher>>(publishersData);
                if (publishers == null) return;

                context.Publisher.AddRange(publishers);
                await context.SaveChangesAsync();
            }

            if (!context.Book.Any())
            {
                var booksData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/books.json");
                var booksDto = JsonSerializer.Deserialize<List<BookDto>>(booksData);
                if (booksDto == null) return;

                foreach (var bookDto in booksDto)
                {
                    var genres = new List<Genre>();
                    if (bookDto.GenreIds != null)
                    {
                        foreach (var genreId in bookDto.GenreIds)
                        {
                            var genre = await context.Genre.FindAsync(genreId);
                            if (genre != null)
                                genres.Add(genre);
                        }
                    }

                    var authors = new List<Author>();
                    if (bookDto.AuthorIds != null)
                    {
                        foreach (var authorId in bookDto.AuthorIds)
                        {
                            var author = await context.Author.FindAsync(authorId);
                            if (author != null)
                                authors.Add(author);
                        }
                    }

                    var book = new Book
                    {
                        Title = bookDto.Title,
                        Description = bookDto.Description,
                        PictureURL = bookDto.PictureURL,
                        ReleaseYear = bookDto.ReleaseYear,
                        Rating = bookDto.Rating,
                        PageNumber = bookDto.PageNumber,
                        Price = bookDto.Price,
                        ISBN = bookDto.ISBN,
                        QuantityInStock = bookDto.QuantityInStock,
                        PublisherId = bookDto.PublisherId,
                        Genre = genres,
                        Author = authors
                    };

                    context.Book.Add(book);
                }

                await context.SaveChangesAsync();
            }

            if (!context.DeliveryMethod.Any())
            {
                var deliveryData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/deliveryMethods.json");
                var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                if (deliveries == null) return;

                context.DeliveryMethod.AddRange(deliveries);
                await context.SaveChangesAsync();
            }

        }

    }
}
