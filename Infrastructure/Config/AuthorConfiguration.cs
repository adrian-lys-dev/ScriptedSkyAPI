﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(p => p.Id).
                IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
