using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;
using Microsoft.AspNetCore.Authorization; // 如果是 ASP.NET Core

//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]

    [Route("api/rank")]
    public class RankingController : ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly IRankingService _RankingService;

        //构造函数:初始化(传入相应的服务)接口
        public  RankingController(IRankingService  RankingService)
        {
            _RankingService =  RankingService;
        }

        [Authorize] 
         [HttpGet("top-songs")]
    public async Task<IActionResult> GetTopSongs()
    {
        var topSongs = await _RankingService.GetTopSongsAsync();
        return Ok(topSongs);
    }
            [Authorize] 
        [HttpGet("ranking")]
        public async Task<IActionResult> GetArtistRanking()
        {
            var ranking = await _RankingService.GetArtistRankingAsync();
            return Ok(ranking);
        }

    }

}