/*
 * 与搜索功能有关的api
*/

using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;

//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]
    //注册/api/search分支路由
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly ISearchService _searchService;

        //构造函数:初始化(传入相应的服务)接口
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        //搜索歌手
        [HttpGet]
        [Route("artists")]
        public async Task<IActionResult> SearchArtists([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { msg = "请求体不能为空" });
            }
            var artists = await _searchService.SearchArtists(query);

            if (artists == null || !artists.Any())
            {
                return NotFound(new { msg = "未找到匹配的歌手" });
            }

            return Ok(artists);
        }

        //搜索歌曲
        [HttpGet]
        [Route("songs")]
        public async Task<IActionResult> SearchSongsByName([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { msg = "请求体不能为空" });
            }
            var songs = await _searchService.SearchSongsByName(query);

            if (songs == null || !songs.Any())
            {
                return NotFound(new { msg = "未找到匹配的歌曲" });
            }

            return Ok(songs);
        }

        //搜索歌词
        [HttpGet]
        [Route("lyrics")]
        public async Task<IActionResult> SearchSongsByLyrics([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { msg = "查询参数不能为空" });
            }

            var songs = await _searchService.SearchSongsByLyrics(query);

            if (songs == null || !songs.Any())
            {
                return NotFound(new { msg = "未找到匹配的歌曲" });
            }

            return Ok(songs);
        }
    }
}
