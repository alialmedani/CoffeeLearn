using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public record DeactivateBrandCommand(int Id) : IRequest<BrandDto?>;




