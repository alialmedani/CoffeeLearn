using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Product> Products { get; }
	DbSet<ProductVariant> ProductVariants { get; }

	DbSet<Order> Orders { get; }
	DbSet<OrderItem> OrderItems { get; }

	DbSet<UploadedFile> UploadedFiles { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}