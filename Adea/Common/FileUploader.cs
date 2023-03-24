using Adea.Interface;
using Adea.Options;
using Microsoft.Extensions.Options;

namespace Adea.Common;

public class FileUploader : IFileUploader
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IOptions<AppEnvOptions> _config;

    public FileUploader(IOptions<AppEnvOptions> config, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _config = config;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var filePath = Path.Combine(
            _webHostEnvironment.WebRootPath,
            _config.Value.UploadDestinationDirname,
            Path.GetRandomFileName() + "-" + file.FileName
        );
        using (var stream = File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
}

