using FluentValidation;
using CoffeeLearn.Domain.Enums;

namespace CoffeeLearn.Application.Files.Commands;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
	private static readonly string[] ImageContentTypes =
	{
		"image/jpeg",
		"image/png",
		"image/webp",
		"image/svg+xml"
	};

	private static readonly string[] ImageExtensions =
	{
		".jpg",
		".jpeg",
		".png",
		".webp",
		".svg"
	};

	private static readonly string[] AttachmentContentTypes =
	{
		"application/pdf",
		"text/plain",

		"image/jpeg",
		"image/png",
		"image/webp",
		"image/svg+xml",

		"application/msword",
		"application/vnd.openxmlformats-officedocument.wordprocessingml.document",

		"application/vnd.ms-excel",
		"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
	};

	private static readonly string[] AttachmentExtensions =
	{
		".pdf",
		".txt",

		".jpg",
		".jpeg",
		".png",
		".webp",
		".svg",

		".doc",
		".docx",

		".xls",
		".xlsx"
	};

	private const long MaxImageSize = 5 * 1024 * 1024;
	private const long MaxAttachmentSize = 10 * 1024 * 1024;

	public UploadFileCommandValidator()
	{
		RuleFor(x => x.EntityId)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.EntityType)
			.NotEqual(FileEntityType.Unknown);

		RuleFor(x => x.FilePlacement)
			.NotEqual(FilePlacement.Unknown);

		RuleFor(x => x.OriginalFileName)
			.NotEmpty()
			.MaximumLength(255);

		RuleFor(x => x.ContentType)
			.NotEmpty();

		RuleFor(x => x.FileSize)
			.GreaterThan(0);

		When(x => x.FilePlacement == FilePlacement.MainImage || x.FilePlacement == FilePlacement.Gallery, () =>
		{
			RuleFor(x => x.ContentType)
				.Must(BeAllowedImageContentType)
				.WithMessage("Only image content types are allowed for MainImage and Gallery.");

			RuleFor(x => x.OriginalFileName)
				.Must(HaveAllowedImageExtension)
				.WithMessage("Only .jpg, .jpeg, .png, .webp, and .svg files are allowed for MainImage and Gallery.");

			RuleFor(x => x.FileSize)
				.LessThanOrEqualTo(MaxImageSize)
				.WithMessage("Image file size must not exceed 5 MB.");
		});

		When(x => x.FilePlacement == FilePlacement.Attachment || x.FilePlacement == FilePlacement.Document, () =>
		{
			RuleFor(x => x.ContentType)
				.Must(BeAllowedAttachmentContentType)
				.WithMessage("Only PDF, text, image, Word, and Excel files are allowed for Attachment and Document.");

			RuleFor(x => x.OriginalFileName)
				.Must(HaveAllowedAttachmentExtension)
				.WithMessage("Only .pdf, .txt, .jpg, .jpeg, .png, .webp, .svg, .doc, .docx, .xls, and .xlsx files are allowed.");

			RuleFor(x => x.FileSize)
				.LessThanOrEqualTo(MaxAttachmentSize)
				.WithMessage("Attachment file size must not exceed 10 MB.");
		});
	}

	private static bool BeAllowedImageContentType(string contentType)
	{
		return ImageContentTypes.Contains(contentType.ToLower());
	}

	private static bool HaveAllowedImageExtension(string fileName)
	{
		var extension = Path.GetExtension(fileName).ToLower();

		return ImageExtensions.Contains(extension);
	}

	private static bool BeAllowedAttachmentContentType(string contentType)
	{
		return AttachmentContentTypes.Contains(contentType.ToLower());
	}

	private static bool HaveAllowedAttachmentExtension(string fileName)
	{
		var extension = Path.GetExtension(fileName).ToLower();

		return AttachmentExtensions.Contains(extension);
	}
}





