using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Repository
{
    // Refactored with chain of responsibility Pattern
    // 定义处理者接口
    public interface IAddSongValidator
    {
        IAddSongValidator SetNext(IAddSongValidator next);
        Task<int?> ValidateAsync(ModelContext context, string songlistId, string songId, string userId);
    }

    // 抽象基类
    public abstract class BaseValidator : IAddSongValidator
    {
        protected IAddSongValidator _next;
        public IAddSongValidator SetNext(IAddSongValidator next) { _next = next; return next; }

        public virtual async Task<int?> ValidateAsync(ModelContext context, string songlistId, string songId, string userId)
        {
            return _next != null ? await _next.ValidateAsync(context, songlistId, songId, userId) : null;
        }
    }

    // 具体校验环节：检查歌单所有权
    public class SonglistOwnershipValidator : BaseValidator
    {
        public override async Task<int?> ValidateAsync(ModelContext context, string songlistId, string songId, string userId)
        {
            var songlist = await context.Songlists.FindAsync(songlistId);
            if (songlist == null || songlist.UserId != userId) return 404; // 歌单不存在或无权操作
            return await base.ValidateAsync(context, songlistId, songId, userId);
        }
    }

    // 具体校验环节：检查重复
    public class DuplicateSongValidator : BaseValidator
    {
        public override async Task<int?> ValidateAsync(ModelContext context, string songlistId, string songId, string userId)
        {
            var exists = await context.Set<Dictionary<string, object>>("SongMakeupSonglist")
                               .AnyAsync(x => EF.Property<string>(x, "SongId") == songId && EF.Property<string>(x, "SonglistId") == songlistId);
            if (exists) return 409; // 冲突
            return await base.ValidateAsync(context, songlistId, songId, userId);
        }
    }

    // 具体的校验环节：检查歌曲是否存在
    public class SongExistenceValidator : BaseValidator
    {
        public override async Task<int?> ValidateAsync(ModelContext context, string songlistId, string songId, string userId)
        {
            // 检查数据库中是否有这首歌
            // 使用 AnyAsync 比 FindAsync 性能更好，因为只需要知道是否存在，不需要加载实体
            var exists = await context.Songs.AnyAsync(s => s.SongId == songId);

            if (!exists)
            {
                // 对应原代码中的：if (song == null) return 404;
                return 404;
            }

            // 如果存在，则传递给下一个校验器（例如检查是否重复）
            return await base.ValidateAsync(context, songlistId, songId, userId);
        }
    }

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
            // Refactored with chain of responsibility Pattern
            // 构建链条 (可以在构造函数中注入)
            var validatorChain = new SonglistOwnershipValidator();
            validatorChain.SetNext(new SongExistenceValidator())
                          .SetNext(new DuplicateSongValidator());

            // 执行校验
            var errorCode = await validatorChain.ValidateAsync(_context, songlistId, songId, userId);
            if (errorCode.HasValue) return errorCode.Value;

            // 校验通过，执行添加逻辑
            var songlist = await _context.Songlists.FindAsync(songlistId);
            var song = await _context.Songs.FindAsync(songId);
            songlist.Songs.Add(song);
            await _context.SaveChangesAsync();
            return 201;
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
}
