using XRest.Images.App.Domain;

namespace XRest.Images.App.Services;

public interface IImageRepository
{
	ValueTask<string> GetImageMimeAsync(int imageId, ImageType imageType);
}