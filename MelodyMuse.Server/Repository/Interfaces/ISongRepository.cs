using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISongRepository
    {
        Task<bool> CreateSongAsync(Song song);
        Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId);

        


        Task<bool> songMakeupAlbumAsync(string songId, string albumId);

    }
}
