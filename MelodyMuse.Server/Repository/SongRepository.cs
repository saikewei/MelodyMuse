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


        public async Task<bool> DeleteMakeupSongListRecord(string songId)
        {
            // SQL 语句用于检查记录是否存在
            var checkSql = "SELECT COUNT(1) FROM SONG_MAKEUP_SONGLIST WHERE SONG_ID = :songId";

            // SQL 语句用于删除记录
            var deleteSql = "DELETE FROM SONG_MAKEUP_SONGLIST WHERE SONG_ID = :songId";

            // 创建参数
            var parameter = new OracleParameter("songId", songId);

            try
            {
                // 获取数据库连接
                var connection = (OracleConnection)_context.Database.GetDbConnection();

                // 打开连接（如果尚未打开）
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // 检查是否存在该记录
                using (var checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = checkSql;
                    checkCommand.Parameters.Add(parameter);

                    var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                    // 如果不存在记录，返回 true，因为没有要删除的内容
                    if (count == 0)
                    {
                        return true;
                    }
                }

                // 如果存在记录，则执行删除操作
                using (var deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.CommandText = deleteSql;
                    deleteCommand.Parameters.Add(parameter);

                    var result = await deleteCommand.ExecuteNonQueryAsync();

                    // 返回删除结果，是否有记录被删除
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志或处理异常
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteSongRecord(string songId)
        {
            try
            {
                // 检查 Songs 表中是否存在指定 songId 的记录
                var songRecord = await _context.Songs.FirstOrDefaultAsync(song => song.SongId == songId);

                if (songRecord != null)
                {
                    // 存在记录，进行删除操作
                    _context.Songs.Remove(songRecord);

                    // 保存更改并返回删除结果
                    return await _context.SaveChangesAsync() > 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message); // 示例日志记录或其他日志记录方式
                return false; // 返回 false 表示删除失败
            }
        }

    }
}




