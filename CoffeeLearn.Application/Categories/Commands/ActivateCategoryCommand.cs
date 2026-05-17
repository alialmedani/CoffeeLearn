using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public record ActivateCategoryCommand(int Id) : IRequest<CategoryDto?>;