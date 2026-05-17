using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public record ActivateBrandCommand(int Id) : IRequest<BrandDto?>;