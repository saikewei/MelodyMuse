using FluentFTP;

// Refactored with factory Pattern
namespace MelodyMuse.Server.Services.Factories
{
    public interface IFtpUploadHandler
    {
        // content 是 object 类型，因为可能是 IFormFile 也可能是 string
        Task UploadAsync(AsyncFtpClient ftp, string remotePath, object content, CancellationToken token);

        // 完整的上传流程，包括目录创建和文件上传
        Task UploadWithDirectoryAsync(AsyncFtpClient ftp, string songFolderPath, string songId, object content, CancellationToken token);
    }
}
