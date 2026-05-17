using CoffeeLearn.Application.Brands.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Brands.Commands;

public record RestoreBrandCommand(int Id) : IRequest<BrandDto?>;