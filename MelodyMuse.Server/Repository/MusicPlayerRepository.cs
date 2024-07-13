using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Repository
{
    // 音乐播放器仓库类，实现IMusicPlayerRepository接口
    public class MusicPlayerRepository : IMusicPlayerRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public MusicPlayerRepository()
        {
            _context = new ModelContext();
        }

        // 根据歌曲ID获取歌曲信息
        public async Task<Song> GetSongBySongId(string songId)
        {
            // 在数据库中查找是否有该歌曲ID
            return await _context.Songs.FirstOrDefaultAsync(song => song.SongId == songId);
        }

        // 根据歌曲ID获取歌手信息
        public async Task<List<Artist>> GetSingersBySongId(string songId)
        {
            // 在数据库中查找相关歌手信息
            return await _context.Songs
                .Where(s => s.SongId == songId)
                .SelectMany(s => s.Artists)
                .ToListAsync();
        }
    }
}
