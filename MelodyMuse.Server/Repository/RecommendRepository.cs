using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MelodyMuse.Server.Repository
{
    public class RecommendRepository : IRecommendRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public RecommendRepository()
        {
            _context = new ModelContext();
        }
        //根据用户id获取播放基本信息
        public async Task<List<SongPlayCount>> GetSongPlayCountById(string userId)
        {
            var songplaycount = await _context.SongPlayCounts
                .Where(u => u.UserId == userId)
                .ToListAsync();

            if (songplaycount == null)
            {
                return null; // 返回 null 表示用户不存在
            }
            return songplaycount;
        }
        //获取所有歌曲
        public async Task<List<Song>> GetAllSongs()
        {
            // 从数据库中获取所有歌曲
            var songs = await _context.Songs
                .OrderBy(x => Guid.NewGuid()) // 随机排序
                .Take(50) // 选择前 count 条记录
                .ToListAsync();
            return songs;
            //return await _context.Songs.ToListAsync();
        }
    }
}
