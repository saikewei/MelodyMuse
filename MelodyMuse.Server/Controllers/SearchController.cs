﻿/*
 * 与搜索功能有关的api
*/

using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using System.Web;


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

            // 对查询参数进行解码
            var decodedQuery = HttpUtility.UrlDecode(query);

            var artists = await _searchService.SearchArtists(decodedQuery);

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

            // 对查询参数进行解码
            var decodedQuery = HttpUtility.UrlDecode(query);

            var songs = await _searchService.SearchSongsByName(decodedQuery);

            if (songs == null || !songs.Any())
            {
                return NotFound(new { msg = "未找到匹配的歌曲" });
            }

            return Ok(songs);
        }


        //搜索歌曲//测试代码，songId返回歌手
        [HttpGet]
        [Route("artist_song")]
        public async Task<IActionResult> GetArtistsBySongId([FromQuery] string songId)
        {
            try
            {
                var artists = await _searchService.GetArtistsBySongId(songId);
                return Ok(artists);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //由于歌词移动，该接口已经废弃

        //搜索歌词
        [HttpGet]
        [Route("lyrics")]
        public async Task<IActionResult> SearchSongsByLyrics([FromQuery] string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new { msg = "查询参数不能为空" });
            }
            // 对查询参数进行解码
            var decodedQuery = HttpUtility.UrlDecode(query);

            var songs = await _searchService.SearchSongsByLyrics(decodedQuery);

            if (songs == null || !songs.Any())
            {
                return NotFound(new { msg = "未找到匹配的歌曲" });
            }

            return Ok(songs);
        }
    }
}