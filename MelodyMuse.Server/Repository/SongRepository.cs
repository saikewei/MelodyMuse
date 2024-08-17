using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;

namespace MelodyMuse.Server.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly ModelContext _context;

        public SongRepository()
        {
            _context = new ModelContext();
        }

        // 获取待审核的歌曲
        public async Task<IEnumerable<Song>> GetPendingApprovalSongsAsync()
        {
            return await _context.Songs.Where(song => song.Status == 0).ToListAsync();
        }

        // 审核通过歌曲
        public async Task ApproveSongAsync(string songId)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.SongId == songId);
            if (song != null)
            {
                song.Status = 1; // 1 表示审核通过
                await _context.SaveChangesAsync();
            }
        }

        // 审核不通过歌曲
        public async Task RejectSongAsync(string songId)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.SongId == songId);
            if (song != null)
            {
                song.Status = 2; // 2 表示审核不通过
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Song>> GetSongsByComposerIdAsync(string composerId)
        {
            return await _context.Songs
                .Where(song => song.ComposerId == composerId)
                .ToListAsync();
        }

        public async Task<Song> GetSongByIdAsync(string songId)
        {
            return await _context.Songs
                .Include(s => s.Albums)  // Eager load the related albums
                .FirstOrDefaultAsync(s => s.SongId == songId);
        }
    }
}

