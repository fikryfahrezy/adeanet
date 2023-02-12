using Adea.Interface;
using Microsoft.AspNetCore.Http;
namespace Adea.Tests;

public class FileUploaderFixture : IFileUploader
{
    readonly public IFormFile? fileMock = null;

    public FileUploaderFixture()
    {
        //Setup mock file using a memory stream
        // Ref: https://stackoverflow.com/questions/36858542/how-to-mock-an-iformfile-for-a-unit-integration-test-in-asp-net-core
        var content = "Hello World from a Fake File";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        //create FormFile with desired data
        fileMock = new FormFile(stream, 0, stream.Length, "id_card", fileName);
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        return Path.GetRandomFileName() + file.FileName;
    }
}


