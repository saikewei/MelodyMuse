using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface ICreateAlbumService
    {
        Task<bool> CreateAlbumAsync(AlbumCreateModel albumCreateDto); //操作是否成功
    }
}
