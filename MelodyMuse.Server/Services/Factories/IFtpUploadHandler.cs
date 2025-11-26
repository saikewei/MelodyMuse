using FluentFTP;

namespace MelodyMuse.Server.Services.Factories
{
    public interface IFtpUploadHandler
    {
        // content 是 object 类型，因为可能是 IFormFile 也可能是 string
        Task UploadAsync(AsyncFtpClient ftp, string remotePath, object content, CancellationToken token);
    }
}
