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
    }
}
