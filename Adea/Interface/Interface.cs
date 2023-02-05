namespace Adea.Interface;
public interface IFileUploader
{
    public Task<string> UploadFileAsync(IFormFile file);
}
