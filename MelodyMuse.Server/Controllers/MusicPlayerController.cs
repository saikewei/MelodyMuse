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
    public class LRUCache
    {
        private readonly int _capacity;
        private readonly Dictionary<string, LinkedListNode<string>> _cacheMap;
        private readonly LinkedList<string> _lruList;
       

        public LRUCache(int capacity)
        {
            _capacity = capacity;
            _cacheMap = new Dictionary<string, LinkedListNode<string>>();
            _lruList = new LinkedList<string>();
        }

        public string? Get(string key)
        {
            if (_cacheMap.TryGetValue(key, out var node))
            {
                // 将节点移动到链表头部，表示最近使用
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                return node.Value;
            }

            return null;
        }

        public void Put(string key, string filePath)
        {
            if (_cacheMap.ContainsKey(key))
            {
                // 更新现有文件的使用顺序
                _lruList.Remove(_cacheMap[key]);
                _lruList.AddFirst(_cacheMap[key]);
            }
            else
            {
                if (_cacheMap.Count >= _capacity)
                {
                    // 移除链表尾部的最久未使用文件
                    var lruKey = _lruList.Last.Value;
                    _lruList.RemoveLast();
                    _cacheMap.Remove(lruKey);

                    // 删除文件以腾出空间
                    File.Delete(lruKey);
                }

                // 插入新文件到链表头部
                var node = new LinkedListNode<string>(filePath);
                _lruList.AddFirst(node);
                _cacheMap[key] = node;
            }
        }
    }

    [ApiController]
    [Route("api/player")]
    public class MusicPlayerController : Controller
    {
        private readonly IMusicPlayerService _musicService;
       
        private readonly string _cacheDirectory;
        private readonly LRUCache _cache;

        private readonly string _ftpServer = "101.126.23.58";
        private readonly string _ftpUsername = "ftpuser";
        private readonly string _ftpPassword = "tongjiORCL2024";

        public MusicPlayerController(IMusicPlayerService musicService)
        {
            _cache = new LRUCache(60); // 假设最大缓存为60首歌
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

        private async Task DownloadAndCacheFileAsync(string cacheKey, string localFilePath, string ftpFilePath)
        {
            
            if (!System.IO.File.Exists(localFilePath) || _cache.Get(cacheKey)==null)
            {
                
                using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
                {
                    ftp.Config.DataConnectionType = FtpDataConnectionType.AutoActive;

                    await ftp.Connect();
                    _cache.Put(cacheKey, localFilePath); // 更新缓存
                    await ftp.DownloadFile(localFilePath, ftpFilePath, FtpLocalExists.Overwrite, FtpVerify.None, null, CancellationToken.None);
                    
                    await ftp.Disconnect();
                }

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

