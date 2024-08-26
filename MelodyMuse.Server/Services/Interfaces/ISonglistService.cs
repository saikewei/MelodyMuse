using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISonglistService
{
    Task<IEnumerable<(string SonglistId, string SonglistName, int SongCount)>> GetUserSonglistsAsync(string userId);
    Task<IEnumerable<Song>> GetSongsInSonglistAsync(string songlistId);
    Task<string> AddSonglistAsync(Songlist songlist);
    Task<bool> DeleteSonglistAsync(string songlistId);
    Task<bool> AddSongToSonglistAsync(string songlistId, string songId);
    Task<bool> DeleteSongFromSonglistAsync(string songlistId, string songId);
}
