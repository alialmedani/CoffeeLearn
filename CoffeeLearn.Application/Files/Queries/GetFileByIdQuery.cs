using MediatR;
using CoffeeLearn.Application.Files.DTOs;

namespace CoffeeLearn.Application.Files.Queries;

public class GetFileByIdQuery : IRequest<UploadedFileDto?>
{
	public int Id { get; set; }

	public GetFileByIdQuery(int id)
	{
		Id = id;
	}
}




