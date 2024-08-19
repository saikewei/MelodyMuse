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
}
