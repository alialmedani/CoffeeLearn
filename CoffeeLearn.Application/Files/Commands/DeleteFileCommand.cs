using MediatR;

namespace CoffeeLearn.Application.Files.Commands;

public class DeleteFileCommand : IRequest<bool>
{
	public int Id { get; set; }

	public DeleteFileCommand(int id)
	{
		Id = id;
	}
}




