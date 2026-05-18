using CoffeeLearn.Application.Reports.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Reports.Queries;

public class GetLowStockVariantsQuery : IRequest<List<LowStockVariantDto>>
{
	public int Threshold { get; set; } = 5;
	public bool OnlyActive { get; set; } = true;
}




