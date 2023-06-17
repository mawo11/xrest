using XRest.Images.App.Domain;
using XRest.Images.App.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XRest.Images.App.Queries.GetImage;

public sealed class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageData>
{
	private readonly IImageRepository _imageRepository;
	private readonly IConfiguration _configuration;
	private readonly ILogger<GetImageQueryHandler> _logger;

	private static readonly Dictionary<string, string> Extensions = new()
	{
		["image/jpeg"] = ".jpg",
		["image/png"] = ".png"
	};

	public GetImageQueryHandler(IImageRepository imageRepository, IConfiguration configuration, ILogger<GetImageQueryHandler> logger)
	{
		_imageRepository = imageRepository;
		_configuration = configuration;
		_logger = logger;
	}

	public async Task<ImageData> Handle(GetImageQuery request, CancellationToken cancellationToken)
	{
		var result = await _imageRepository.GetImageMimeAsync(request.ImageId, request.ImageType);
		var imageDirectory = _configuration.GetValue<string>("Media:ImagesDirectory");

		if (!string.IsNullOrEmpty(result) && Extensions.ContainsKey(result))
		{
			ImageData? imageData = null;
			if (request.ImageType == ImageType.Product)
			{
				imageData = new ImageData(
					 result,
					 $"product{request.ImageId}{Extensions[result]}",
					 Path.Combine(imageDirectory, ((int)request.ImageType).ToString(), request.ImageId.ToString()));
			}

			if (imageData != null && File.Exists(imageData.FullPath))
			{
				return imageData;
			}
		}

		_logger.LogError("Brak pliku: id: {ImageId} type: {ImageType}", request.ImageId, request.ImageType);

		return new ImageData(
						 "image/png",
						 "default.png",
						 Path.Combine(imageDirectory, "default.png"));
	}
}
