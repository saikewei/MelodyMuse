using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;
//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定
    [ApiController]
    //注册/api/users分支路由
    [Route("api/album")]
    public class AlbumController : ControllerBase
    {
        //维护一个到下层服务的接口
       private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

   [HttpGet("{albumId}")]
    public async Task<ActionResult<AlbumsDto>> GetAlbumById(string albumId)
    {
        var albumDto = await _albumService.GetAlbumAsync(albumId);

        if (albumDto == null)
        {
            return NotFound();
        }

        return Ok(albumDto);
    }
    }
}
