using Microsoft.Extensions.Options;
using Adea.Exceptions;
using Adea.Interface;
using Adea.Options;

namespace Adea.Common;

public class FileUploader : IFileUploader
{
    private readonly string _uploadPath;

    public FileUploader(IOptions<AppEnvOptions> config)
    {
        _uploadPath = config.Value.UploadDestinationPath;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var filePath = Path.Combine(_uploadPath, Path.GetRandomFileName() + file.FileName);
        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
}

