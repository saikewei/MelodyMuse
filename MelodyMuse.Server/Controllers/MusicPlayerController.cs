using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using FluentFTP;
using System.Net.Http.Headers;
using MelodyMuse.Server.Models;
using System.Text;
using TencentCloud.Tiia.V20190529.Models;

namespace MelodyMuse.Server.Controllers
{
   

    [ApiController]
    [Route("api/player")]
    public class MusicPlayerController : Controller
    {
        private readonly IMusicPlayerService _musicService;
       
        private readonly string _cacheDirectory;
        // 设置缓存目录的最大大小限制 (例如: 500MB)
        private readonly long _cacheSizeLimit = 500 * 1024 * 1024; // 500 MB

        private readonly string _ftpServer = "101.126.23.58";
        private readonly string _ftpUsername = "ftpuser";
        private readonly string _ftpPassword = "tongjiORCL2024";

        public MusicPlayerController(IMusicPlayerService musicService)
        {
            
            _musicService = musicService;
           
            // 使用相对路径设置缓存目录
            _cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MusicCache");

            // 确保缓存目录存在
            if (!Directory.Exists(_cacheDirectory))
            {
                Directory.CreateDirectory(_cacheDirectory);
            }
        }

        [HttpGet("mp3")]
        public async Task<IActionResult> GetMp3File([FromQuery] string songId, [FromQuery] string artistId)
        {
            var localFilePathMp3 = Path.Combine(_cacheDirectory, $"{artistId}_{songId}.mp3");
            var ftpMp3FilePath = $"/songs/{artistId}/{songId}/{songId}.mp3";

            await DownloadAndCacheFileAsync($"{artistId}_{songId}.mp3", localFilePathMp3, ftpMp3FilePath);

            var fileStream = new FileStream(localFilePathMp3, FileMode.Open, FileAccess.Read);

            var response = new FileStreamResult(fileStream, "audio/mpeg");
            response.EnableRangeProcessing = true;

            return response;
            
        }

        [HttpGet("jpg")]
        public async Task<IActionResult> GetJpgFile([FromQuery] string albumId)
        {
            var localFilePathJPG = Path.Combine(_cacheDirectory, $"{albumId}.jpg");
            var ftpJpgFilePath = $"/albumCover/{albumId}/{albumId}.jpg";

            await DownloadAndCacheFileAsync($"{albumId}.jpg", localFilePathJPG, ftpJpgFilePath);

            var fileStream = new FileStream(localFilePathJPG, FileMode.Open, FileAccess.Read);
            return File(fileStream, "image/jpeg");
        }

        [HttpGet("txt")]
        public async Task<IActionResult> GetTxtFile([FromQuery] string songId, [FromQuery] string artistId)
        {
            var localFilePathTxt = Path.Combine(_cacheDirectory, $"{artistId}_{songId}.txt");
            var ftpTxtFilePath = $"/songs/{artistId}/{songId}/{songId}.txt";

            await DownloadAndCacheFileAsync($"{artistId}_{songId}.txt", localFilePathTxt, ftpTxtFilePath);

            var fileStream = new FileStream(localFilePathTxt, FileMode.Open, FileAccess.Read);
            return File(fileStream, "text/plain");
        }

        // 管理缓存大小，删除最近最少使用的文件
        private void ManageCacheSize()
        {
            var cacheFiles = Directory.GetFiles(_cacheDirectory).Select(f => new FileInfo(f)).OrderBy(f => f.LastAccessTime).ToList();
            long totalCacheSize = cacheFiles.Sum(f => f.Length);

            // 如果缓存大小超过限制，按LRU策略删除文件
            while (totalCacheSize > _cacheSizeLimit && cacheFiles.Any())
            {
                var oldestFile = cacheFiles.First();
                totalCacheSize -= oldestFile.Length;
                oldestFile.Delete();
                cacheFiles.RemoveAt(0);
            }
        }
        private async Task DownloadAndCacheFileAsync(string cacheKey, string localFilePath, string ftpFilePath)
        {
            // 如果文件不存在或缓存过期，下载文件
            if (!System.IO.File.Exists(localFilePath))
            {
                // 在下载前检查缓存大小，并清理缓存
                ManageCacheSize();

                using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
                {
                    ftp.Config.DataConnectionType = FtpDataConnectionType.AutoActive;

                    await ftp.Connect();
                    await ftp.DownloadFile(localFilePath, ftpFilePath, FtpLocalExists.Overwrite, FtpVerify.None, null, CancellationToken.None);
                    await ftp.Disconnect();
                }

                // 更新文件的最后访问时间
                System.IO.File.SetLastAccessTime(localFilePath, DateTime.Now);
            }
        }


        [HttpGet]
        [Route("{songId}")]
        public async Task<IActionResult> GetMusicInfo(string songId)
        {
            try
            {
                var songMetadata = await _musicService.GetSongBySongId(songId);
                var artistId = songMetadata.ComposerId;
                var albumId = songMetadata.AlbumId;
                if (songMetadata == null)
                {
                    // 歌曲ID不存在时的处理逻辑
                    return NotFound("歌曲ID不存在");
                }

                songMetadata.SongUrl = $"api/player/mp3?songId={songId}&artistId={artistId}"; // 更新URL指向文件流方法
                songMetadata.LyricUrl = $"api/player/txt?songId={songId}&artistId={artistId}"; // 更新URL指向文件流方法
                songMetadata.CoverUrl = $"api/player/jpg?albumId={albumId}"; // 更新URL指向文件流方法
                return Ok(songMetadata);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "歌曲查询错误：" + ex.Message
                };
                return NotFound(errorResponse);
            }
        }
    }
}

