using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SonglistService : ISonglistService
{
    private readonly ISonglistRepository _songlistRepository;

    public SonglistService(ISonglistRepository songlistRepository)
    {
        _songlistRepository = songlistRepository;
    }

    public async Task<IEnumerable<(string SonglistId, string SonglistName, int SongCount)>> GetUserSonglistsAsync(string userId)
    {
        return await _songlistRepository.GetUserSonglistsWithSongCountAsync(userId);
    }
    public async Task<IEnumerable<Song>> GetSongsInSonglistAsync(string songlistId)
    {
        return await _songlistRepository.GetSongsBySonglistIdAsync(songlistId);
    }
    public async Task<string> AddSonglistAsync(Songlist songlist)
    {
        return await _songlistRepository.AddSonglistAsync(songlist);
    }

    public async Task<bool> DeleteSonglistAsync(string songlistId, string userId)
    {
        return await _songlistRepository.DeleteSonglistAsync(songlistId, userId);
    }

    public async Task<int> AddSongToSonglistAsync(string songlistId, string songId, string userId)
    {
        return await _songlistRepository.AddSongToSonglistAsync(songlistId, songId, userId);
    }

    public async Task<bool> DeleteSongFromSonglistAsync(string songlistId, string songId, string userId)
    {
        return await _songlistRepository.DeleteSongFromSonglistAsync(songlistId, songId, userId);
    }
    public async Task<bool> UpdateSonglistAsync(string songlistId, string userId, CreateSonglistModel model)
    {
        var songlist = await _songlistRepository.GetSonglistByIdAsync(songlistId);

        if (songlist == null || songlist.UserId != userId)
        {
            return false;
        }

        // 更新歌单信息
        songlist.SonglistName = model.SonglistName;
        songlist.SonglistIspublic = model.IsPublic;

        await _songlistRepository.UpdateSonglistAsync(songlist);

        return true;
    }
}
