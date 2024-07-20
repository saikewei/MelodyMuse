using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Services;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/submit")]
    public class MusicSubmitController : ControllerBase
    {
        private readonly ICreateAlbumService _albumService;
        private readonly IUploadSongService _songService;
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumInfoService;

        // 构造函数注入ICreateAlbumService、IUploadSongService、IArtistService和IAlbumService
        public MusicSubmitController(ICreateAlbumService albumService, IUploadSongService songService, IArtistService artistService, IAlbumService albumInfoService)
        {
            _albumService = albumService;
            _songService = songService;
            _artistService = artistService;
            _albumInfoService = albumInfoService;
        }

        /// <summary>
        /// 创建空专辑的API端点
        /// </summary>
        /// <param name="albumCreateDto">包含专辑信息的数据传输对象</param>
        /// <returns>返回创建专辑的结果</returns>
        [HttpPost("createAlbum")]
        public async Task<IActionResult> CreateAlbum([FromForm] AlbumCreateModel albumCreateDto)
        {
            // 检查albumCreateDto是否为null
            if (albumCreateDto == null)
            {
                return BadRequest("Album data is required.");
            }

            // 检查专辑封面文件是否存在
            if (albumCreateDto.AlbumCover == null || albumCreateDto.AlbumCover.Length == 0)
            {
                return BadRequest("Album cover is required.");
            }

            // 调用服务层方法创建专辑
            var result = await _albumService.CreateAlbumAsync(albumCreateDto);
            if (result)
            {
                return Ok(new { message = "Album created successfully." });
            }

            return BadRequest("Error creating album.");
        }

        /// <summary>
        /// 上传歌曲的API端点
        /// </summary>
        /// <param name="songUploadDto">包含歌曲信息的数据传输对象</param>
        /// <returns>返回上传歌曲的结果</returns>
        [HttpPost("uploadSong")]
        public async Task<IActionResult> UploadSong([FromForm] SongUploadModel songUploadDto)
        {
            // 检查songUploadDto是否为null
            if (songUploadDto == null)
            {
                return BadRequest("Song data is required.");
            }

            // 检查歌曲文件是否存在
            if (songUploadDto.SongFile == null || songUploadDto.SongFile.Length == 0)
            {
                return BadRequest("Song file is required.");
            }

            // 调用服务层方法上传歌曲
            var result = await _songService.UploadSongAsync(songUploadDto);
            if (result)
            {
                return Ok(new { message = "Song uploaded successfully." });
            }

            return BadRequest("Error uploading song.");
        }

        /// <summary>
        /// 查询歌手的方法
        /// </summary>
        /// <param name="name">歌手名</param>
        /// <returns>返回查询到的歌手列表</returns>
        [HttpGet("{ArtistName}")]
        public async Task<IActionResult> SearchArtists(string ArtistName)
        {
            if (string.IsNullOrEmpty(ArtistName))
            {
                return BadRequest("Artist name is required.");
            }

            var artists = await _artistService.GetArtistsByNameAsync(ArtistName);
            return Ok(artists);
        }

        /// <summary>
        /// 获取某位歌手所有专辑的方法
        /// </summary>
        /// <returns>返回所有专辑的信息</returns>
        [HttpGet("{ArtistId}")]
        public async Task<IActionResult> GetAllAlbumsByArtistId(string artistId)
        {
            if (string.IsNullOrEmpty(artistId))
            {
                return BadRequest("Artist ID is required.");
            }

            var albums = await _albumInfoService.GetAllAlbumsByArtistIdAsync(artistId);
            return Ok(albums);
        }
    }
}
