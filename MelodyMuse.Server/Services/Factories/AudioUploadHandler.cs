using FluentFTP;
// Refactored with factory Pattern
namespace MelodyMuse.Server.Services.Factories
{
    public class AudioUploadHandler : IFtpUploadHandler
    {
        public async Task UploadAsync(AsyncFtpClient ftp, string remotePath, object content, CancellationToken token)
        {
            if (content is IFormFile file)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    // 封装了具体的 FTP 库调用
                    await ftp.UploadStream(memoryStream, remotePath, token: token);
                }
            }
            else
            {
                throw new ArgumentException("Content must be IFormFile for AudioUploadHandler");
            }
        }

        public async Task UploadWithDirectoryAsync(AsyncFtpClient ftp, string songFolderPath, string songId, object content, CancellationToken token)
        {
            if (content is not IFormFile file)
            {
                throw new ArgumentException("Content must be IFormFile for AudioUploadHandler");
            }

            // 确保目录存在
            if (!await ftp.DirectoryExists(songFolderPath, token))
            {
                await ftp.CreateDirectory(songFolderPath, token);
            }

            // 生成文件路径并上传
            var fileName = $"{songId}{Path.GetExtension(file.FileName)}";
            var audioPath = $"{songFolderPath}/{fileName}";

            await UploadAsync(ftp, audioPath, content, token);
        }
    }
}
