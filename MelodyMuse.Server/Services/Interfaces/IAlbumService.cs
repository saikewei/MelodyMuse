using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsByArtistIdAsync(string artistId); // 根据歌手ID获取专辑
    }
}
