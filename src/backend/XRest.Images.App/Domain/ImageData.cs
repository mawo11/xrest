namespace XRest.Images.App.Domain;

public class ImageData
{
	public ImageData(string mime, string fileName, string fullPath)
	{
		Mime = mime;
		FileName = fileName;
		FullPath = fullPath;
	}

	public string Mime { get; }

	public string FileName { get; }

	public string FullPath { get; }
}
