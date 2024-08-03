using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IAlbumRepository
    {

        Task<bool> CreateAlbumAsync(Album album);
        Task<Album> GetAlbumByIdAsync(string albumId);  //根据专辑ID获取指定专辑
        Task<IEnumerable<Album>> GetAllAlbumsByArtistIdAsync(string artistId); // 根据歌手ID获取专辑
    }
}
