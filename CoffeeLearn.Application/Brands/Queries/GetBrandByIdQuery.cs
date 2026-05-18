using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Queries;

public class GetBrandByIdQuery : IRequest<BrandDto?>
{
	public int Id { get; set; }
}





