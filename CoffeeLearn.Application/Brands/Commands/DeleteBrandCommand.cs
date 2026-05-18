using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public class DeleteBrandCommand : IRequest<bool>
{
	public int Id { get; set; }
}




