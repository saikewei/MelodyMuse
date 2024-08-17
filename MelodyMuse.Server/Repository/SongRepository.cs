using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;



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

        public async Task<bool> CreateSongAsync(Song song)
        {
            _context.Songs.Add(song);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId)
        {
            return await _context.Songs
                .Include(s => s.Albums)
                .FirstOrDefaultAsync(s => s.SongName == songName && s.Albums.Any(a => a.AlbumId == albumId));
        }

        public async Task<bool> songMakeupAlbumAsync(string songId, string albumId)
        {
            var sql = "INSERT INTO SONG_MAKEUP_ALBUM (SONG_ID, ALBUM_ID) VALUES (:songId, :albumId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("songId", songId), new OracleParameter("albumId", albumId));
            return result > 0;
        }

    }
}




