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
        var songlist = await _context.Songlists
             .Include(s => s.Songs)
                 .ThenInclude(song => song.Artists)
             .FirstOrDefaultAsync(s => s.SonglistId == songlistId);

        return songlist?.Songs.ToList();
    }

    public async Task<string> AddSonglistAsync(Songlist songlist)
    {
        _context.Songlists.Add(songlist);
        await _context.SaveChangesAsync();
        return songlist.SonglistId;
    }

    public async Task<bool> DeleteSonglistAsync(string songlistId, string userId)
    {
        // 获取对应的歌单，包括其相关的歌曲
        var songlist = await _context.Songlists
            .Include(s => s.Songs)
            .FirstOrDefaultAsync(s => s.SonglistId == songlistId);

        if (songlist == null || songlist.UserId != userId)
            return false;

        // 清除该歌单内的所有歌曲
        songlist.Songs.Clear();

        // 删除歌单
        _context.Songlists.Remove(songlist);

        // 保存更改
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> AddSongToSonglistAsync(string songlistId, string songId, string userId)
    {
        var songlist = await _context.Songlists
            .Include(s => s.Songs)
            .FirstOrDefaultAsync(s => s.SonglistId == songlistId);

        if (songlist == null || songlist.UserId != userId)
        {
            // 歌单不存在或用户不是创建者，返回 NotFound
            return 404;
        }

        var song = await _context.Songs.FindAsync(songId);
        if (song == null)
        {
            // 歌曲不存在，返回 NotFound
            return 404;
        }

        if (songlist.Songs.Any(s => s.SongId == songId))
        {
            // 如果歌曲已经在该歌单中，返回 Conflict (409)
            return 409;
        }

        songlist.Songs.Add(song);
        await _context.SaveChangesAsync();
        return 201; // 成功添加，返回 Created 状态码
    }

        public async Task<bool> DeleteSongFromSonglistAsync(string songlistId, string songId, string userId)
    {
        var songlist = await _context.Songlists
            .Include(s => s.Songs)
            .FirstOrDefaultAsync(s => s.SonglistId == songlistId);

        if (songlist == null || songlist.UserId != userId) return false;

        var song = songlist.Songs.FirstOrDefault(s => s.SongId == songId);
        if (song == null) return false;

        songlist.Songs.Remove(song);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Songlist> GetSonglistByIdAsync(string songlistId)
    {
        return await _context.Songlists.FindAsync(songlistId);
    }

    public async Task UpdateSonglistAsync(Songlist songlist)
    {
        _context.Songlists.Update(songlist);
        await _context.SaveChangesAsync();
    }
    public async Task<Songlist> GetSonglistBySonglistIdAsync(string songlistId)
    {
        return await _context.Songlists
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.SonglistId == songlistId);
    }

}
