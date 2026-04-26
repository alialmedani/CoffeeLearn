using Microsoft.EntityFrameworkCore;
using CoffeeLearn.Domain.Entities;

namespace CoffeeLearn.Application.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Product> Products { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}