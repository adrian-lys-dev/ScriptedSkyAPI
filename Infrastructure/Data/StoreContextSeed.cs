using Core.Entities;
using Core.Entities.User;
using Infrastructure.Data.SeedData.SeedDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            logger.LogInformation("Starting database seeding...");
            logger.LogInformation("Starting database with actual data...");

            await EnsureTriggersExistAsync(context, logger);

            if (!await context.Avatar.AnyAsync())
            {
                var avatarsData = await File.
                    ReadAllTextAsync(path + @"/Data/SeedData/avatars.json");
                var avatars = JsonSerializer.Deserialize<List<Avatar>>(avatarsData);
                if (avatars == null) return;

                context.Avatar.AddRange(avatars);
                await context.SaveChangesAsync();
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            if (!await userManager.Users.AnyAsync(x => x.UserName == "test"))
            {
                var user = new AppUser
                {
                    UserName = "test",
                    Email = "test@test.com",
                    FirstName = "test",
                    LastName = "test",
                };

                await userManager.CreateAsync(user, "test123");
                await userManager.AddToRoleAsync(user, "User");
            }

            if (!await userManager.Users.AnyAsync(x => x.UserName == "crazyReviewer"))
            {
                var user = new AppUser
                {
                    Id = "crazyReviewer-123",
                    UserName = "crazyReviewer",
                    Email = "crazyReviewer@test.com",
                    FirstName = "Crazy",
                    LastName = "Reviewer"
                };

                await userManager.CreateAsync(user, "test123");
                await userManager.AddToRoleAsync(user, "User");
            }

            if (!await context.Author.AnyAsync())
            {
                var authorsData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/authors.json");
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);
                if (authors == null) return;

                context.Author.AddRange(authors);
                await context.SaveChangesAsync();
            }

            if (!await context.Genre.AnyAsync())
            {
                var genresData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/genres.json");
                var genres = JsonSerializer.Deserialize<List<Genre>>(genresData);
                if (genres == null) return;

                context.Genre.AddRange(genres);
                await context.SaveChangesAsync();
            }

            if (!await context.Publisher.AnyAsync())
            {
                var publishersData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/publishers.json");
                var publishers = JsonSerializer.Deserialize<List<Publisher>>(publishersData);
                if (publishers == null) return;

                context.Publisher.AddRange(publishers);
                await context.SaveChangesAsync();
            }

            if (!await context.Book.AnyAsync())
            {
                var booksData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/books.json");
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

            if (!await context.DeliveryMethod.AnyAsync())
            {
                var deliveryData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/deliveryMethods.json");
                var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                if (deliveries == null) return;

                context.DeliveryMethod.AddRange(deliveries);
                await context.SaveChangesAsync();
            }

            if (!await context.Review.AnyAsync())
            {
                var reviewData = await File
                    .ReadAllTextAsync(path + @"/Data/SeedData/reviews.json");
                var reviews = JsonSerializer.Deserialize<List<Review>>(reviewData);
                if (reviews == null) return;

                context.Review.AddRange(reviews);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("Database seeding completed.");
        }

        private static async Task EnsureTriggersExistAsync(StoreContext context, ILogger logger)
        {
            logger.LogInformation("Checking triggers...");
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var triggerDir = path + @"/Data/SeedData/TriggersSQL";

            if (!Directory.Exists(triggerDir))
            {
                logger.LogWarning("Trigger directory not found: {TriggerDir}", triggerDir);
                return;
            }

            var sqlFiles = Directory.GetFiles(triggerDir, "*.sql");

            foreach (var file in sqlFiles)
            {
                var triggerName = Path.GetFileNameWithoutExtension(file);
                logger.LogInformation("Checking trigger '{TriggerName}'...", triggerName);

                var sqlBody = await File.ReadAllTextAsync(file);

                var wrappedSql = $@"
                    IF NOT EXISTS (
                        SELECT * FROM sys.triggers WHERE name = N'{triggerName}'
                    )
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX);
                        SET @sql = N'{sqlBody.Replace("'", "''")}';
                        EXEC(@sql);
                        PRINT 'Trigger {triggerName} created.';
                    END
                    ELSE
                    BEGIN
                        PRINT 'Trigger {triggerName} already exists.';
                    END
                ";

                await context.Database.ExecuteSqlRawAsync(wrappedSql);

                logger.LogInformation("Processed trigger '{TriggerName}'.", triggerName);
            }

            logger.LogInformation("Trigger check completed.");
        }
    }

    /// <summary>
    /// Catergory looger for StoreContextSeed.
    /// Used only for grouping logs.
    /// </summary>
    [SuppressMessage("SonarLint", "S2094", Justification = "Marker class for ILogger category")]
    public class StoreContextSeedLoggerCategory { }

}
