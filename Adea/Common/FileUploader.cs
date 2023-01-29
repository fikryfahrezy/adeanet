using Microsoft.Extensions.Options;
using Adea.Exceptions;
using Adea.Options;

namespace Adea.Common;

public class FileUploader
{

	private readonly string _uploadPath;

	public FileUploader(IOptions<AppEnvOptions> config)
	{
		try
		{
			_uploadPath = config.Value.UploadDestinationPath;
		}
		catch
		{
			throw new RequiredUploadPathException("Upload destinaion path is null");
		}
	}

	public async Task<string> UploadFileAsync(IFormFile file)
	{
		var filePath = Path.Combine(_uploadPath, Path.GetRandomFileName());
		using (var stream = System.IO.File.Create(filePath))
		{
			await file.CopyToAsync(stream);
		}

		return filePath;
	}
}

