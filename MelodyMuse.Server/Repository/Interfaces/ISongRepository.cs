using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISongRepository
    {
        Task<bool> CreateSongAsync(Song song);
        Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId);
        
    }
}
