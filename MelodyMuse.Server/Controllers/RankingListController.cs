using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;

//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]

    [Route("api/rank")]
    public class RankingListController : ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly IRankingListService _RankingListService;

        //构造函数:初始化(传入相应的服务)接口
        public  RankingListController(IRankingListService  RankingListService)
        {
            _RankingListService =  RankingListService;
        }


        [HttpGet("with-playcount")]
         public async Task<IActionResult> GetSongsWithPlayCount()
    {
        var songsWithPlayCount = await _RankingListService.GetSongsWithPlayCountAsync();
        return Ok(songsWithPlayCount);
    }
    }

}