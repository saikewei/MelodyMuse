using FluentFTP;
using System.Text;

namespace MelodyMuse.Server.Services.Factories
{
    public class LyricsUploadHandler : IFtpUploadHandler
    {
        public async Task UploadAsync(AsyncFtpClient ftp, string remotePath, object content, CancellationToken token)
        {
            if (content is string lyricsText)
            {
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(lyricsText)))
                {
                    await ftp.UploadStream(memoryStream, remotePath, token: token);
                }
            }
            else
            {
                throw new ArgumentException("Content must be string for LyricsUploadHandler");
            }
        }

        public async Task UploadWithDirectoryAsync(AsyncFtpClient ftp, string songFolderPath, string songId, object content, CancellationToken token)
        {
            if (content is not string lyricsText || string.IsNullOrEmpty(lyricsText))
            {
                return; // 歌词为空时不上传
            }

            // 确保目录存在
            if (!await ftp.DirectoryExists(songFolderPath, token))
            {
                await ftp.CreateDirectory(songFolderPath, token);
            }

            // 生成文件路径并上传
            var lyricsPath = $"{songFolderPath}/{songId}.txt";
            await UploadAsync(ftp, lyricsPath, content, token);
        }
    }
}
