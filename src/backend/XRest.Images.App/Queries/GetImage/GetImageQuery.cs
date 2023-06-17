using XRest.Images.App.Domain;
using MediatR;

namespace XRest.Images.App.Queries.GetImage;

public record GetImageQuery(ImageType ImageType, int ImageId) : IRequest<ImageData>;
