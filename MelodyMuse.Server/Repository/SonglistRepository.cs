using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using Microsoft.EntityFrameworkCore;

public class SonglistRepository : ISonglistRepository
{
    private readonly ModelContext _context;

    public SonglistRepository()
    {
        _context = new ModelContext();
    }

    public async Task<IEnumerable<(string SonglistId, string SonglistName, int SongCount)>> GetUserSonglistsWithSongCountAsync(string userId)
    {
        Console.WriteLine($"Requested UserID: {userId}");

        // 查询该用户的所有歌单
        var userSonglists = await _context.Songlists
            .Where(s => s.UserId == userId)
            .Select(s => new
            {
                s.SonglistId,
                s.SonglistName,
                SongCount = s.Songs.Count  // 使用自动生成的多对多关系统计歌曲数量
            })
            .ToListAsync();

        if (userSonglists == null || !userSonglists.Any())
        {
            Console.WriteLine($"No songlists found for UserID: {userId}");
            return new List<(string, string, int)>();
        }

        foreach (var songlist in userSonglists)
        {
            Console.WriteLine($"SonglistID: {songlist.SonglistId}, SonglistName: {songlist.SonglistName}, SongCount: {songlist.SongCount}");
        }

        // 返回包含歌曲数量的结果
        return userSonglists.Select(s => (
            s.SonglistId,
            s.SonglistName ?? "Unnamed",
            s.SongCount
        )).ToList();
    }

    public async Task<IEnumerable<Song>> GetSongsBySonglistIdAsync(string songlistId)
    {
        return await _context.Songlists
            .Where(sl => sl.SonglistId == songlistId)
            .Include(sl => sl.Songs)
            .SelectMany(sl => sl.Songs)
            .ToListAsync();
    }
}
