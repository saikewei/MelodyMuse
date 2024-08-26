/*
* 与歌曲推荐功能有关的api
*/

using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
        private readonly ISearchService _searchService;
        private readonly IUsersService _usersService;

        // 构造函数: 初始化(传入相应的服务)接口
        public RecommendController(IRecommendService recommendService, IUsersService usersService, ISearchService searchService)
        {
            _recommendService = recommendService;
            _usersService = usersService;
            _searchService = searchService;
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
        //根据id进行歌曲推荐
        [Authorize]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> RecommendSongs(string userId)
        {
            try
            {
                //随机数
                var random = new Random();

                //获取30个推荐歌曲//待更改
                //var recommendedSongs = unplayedSongs.OrderBy(x => random.Next()).Take(30).ToList();

                var user = await _usersService.GetUserById(userId);
                if (user == null)
                {
                    // 如果用户不存在，返回404
                    return NotFound(new { msg = "用户不存在" });
                }

                // 获取用户播放数据
                var userPlayCounts = await _recommendService.GetSongPlayCountById(userId);

                //由于卡顿，改为50个
                // 获取所有歌曲列表
                var allSongs = await _recommendService.GetAllSongs();

                // 提取用户已经播放过的歌曲ID列表
                var playedSongsIds = userPlayCounts.Select(spc => spc.SongId).ToList();

                // 从所有歌曲中过滤掉已经播放过的歌曲
                var playedSongs = allSongs.Where(song => playedSongsIds.Contains(song.SongId)).ToList();
                //var unplayedSongs = allSongs.Where(song => !playedSongsIds.Contains(song.SongId)).ToList();


                // 使用 HashSet 来存储推荐歌曲，避免重复
                var recommendedSongsSet = new HashSet<Song>();

                // 获取用户最常听的流派
                var preferredGenres = playedSongs
                    .GroupBy(song => song.SongGenre)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(3) // 取前3个最常听的流派
                    .ToList();

                // 推荐随机歌曲中符合用户流派的歌曲
                var genreBasedSongs = allSongs
                    .Where(song => preferredGenres.Contains(song.SongGenre))
                    .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                    .Take(15) // 取前15首
                    .ToList();

                // 将符合流派的歌曲添加到 HashSet
                foreach (var song in genreBasedSongs)
                {
                    recommendedSongsSet.Add(song);
                }

                // 如果流派推荐的歌曲不足15个，则补充推荐
                if (recommendedSongsSet.Count < 15)
                {
                    // 从未播放的歌曲中补充推荐（包括其他流派）
                    var additionalSongs = allSongs
                        .Where(song => !recommendedSongsSet.Contains(song)) // 确保不重复
                        .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                        .Take(15 - recommendedSongsSet.Count) // 补充到20个
                        .ToList();

                    // 将补充歌曲添加到 HashSet
                    foreach (var song in additionalSongs)
                    {
                        recommendedSongsSet.Add(song);
                    }
                }

                // 如果流派推荐和补充推荐的歌曲仍然不足，则从已播放的歌曲中补充推荐
                /*if (recommendedSongsSet.Count < 5)
                {
                    var additionalFromPlayed = allSongs
                        .Where(song => playedSongsIds.Contains(song.SongId)) // 已播放的歌曲
                        .Where(song => !recommendedSongsSet.Contains(song)) // 确保不重复
                        .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                        .Take( - recommendedSongsSet.Count) // 补充
                        .ToList();

                    // 将补充歌曲添加到 HashSet
                    foreach (var song in additionalFromPlayed)
                    {
                        recommendedSongsSet.Add(song);
                    }
                }*/

                // 选择完全随机的歌曲
                var randomSongs = allSongs
                    .Where(song => !recommendedSongsSet.Contains(song)) // 确保不重复
                    .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                    .Take(5)
                    .ToList();

                //合并流派推荐和完全随机推荐
                var finalRecommendedSongs = recommendedSongsSet.ToList();
                finalRecommendedSongs.AddRange(randomSongs);

                // 使用 Guid.NewGuid() 对最终推荐的歌曲列表进行随机排序
                finalRecommendedSongs = finalRecommendedSongs
                    .OrderBy(x => Guid.NewGuid())
                    .ToList();


                //以下为获取歌曲的歌手
                var songModelList = new List<SongModel>();

                foreach (var song in finalRecommendedSongs)
                {
                    // 获取每首歌的艺术家
                    var artists = await _searchService.GetArtistsBySongId(song.SongId);

                    // 创建 SongWithArtists 对象并填充数据
                    var songModelArtists = new SongModel
                    {
                        SongId = song.SongId,
                        SongName = song.SongName,
                        SongGenre = song.SongGenre,
                        Duration = song.Duration,
                        Lyrics = song.Lyrics,
                        SongDate = song.SongDate,
                        ComposerId = song.ComposerId,
                        Status = song.Status,
                        Artists = artists?.Select(a => new Artist
                        {
                            ArtistId = a.ArtistId,
                            ArtistName = a.ArtistName,
                            ArtistBirthday = a.ArtistBirthday,
                            ArtistIntro = a.ArtistIntro,
                            ArtistGenre = a.ArtistGenre,
                            ArtistFansNum = a.ArtistFansNum
                        }).ToList() // 如果 artists 为 null，结果也将为 null
                    };

                    // 将结果添加到列表中
                    songModelList.Add(songModelArtists);
                }

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
