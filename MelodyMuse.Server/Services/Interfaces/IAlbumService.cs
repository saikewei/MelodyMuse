using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumDto>> GetAllAlbumsByArtistIdAsync(string artistId); // 根据歌手ID获取专辑
        Task<AlbumsDto> GetAlbumAsync(string albumId);
    }
}
