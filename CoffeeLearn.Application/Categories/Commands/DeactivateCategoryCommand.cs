using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public record DeactivateCategoryCommand(int Id) : IRequest<CategoryDto?>;




