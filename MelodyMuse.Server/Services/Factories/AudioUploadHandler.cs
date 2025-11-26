using FluentFTP;

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
    }
}
