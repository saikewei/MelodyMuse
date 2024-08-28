using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Configure;
using System.Text;
using System.Security.Cryptography;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/songlist")]
    public class SonglistController : Controller
    {
        private readonly ISonglistService _songlistService;

        public SonglistController(ISonglistService songlistService)
        {
            _songlistService = songlistService;
        }

        // 获取某用户的所有歌单及其包含的歌曲数量
        [Authorize]
        [HttpGet("getall")]
        public async Task<IActionResult> GetUserSonglists()
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }

            var songlists = await _songlistService.GetUserSonglistsAsync(userId);
            if (songlists == null || !songlists.Any())
            {
                return NotFound();
            }
            var songlistDtos = songlists.Select(s => new SonglistModel
            {
                SonglistId = s.SonglistId,
                SonglistName = s.SonglistName,
                SongCount = s.SongCount
            }).ToList();

            // 返回 JSON 格式的 DTO 列表
            return Ok(songlistDtos);
        }

        // 获取某歌单中的所有歌曲
        [Authorize]
        [HttpGet("{songlistId}/songs")]
        public async Task<IActionResult> GetSongsInSonglist(string songlistId)
        {
            var songs = await _songlistService.GetSongsInSonglistAsync(songlistId);
            if (songs == null || !songs.Any())
            {
                return NotFound();
            }
            return Ok(songs);
        }

        //增加歌单
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddSonglist([FromBody] CreateSonglistModel model)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }

            string songlistId;
            // 获取当前时间的字符串表示
            string currentTimeString = DateTime.UtcNow.ToString("o"); // 使用ISO 8601格式

            // 计算哈希值
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(currentTimeString));

                // 将哈希值转换为16进制字符串
                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2"));
                }

                string hashString = hashStringBuilder.ToString();

                // 截取前10位作为用户ID
                songlistId = hashString.Substring(0, 10);
            }

            // 创建一个 Songlist 实体并填充需要的属性
            var songlist = new Songlist
            {
                SonglistId = songlistId,
                SonglistName = model.SonglistName,
                SonglistDate = DateTime.Now,
                SonglistIspublic = model.IsPublic,
                UserId = userId
            };

            var newSonglistId = await _songlistService.AddSonglistAsync(songlist);
            return CreatedAtAction(nameof(GetSongsInSonglist), new { songlistId = newSonglistId }, newSonglistId);
        }

        //删除歌单
        [Authorize]
        [HttpDelete("{songlistId}/delete")]
        public async Task<IActionResult> DeleteSonglist(string songlistId)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }
            var result = await _songlistService.DeleteSonglistAsync(songlistId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // 向歌单中添加歌曲
        [Authorize]
        [HttpPost("{songlistId}/songs/{songId}/add")]
        public async Task<IActionResult> AddSongToSonglist(string songlistId, string songId)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }
            var result = await _songlistService.AddSongToSonglistAsync(songlistId, songId, userId);
            switch (result)
            {
                case 404:
                    return NotFound();  // 歌单或歌曲不存在，或用户不是创建者
                case 409:
                    return Conflict("The song is already in the songlist.");  // 歌曲已经存在于该歌单中
                case 201:
                    return CreatedAtAction(nameof(GetSongsInSonglist), new { songlistId = songlistId }, songId);  // 成功添加
                default:
                    return StatusCode(500, "An unexpected error occurred.");  // 其他错误
            }
        }

        // 从歌单中删除歌曲
        //[Authorize]
        [HttpDelete("{songlistId}/songs/{songId}/delete")]
        public async Task<IActionResult> DeleteSongFromSonglist(string songlistId, string songId)
        {
            string userId = "001";
            //try
            //{
            //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //    if (token == null)
            //    {
            //        return Unauthorized();
            //    }

            //    userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("token错误！" + ex);
            //    return Unauthorized();
            //}
            var result = await _songlistService.DeleteSongFromSonglistAsync(songlistId, songId, userId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        // 修改歌单信息
        [Authorize]
        [HttpPut("{songlistId}/update")]
        public async Task<IActionResult> UpdateSonglist(string songlistId, [FromBody] CreateSonglistModel model)
        {
            string userId;
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    return Unauthorized();
                }

                userId = TokenParser.Token2Id(token, JWTConfigure.serect_key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("token错误！" + ex);
                return Unauthorized();
            }

            var result = await _songlistService.UpdateSonglistAsync(songlistId, userId, model);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}