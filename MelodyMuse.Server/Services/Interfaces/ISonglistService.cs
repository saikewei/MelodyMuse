using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISonglistService
{
    Task<IEnumerable<(string SonglistId, string SonglistName, int SongCount)>> GetUserSonglistsAsync(string userId);
    Task<IEnumerable<Song>> GetSongsInSonglistAsync(string songlistId);
    Task<string> AddSonglistAsync(Songlist songlist);
    Task<bool> DeleteSonglistAsync(string songlistId, string userId);
    Task<int> AddSongToSonglistAsync(string songlistId, string songId, string userId);
    Task<bool> DeleteSongFromSonglistAsync(string songlistId, string songId, string userId);
    Task<bool> UpdateSonglistAsync(string songlistId, string userId, CreateSonglistModel model);
}
