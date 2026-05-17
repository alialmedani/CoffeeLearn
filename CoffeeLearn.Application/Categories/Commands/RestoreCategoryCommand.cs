using CoffeeLearn.Application.Categories.DTOs;
using MediatR;

namespace CoffeeLearn.Application.Categories.Commands;

public record RestoreCategoryCommand(int Id) : IRequest<CategoryDto?>;