/*
* 与歌曲推荐功能有关的api
*/

using FluentFTP;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TencentCloud.Ame.V20190916.Models;
using TencentCloud.Tcr.V20190924.Models;


//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]
    //注册分支路由
    [Route("api/recommend")]

    public class RecommendController : ControllerBase
    {
        // 维护到下层服务的接口
        private readonly IRecommendService _recommendService;
        private readonly IUsersService _usersService;

        // 构造函数: 初始化(传入相应的服务)接口
        public RecommendController(IRecommendService recommendService,
            IUsersService usersService)
        {
            _recommendService = recommendService;
            _usersService = usersService;
        }

        //根据id获取播放数据
        [Authorize]
        [HttpGet]
        [Route("getdata/{userId}")]
        public async Task<IActionResult> GetSongPlayCountById(string userId)
        {
            try
            {
                var user = await _usersService.GetUserById(userId);
                if (user == null)
                {
                    // 如果用户不存在，返回404
                    return NotFound(new { msg = "用户不存在" });
                }

                //构建实例
                var songplaycount = await _recommendService.GetSongPlayCountById(userId);

                if (songplaycount == null || !songplaycount.Any())
                {
                    // 不存在时的处理逻辑
                    return NotFound(new { msg = "用户播放数据为空" });
                }
                return Ok(songplaycount);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "查询失败" + ex.Message
                };
                //返回404
                return NotFound(errorResponse);
            }
        }

        //根据id进行歌手歌曲推荐
        [Authorize]
        [HttpGet]
        [Route("byartist")]
        public async Task<IActionResult> RecommendSongsbyArtist()
        {
            try
            {
                // 从请求头中获取 JWT 令牌
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // 如果没有令牌，返回未授权错误码401
                if (token == null)
                {
                    return Unauthorized(new { msg = "未提供令牌" });
                }

                // 解析 JWT 令牌，得到存储的信息
                var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

                // 检查解析结果是否为空
                if (parsedToken == null)
                {
                    return Unauthorized(new { msg = "令牌无效" });
                }

                string userId = parsedToken.UserID;
                // 获取用户信息
                var user = await _usersService.GetUserById(userId);
                if (user == null)
                {
                    return NotFound(new { msg = "用户不存在" });
                }

                var songModelList = _recommendService.RecommendSongsbyArtist(userId);

                return Ok(songModelList);

            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "查询失败: " + ex.Message
                };
                return StatusCode(500, errorResponse); // 返回500内部服务器错误
            }
        }



        //根据id进行歌曲推荐
        [Authorize]
        [HttpGet]
        [Route("bygenre")]
        public async Task<IActionResult> RecommendSongsById()
        {
            try
            {
                // 从请求头中获取 JWT 令牌
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // 如果没有令牌，返回未授权错误码401
                if (token == null)
                {
                    return Unauthorized(new { msg = "未提供令牌" });
                }

                // 解析 JWT 令牌，得到存储的信息
                var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

                // 检查解析结果是否为空
                if (parsedToken == null)
                {
                    return Unauthorized(new { msg = "令牌无效" });
                }

                string userId = parsedToken.UserID;
                //随机数
                var random = new Random();

                var user = await _usersService.GetUserById(userId);
                if (user == null)
                {
                    // 如果用户不存在，返回404
                    return NotFound(new { msg = "用户不存在" });
                }

                var songModelList = _recommendService.RecommendSongsById(userId);

                return Ok(songModelList);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "查询失败: " + ex.Message
                };
                return NotFound(errorResponse);
            }
        }
    }
}