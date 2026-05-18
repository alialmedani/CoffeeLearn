using CoffeeLearn.Application.SizeGroups.DTOs;
using MediatR;

namespace CoffeeLearn.Application.SizeGroups.Queries;

public class GetSizeGroupByIdQuery : IRequest<SizeGroupDto?>
{
	public int Id { get; set; }
}


