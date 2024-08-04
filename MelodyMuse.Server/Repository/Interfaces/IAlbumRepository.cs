using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IAlbumRepository
    {

        Task<bool> CreateAlbumAsync(Album album);
        Task<Album> GetAlbumByIdAsync(string albumId);  //根据专辑ID获取指定专辑


        Task<IEnumerable<AlbumDto>> GetAllAlbumsByArtistIdAsync(string artistId); // 根据歌手ID获取专辑

        Task<List<string>> GetSongsByAlbumIdAsync(string albumId);//根据专辑ID获取所有与之关联的歌曲

    }
}
