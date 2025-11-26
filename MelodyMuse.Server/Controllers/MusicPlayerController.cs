using FluentFTP;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Controllers.Facades;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace MelodyMuse.Server.Controllers
{


    [ApiController]
    [Route("api/player")]
    public class MusicPlayerController : Controller
    {
        private readonly IMusicPlayerService _musicService;
        private readonly FtpSettings _ftpSettings;
        private readonly string _cacheDirectory;
        // 设置缓存目录的最大大小限制 (例如: 500MB)
        private readonly long _cacheSizeLimit = 500 * 1024 * 1024; // 500 MB
        private readonly UserContextFacade _userContext;

        //private readonly string _ftpServer = "101.126.23.58";
        //private readonly string _ftpUsername = "ftpuser";
        //private readonly string _ftpPassword = "tongjiORCL2024";


        public MusicPlayerController(IMusicPlayerService musicService, IOptions<FtpSettings> ftpSettings, UserContextFacade userContext)
        {

            _musicService = musicService;
            _ftpSettings = ftpSettings.Value;
            // 使用相对路径设置缓存目录
            _cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MusicCache");
            _userContext = userContext;

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
            Console.WriteLine(localFilePathMp3, ftpMp3FilePath);
            await DownloadAndCacheFileAsync($"{artistId}_{songId}.mp3", localFilePathMp3, ftpMp3FilePath);

            var fileStream = new FileStream(localFilePathMp3, FileMode.Open, FileAccess.Read);
            
            var response = new FileStreamResult(fileStream, "audio/mpeg");
            response.EnableRangeProcessing = true;

            return response;

        }

        [HttpGet("jpg")]
        public async Task<IActionResult> GetJpgFile([FromQuery] string albumId)
        {
            string localFilePathJPG;
            string ftpJpgFilePath;

            // 使用指定 albumId 的封面文件路径
            localFilePathJPG = Path.Combine(_cacheDirectory, $"{albumId}.jpg");
            ftpJpgFilePath = $"/albumCover/{albumId}/{albumId}.jpg";
            Console.WriteLine("1"+localFilePathJPG);
            Console.WriteLine("2"+ftpJpgFilePath);
            // 下载并缓存指定 albumId 的封面文件
            await DownloadAndCacheFileAsync($"{albumId}.jpg", localFilePathJPG, ftpJpgFilePath);


            // 检查文件是否存在
            if (!System.IO.File.Exists(localFilePathJPG))
            {
                return NotFound("The requested album cover was not found.");
            }

            // 打开文件流并返回文件
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

                //using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
                using (var ftp = new AsyncFtpClient(_ftpSettings.Server, _ftpSettings.Username, _ftpSettings.Password))
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
        [Authorize]
        public async Task<IActionResult> GetMusicInfo(string songId)
        {
            // 1. 【应用外观模式】一行代码获取 UserId
            // 不再需要 Request.Headers... Split... Parse... 判空...
            // 如果 Token 无效，Facade 内部会抛出异常，由全局过滤器或中间件捕获返回 401
            string userId;
            try
            {
                userId = _userContext.GetCurrentUserId();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }

            // 2. 调用 Service 获取数据
            // 注意：我们移除了外层的 try-catch，因为 Service 现在承诺不抛空引用异常
            var songMetadata = await _musicService.GetSongBySongId(songId);

            // 3. 【应用空对象模式】
            // 不再判断 songMetadata == null，而是检查业务属性 IsNull
            if (songMetadata.IsNull)
            {
                // 此时可以放心地返回 404，附带安全的空对象数据给前端（可选）
                return NotFound(new { msg = "歌曲ID不存在", data = songMetadata });
            }

            // 4. 业务逻辑：增加播放量
            await _musicService.IncreaseSongPlaysBySongIdandUserId(songId, userId);

            // 5. 业务逻辑：拼接 URL
            // 此时 songMetadata 里的字段绝对是安全的（Service 里的 Null Object 逻辑保证了 AlbumId 不为 null）
            var artistId = songMetadata.ComposerId;
            var albumId = songMetadata.AlbumId; // Service 已经保证了如果为空返回 "Default" 或空串

            songMetadata.SongUrl = $"api/player/mp3?songId={songId}&artistId={artistId}";
            songMetadata.LyricUrl = $"api/player/txt?songId={songId}&artistId={artistId}";
            songMetadata.CoverUrl = $"api/player/jpg?albumId={albumId}";

            return Ok(songMetadata);
        }
    }
}