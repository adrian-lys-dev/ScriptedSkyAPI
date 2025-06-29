using Core.Entities;
using Core.Entities.Base;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(BookConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(GenreConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(AuthorConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(PublisherConfiguration).Assembly);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var createdAtProperty = entityType.FindProperty(nameof(BaseEntity.CreatedAt));
                var updatedAtProperty = entityType.FindProperty(nameof(BaseEntity.UpdatedAt));

                if (createdAtProperty != null)
                {
                    createdAtProperty.SetDefaultValueSql("GETUTCDATE()");
                }

                if (updatedAtProperty != null)
                {
                    updatedAtProperty.SetDefaultValueSql("GETUTCDATE()");
                }
            }

        }
    }
}
